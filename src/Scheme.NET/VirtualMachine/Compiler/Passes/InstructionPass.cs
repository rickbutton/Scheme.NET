using Scheme.NET.Numbers;
using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheme.NET.VirtualMachine.Compiler.Passes
{
    public class InstructionPass : PassBase<ISExpression, IInstruction>
    {
        public override IInstruction Compile(ISchemeVM vm, ISExpression expr)
        {
            return Compile(vm, expr, new HaltInstruction());
        }

        public static IInstruction Compile(ISchemeVM vm, ISExpression expr, IInstruction next)
        {
            if (expr.IsSymbol())
            {
                return new ReferInstruction(expr, next);
            }
            else if (expr.IsCons())
            {
                return CompileCons(vm, expr, next);
            }
            else if (expr.IsProcedure())
            {
                return new NativeInstruction(expr as Procedure, new ReturnInstruction());
            }

            return new ConstantInstruction(expr, next);
        }

        private static IInstruction CompileCons(ISchemeVM vm, ISExpression expr, IInstruction next)
        {
            var x = expr as Cons;

            if (!x.IsList())
            {
                return new ConstantInstruction(x, next);
            }

            var op = x.Get(0);

            /*if (op == CompilerConstants.Eval)
            {
                if (x.ListCount() != 3)
                    ThrowErr("eval", "invalid number of arguments", x.String());

                var e = x.Get(1);

                if (e.IsList() && e.GetUnsafeCar() == CompilerConstants.Quote)
                    e = e.GetUnsafeCdr().GetUnsafeCar();

                var env = x.Get(2);

                if (!env.IsList() || env.ListCount() != 2)
                    ThrowErr("eval", "invalid argument 'environment specifier'", x.String());

                var specifier = env.GetUnsafeCar();
                if (!(specifier == CompilerConstants.SchemeReportEnvironment || specifier == CompilerConstants.NullEnvironment))
                    ThrowErr("eval", "invalid argument 'environment specifier'", x.String());

                var version = env.GetUnsafeCdr().GetUnsafeCar();
                if (!version.IsInteger() || ((NumberAtom)version).Val != Complex.CreateExactReal(5))
                    ThrowErr("eval", "invalid version for argument 'environment specifier'", x.String());

                var populate = specifier == CompilerConstants.SchemeReportEnvironment;

                return new FrameInstruction(next, new EnvironmentInstruction(populate, Compile(e, new ReturnInstruction())));
            }*/
            if (op == CompilerConstants.SchemeReportEnvironment || op == CompilerConstants.NullEnvironment)
            {
                var version = x.Get(1);
                if (!version.IsInteger() || ((NumberAtom)version).Val != Complex.CreateExactReal(5))
                    ThrowErr("eval", "invalid version for argument 'environment specifier'", x.String());

                var populate = op == CompilerConstants.SchemeReportEnvironment;
                return new EnvironmentInstruction(populate, next);
            }
            if (op == CompilerConstants.Quote)
            {
                if (x.ListCount() != 2)
                    ThrowErr("quote", "invalid number of arguments", x.String());

                return new ConstantInstruction(x.Cdr.GetUnsafeCar(), next);
            }
            else if (op == CompilerConstants.Lambda)
            {
                if (x.ListCount() < 3)
                    ThrowErr("lambda", "invalid number of arguments", x.String());

                var vars = x.Get(1);
                var body = x.GetUnsafeCdr().GetUnsafeCdr();
                return new CloseInstruction(vars, CompileLambdaBody(vm, body, new ReturnInstruction()), next);
            }
            else if (op == CompilerConstants.Begin)
            {
                var body = x.Cdr;
                if (body == AtomHelper.Nil)
                    return new ConstantInstruction(body, next);

                return CompileLambdaBody(vm, body, next);
            }
            else if (op == CompilerConstants.If)
            {
                var c = x.ListCount();

                var test = x.Get(1);

                IInstruction tc, ec;

                var then = x.Get(2);
                tc = Compile(vm, then, next);
                var elsa = x.Get(3);
                ec = Compile(vm, elsa, next);

                return Compile(vm, test, new TestInstruction(tc, ec));
            }
            else if (op == CompilerConstants.SetBang)
            {
                if (x.ListCount() != 3)
                    ThrowErr("set!", "invalid number of arguments", x.String());

                var set_v = x.Get(1);
                var set_expr = x.Get(2);

                CheckNotIllegalSymbol(set_v);

                return Compile(vm, set_expr, new AssignInstruction(set_v, next));
            }
            else if (op == CompilerConstants.Define)
            {
                if (x.ListCount() != 3)
                    ThrowErr("define", "invalid number of arguments", x.String());

                var def_v = x.Get(1);

                CheckNotIllegalSymbol(def_v);

                if (def_v.IsSymbol())
                {
                    var def_expr = x.Get(2);
                    return Compile(vm, def_expr, new DefineInstruction(def_v, next));
                }
                else if (def_v.IsCons())
                {
                    var v = def_v.GetUnsafeCar();
                    var formals = def_v.GetUnsafeCdr();
                    return new CloseInstruction(formals, CompileLambdaBody(vm, x.GetUnsafeCdr().GetUnsafeCdr(), 
                        new ReturnInstruction()), new DefineInstruction(v, next));
                }

                ThrowErr("define", "invalid argument '<variable> <formals>'", x.String());
                return null;
            }
            else if (op == CompilerConstants.CallCC)
            {
                if (x.ListCount() != 2)
                    ThrowErr("call/cc", "invalid number of arguments", x.String());

                var e = x.Get(1);
                var c = new ContiInstruction(new ArgumentInstruction(Compile(vm, e, new ApplyInstruction())));

                // tail?
                if (next.Name == "return")
                    return c;
                else
                    return new FrameInstruction(next, c);
            }
            else
            {
                var args = x.Cdr;

                var c = Compile(vm, x.Car, new ApplyInstruction());
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
                    c = Compile(vm, ((Cons)args).Car, new ArgumentInstruction(c));
                    args = ((Cons)args).Cdr;
                }
            }
        }

        private static IInstruction CompileLambdaBody(ISchemeVM vm, ISExpression body, IInstruction next)
        {
            if (body.GetUnsafeCdr() == AtomHelper.Nil)
                return Compile(vm, body.GetUnsafeCar(), next);
            return Compile(vm, body.GetUnsafeCar(), CompileLambdaBody(vm, body.GetUnsafeCdr(), next));
        }
    }
}
