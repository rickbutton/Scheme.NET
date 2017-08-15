using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine.Natives.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.VirtualMachine.Natives
{
    public static class Pairs
    {

        [Count(2)]
        public static ISExpression Cons(IEnumerable<ISExpression> args)
        {
            var a = args.ToArray();
            return AtomHelper.CreateCons(a[0], a[1]);
        }

        [Count(1)]
        [AllCons]
        public static ISExpression Car(IEnumerable<ISExpression> args)
        {
            var c = args.First() as Cons;
            return c.Car;
        }

        [Count(1)]
        [AllCons]
        public static ISExpression Cdr(IEnumerable<ISExpression> args)
        {
            var c = args.First() as Cons;
            return c.Cdr;
        }

        [Count(2)]
        [Cons(0)]
        public static ISExpression SetCar(IEnumerable<ISExpression> args)
        {
            var c = args.First() as Cons;
            c.Car = args.ToArray()[1];
            return AtomHelper.Nil;
        }

        [Count(2)]
        [Cons(0)]
        public static ISExpression SetCdr(IEnumerable<ISExpression> args)
        {
            var c = args.First() as Cons;
            c.Cdr = args.ToArray()[1];
            return AtomHelper.Nil;
        }
    }
}
