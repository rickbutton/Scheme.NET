using Scheme.NET.Lib;
using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Eval
{
    public static class Evaluator
    {
        public static ISExpression Eval(Scope scope, ISExpression e)
        {
            return Eval(scope, new ISExpression[] { e });
        }

        public static ISExpression Eval(Scope scope, IEnumerable<ISExpression> es)
        {
            LibHelper.EnsureArgCount(es, 1);

            var e = es.First();

            if (e.IsCons() && e.IsList())
            {
                var cons = e as Cons;
                var car = Eval(scope, cons.Car);

                if (!car.IsProcedure())
                    ThrowError("Attempted application of non-function");

                var cdr = cons.Cdr;
                var args = cdr.Flatten().ToArray();

                var proc = (car as Procedure);

                if (!proc.Primitive)
                {
                    for (var i = 0; i < args.Length; i++)
                    {
                        args[i] = Eval(scope, args[i]);
                    }
                }
                return proc.Proc(scope, args);
            }
            else if (e.IsSymbol())
                return scope.Lookup(e as SymbolAtom);

            return e;
        }

        private static void ThrowError(string msg)
        {
            throw new InvalidOperationException($"Eval error: {msg}");
        }
    }
}
