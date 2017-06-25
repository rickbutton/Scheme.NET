using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Eval
{
    public class Evaluator
    {
        public Scope GlobalScope { get; private set; }

        public Evaluator()
        {
            GlobalScope = new Scope();
        }

        public Evaluator(IDictionary<SymbolAtom, ISExpression> data)
        {
            GlobalScope = new Scope(data);
        }

        public ISExpression Eval(ISExpression e)
        {
            if (e.IsCons() && e.IsList())
            {
                var cons = e as Cons;
                var car = Eval(cons.Car);

                if (!car.IsProcedure())
                    ThrowError("Attempted application of non-function");

                var cdr = cons.Cdr;
                var args = cdr.Flatten().ToArray();

                var proc = (car as Procedure);

                if (!proc.Primitive)
                {
                    for (var i = 0; i < args.Length; i++)
                    {
                        args[i] = Eval(args[i]);
                    }
                }
                return proc.Proc(GlobalScope, args);
            }
            else if (e.IsSymbol())
                return GlobalScope.Lookup(e as SymbolAtom);

            return e;
        }

        private void ThrowError(string msg)
        {
            throw new InvalidOperationException($"Eval error: {msg}");
        }
    }
}
