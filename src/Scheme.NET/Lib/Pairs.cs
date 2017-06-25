using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Lib
{
    public static class Pairs
    {
        public static ISExpression Cons(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 2);
            var a = args.ToArray();
            return AtomHelper.CreateCons(a[0], a[1]);
        }

        public static ISExpression Car(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            LibHelper.EnsureAllCons(args);
            var c = args.First() as Cons;
            return c.Car;
        }

        public static ISExpression Cdr(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            LibHelper.EnsureAllCons(args);
            var c = args.First() as Cons;
            return c.Cdr;
        }

        public static ISExpression SetCar(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 2);
            LibHelper.EnsureCons(args, 0);
            var c = args.First() as Cons;
            c.Car = args.ToArray()[1];
            return AtomHelper.Nil;
        }

        public static ISExpression SetCdr(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 2);
            LibHelper.EnsureCons(args, 0);
            var c = args.First() as Cons;
            c.Cdr = args.ToArray()[1];
            return AtomHelper.Nil;
        }
    }
}
