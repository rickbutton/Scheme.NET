using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheme.NET.VirtualMachine.Compiler.Passes
{
    public class RewriteLetPass : PassBase<ISExpression, ISExpression>
    {
        public override ISExpression Compile(ISchemeVM vm, ISExpression expr)
        {
            if (expr.IsList() && expr != AtomHelper.Nil)
            {
                var op = (expr as Cons).Get(0);

                if (op == CompilerConstants.Let)
                {
                    return RewriteLet(vm, expr);
                }
                else if (op == CompilerConstants.LetStar)
                {
                    return RewriteLetStar(vm, expr);
                }
            }

            if (expr.IsCons())
            {
                var c = expr as Cons;
                return new Cons(Compile(vm, c.Car), Compile(vm, c.Cdr));
            }

            return expr;
        }

        private ISExpression RewriteLetStar(ISchemeVM vm, ISExpression expr)
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
                return RewriteLet(vm, expr);

            var first = bindings.GetUnsafeCar();
            var remain = bindings.GetUnsafeCdr();
            return RewriteLet(vm, AtomHelper.CreateList(CompilerConstants.Let, AtomHelper.CreateList(Compile(vm, first)),
                AtomHelper.CreateList(CompilerConstants.LetStar, Compile(vm, remain), Compile(vm, body))));
        }



        private ISExpression RewriteLet(ISchemeVM vm, ISExpression expr)
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
                var sym = x.Get(1);
                defs = x.Get(2);
                while (defs != AtomHelper.Nil)
                {
                    var n = defs.GetUnsafeCar().GetUnsafeCar();
                    var v = defs.GetUnsafeCar().GetUnsafeCdr().GetUnsafeCar();

                    if (!n.IsSymbol())
                        ThrowErr("let", "invalid argument 'bindings'", x.String());

                    vars.Add(n);
                    vals.Add(v);
                    defs = defs.GetUnsafeCdr();
                }

                // ((lambda ()
                //   (define name (lambda (vars) body))
                //   (name vals)))
                var body = x.GetUnsafeCdr().GetUnsafeCdr().GetUnsafeCdr();
                var innerLambda = AtomHelper.CreateCons(
                    CompilerConstants.Lambda,
                    AtomHelper.CreateCons(vars.Unflatten(),
                    Compile(vm, body))
                );
                var define = AtomHelper.CreateList(CompilerConstants.Define, sym, innerLambda);
                var loopCall = AtomHelper.CreateCons(sym, Compile(vm, vals.Unflatten()));
                var outerLambda = AtomHelper.CreateList(CompilerConstants.Lambda, AtomHelper.Nil, define, loopCall);

                return AtomHelper.CreateList(outerLambda);
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
                var body = x.GetUnsafeCdr().GetUnsafeCdr();

                var lambda = AtomHelper.CreateCons(CompilerConstants.Lambda,
                             AtomHelper.CreateCons(vars.Unflatten(), Compile(vm, body)));
                return AtomHelper.CreateCons(lambda, Compile(vm, vals.Unflatten()));
            }

            ThrowErr("let", "invalid argument 'bindings'", x.String());
            return null;
        }
    }
}
