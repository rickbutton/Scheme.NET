using Scheme.NET.Numbers;
using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine.Instructions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Compiler
{
    public static class SchemeCompiler
    {
        private static readonly ISExpression Quote = AtomHelper.SymbolFromString("quote");
        private static readonly ISExpression Let = AtomHelper.SymbolFromString("let");
        private static readonly ISExpression LetStar = AtomHelper.SymbolFromString("let*");
        private static readonly ISExpression Lambda = AtomHelper.SymbolFromString("lambda");
        private static readonly ISExpression If = AtomHelper.SymbolFromString("if");
        private static readonly ISExpression SetBang = AtomHelper.SymbolFromString("set!");
        private static readonly ISExpression Define = AtomHelper.SymbolFromString("define");
        private static readonly ISExpression Eval = AtomHelper.SymbolFromString("eval");
        private static readonly ISExpression CallCC = AtomHelper.SymbolFromString("call/cc");
        private static readonly ISExpression Begin = AtomHelper.SymbolFromString("begin");

        private static readonly ISExpression SchemeReportEnvironment = AtomHelper.SymbolFromString("scheme-report-environment");
        private static readonly ISExpression NullEnvironment = AtomHelper.SymbolFromString("null-environment");

        public static IInstruction Compile(ISExpression expr)
        {
            return Compile(expr, new HaltInstruction());
        }

        public static IInstruction Compile(ISExpression expr, IInstruction next)
        {
            if (expr.IsSymbol())
            {
                return new ReferInstruction(expr, next);
            }
            else if (expr.IsCons())
            {
                return CompileCons(expr, next);
            }
            else if (expr.IsProcedure())
            {
                return new NativeInstruction(expr as Procedure, new ReturnInstruction());
            }

            return new ConstantInstruction(expr, next);
        }

        private static IInstruction CompileCons(ISExpression expr, IInstruction next)
        {
            var x = expr as Cons;

            if (!x.IsList())
            {
                return new ConstantInstruction(x, next);
            }

            var op = x.Get(0);

            if (op == Eval)
            {
                if (x.ListCount() != 3)
                    ThrowErr("eval", "invalid number of arguments", x.String());

                var e = x.Get(1);

                if (!e.IsList())
                    ThrowErr("eval", "invalid argument 'expression'", x.String());

                if (e.GetUnsafeCar() != Quote)
                    ThrowErr("eval", "invalid argument 'expression'", x.String());

                var env = x.Get(2);

                if (!env.IsList() || env.ListCount() != 2)
                    ThrowErr("eval", "invalid argument 'environment specifier'", x.String());

                var specifier = env.GetUnsafeCar();
                if (!(specifier == SchemeReportEnvironment || specifier == NullEnvironment))
                    ThrowErr("eval", "invalid argument 'environment specifier'", x.String());

                var version = env.GetUnsafeCdr().GetUnsafeCar();
                if (!version.IsInteger() || ((NumberAtom)version).Val != Complex.CreateExactReal(5))
                    ThrowErr("eval", "invalid version for argument 'environment specifier'", x.String());

                var populate = specifier == SchemeReportEnvironment;

                var body = (e as Cons).Get(1);
                return new FrameInstruction(next, new EnvironmentInstruction(populate, Compile(body, new ReturnInstruction())));
            }
            if (op == Quote)
            {
                if (x.ListCount() != 2)
                    ThrowErr("quote", "invalid number of arguments", x.String());

                return new ConstantInstruction(x.Cdr.GetUnsafeCar(), next);
            }
            else if (op == Let)
            {
                return Compile(RewriteLet(expr), next);
            }
            else if (op == LetStar)
            {
                return Compile(RewriteLetStar(expr), next);
            }
            else if (op == Lambda)
            {
                if (x.ListCount() < 3)
                    ThrowErr("lambda", "invalid number of arguments", x.String());

                var vars = x.Get(1);
                var body = x.GetUnsafeCdr().GetUnsafeCdr();
                return new CloseInstruction(vars, CompileLambdaBody(body, new ReturnInstruction()), next);
            }
            else if (op == Begin)
            {
                var body = x.Cdr;
                if (body == AtomHelper.Nil)
                    return new ConstantInstruction(body, next);

                return CompileLambdaBody(body, next);
            }
            else if (op == If)
            {
                var c = x.ListCount();
                if (c < 3 || c > 4)
                    ThrowErr("if", "invalid number of arguments", x.String());

                var test = x.Get(1);

                IInstruction tc, ec;

                var then = x.Get(2);
                tc = Compile(then, next);

                if (c == 4)
                {
                    var elsa = x.Get(3);
                    ec = Compile(elsa, next);
                }
                else
                {
                    ec = new ConstantInstruction(AtomHelper.Nil, next);
                }

                return Compile(test, new TestInstruction(tc, ec));
            }
            else if (op == SetBang)
            {
                if (x.ListCount() != 3)
                    ThrowErr("set!", "invalid number of arguments", x.String());

                var set_v = x.Get(1);
                var set_expr = x.Get(2);

                return Compile(set_expr, new AssignInstruction(set_v, next));
            }
            else if (op == Define)
            {
                if (x.ListCount() != 3)
                    ThrowErr("define", "invalid number of arguments", x.String());

                var def_v = x.Get(1);

                if (def_v.IsSymbol())
                {
                    var def_expr = x.Get(2);
                    return Compile(def_expr, new DefineInstruction(def_v, next));
                }
                else if (def_v.IsCons())
                {
                    var v = def_v.GetUnsafeCar();
                    var formals = def_v.GetUnsafeCdr();
                    return new CloseInstruction(formals, CompileLambdaBody(x.GetUnsafeCdr().GetUnsafeCdr(), 
                        new ReturnInstruction()), new DefineInstruction(v, next));
                }

                ThrowErr("define", "invalid argument '<variable> <formals>'", x.String());
                return null;
            }
            else if (op == CallCC)
            {
                if (x.ListCount() != 2)
                    ThrowErr("call/cc", "invalid number of arguments", x.String());

                var e = x.Get(1);
                var c = new ContiInstruction(new ArgumentInstruction(Compile(e, new ApplyInstruction())));

                // tail?
                if (next.Name == "return")
                    return c;
                else
                    return new FrameInstruction(next, c);
            }
            else
            {
                var args = x.Cdr;

                var c = Compile(x.Car, new ApplyInstruction());
                while (true)
                {
                    if (args == AtomHelper.Nil)
                    {
                        // tail?
                        if (next.Name == "return")
                        {
                            return c;
                        }
                        else
                        {
                            return new FrameInstruction(next, c);
                        }
                    }
                    c = Compile(((Cons)args).Car, new ArgumentInstruction(c));
                    args = ((Cons)args).Cdr;
                }
            }
        }

        private static IInstruction CompileLambdaBody(ISExpression body, IInstruction next)
        {
            if (body.GetUnsafeCdr() == AtomHelper.Nil)
                return Compile(body.GetUnsafeCar(), next);
            return Compile(body.GetUnsafeCar(), CompileLambdaBody(body.GetUnsafeCdr(), next));
        }

        private static ISExpression RewriteLet(ISExpression expr)
        {
            List<ISExpression> vars = new List<ISExpression>(), vals = new List<ISExpression>();
            ISExpression defs;
            var x = expr as Cons;
            var c = x.ListCount();

            if (c < 3)
                ThrowErr("let", "invalid number of arguments", expr.String());

            //  (let name ((var val) ...) body)
            if (x.Get(1).IsSymbol())
            {
                defs = x.Get(2);
                while (defs != AtomHelper.Nil)
                {
                    vars.Add(defs.GetUnsafeCar().GetUnsafeCar());
                    vals.Add(defs.GetUnsafeCar().GetUnsafeCdr().GetUnsafeCar());
                    defs = defs.GetUnsafeCdr();
                }

                // ((lambda ()
                //   (define name (lambda (vars) body))
                //   (name vals)))
                return AtomHelper.CreateList(
                        AtomHelper.CreateList(Lambda, AtomHelper.CreateList(),
                            AtomHelper.CreateList(Define, x.Get(1), AtomHelper.CreateList(Lambda, vars.Unflatten(), x.Get(3))),
                            AtomHelper.CreateCons(x.Get(1), vals.Unflatten())
                        )
                       );
            }
            // (let ((var val) ...) body)
            else if (x.Get(1).IsList())
            {
                defs = x.Get(1);

                while (defs != AtomHelper.Nil)
                {
                    var d = defs.GetUnsafeCar();

                    if (!d.IsList() || d.ListCount() != 2)
                        ThrowErr("let", "invalid argument 'bindings'", x.String());

                    var n = d.GetUnsafeCar();
                    var v = d.GetUnsafeCdr().GetUnsafeCar();

                    if (!n.IsSymbol())
                        ThrowErr("let", "invalid argument 'bindings'", x.String());

                    vars.Add(n);
                    vals.Add(v);
                    defs = defs.GetUnsafeCdr();
                }
                // ((lambda (vars) body) vals)
                return AtomHelper.CreateCons(AtomHelper.CreateList(Lambda, vars.Unflatten(), x.Get(2)), vals.Unflatten());
            }

            ThrowErr("let", "invalid argument 'bindings'", x.String());
            return null;
        }

        private static ISExpression RewriteLetStar(ISExpression expr)
        {
            var x = expr as Cons;
            var c = x.ListCount();

            if (c < 3)
                ThrowErr("let*", "invalid number of arguments", expr.String());

            var bindings = x.Get(1);
            var body = x.Get(2);

            if (!bindings.IsList())
                ThrowErr("let*", "invalid argument 'bindings'", expr.String());

            if (bindings == AtomHelper.Nil || bindings.GetUnsafeCdr() == AtomHelper.Nil)
                return RewriteLet(expr);

            var first = bindings.GetUnsafeCar();
            var remain = bindings.GetUnsafeCdr();
            return RewriteLet(AtomHelper.CreateList(Let, AtomHelper.CreateList(first),
                AtomHelper.CreateList(LetStar, remain, body)));
        }

        private static void ThrowErr(string name, string msg, string rep) { throw new SchemeCompilerException(name, msg, rep); }
    }
}
