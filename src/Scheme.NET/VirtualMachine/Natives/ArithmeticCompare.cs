using Scheme.NET.Numbers;
using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine.Natives.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.VirtualMachine.Natives
{
    public static class ArithmeticCompare
    {
        [AllNumbers]
        [MinCount(2)]
        public static ISExpression Equal(IEnumerable<ISExpression> args)
        {
            var first = args.First() as NumberAtom;

            return AtomHelper.BooleanFromBool(args.Cast<NumberAtom>().All(a => a.Val == first.Val));
        }

        [AllReals]
        [MinCount(2)]
        public static ISExpression IsIncreasing(IEnumerable<ISExpression> args)
        {
            return AtomHelper.BooleanFromBool(args.IsIncreasingMontonically());
        }

        [AllReals]
        [MinCount(2)]
        public static ISExpression IsDecreasing(IEnumerable<ISExpression> args)
        {
            return AtomHelper.BooleanFromBool(args.IsDecreasingMontonically());
        }

        [AllReals]
        [MinCount(2)]
        public static ISExpression IsNonIncreasing(IEnumerable<ISExpression> args)
        {
            return AtomHelper.BooleanFromBool(args.IsDecreasingOrEqualMontonically());
        }

        [AllReals]
        [MinCount(2)]
        public static ISExpression IsNonDecreasing(IEnumerable<ISExpression> args)
        {
            return AtomHelper.BooleanFromBool(args.IsIncreasingOrEqualMontonically());
        }

        private static bool IsIncreasingMontonically(this IEnumerable<ISExpression> list)
        {
            var baseList = list.Cast<NumberAtom>().Select(a => a.Val);
            return baseList.Zip(baseList
                     .Skip(1), (a, b) => a.CompareTo(b) < 0).All(b => b);
        }

        private static bool IsIncreasingOrEqualMontonically(this IEnumerable<ISExpression> list)
        {
            var baseList = list.Cast<NumberAtom>().Select(a => a.Val);
            return baseList.Zip(baseList
                     .Skip(1), (a, b) => a.CompareTo(b) <= 0).All(b => b);
        }

        private static bool IsDecreasingMontonically(this IEnumerable<ISExpression> list)
        {
            var baseList = list.Cast<NumberAtom>().Select(a => a.Val);
            return baseList.Zip(baseList
                     .Skip(1), (a, b) => a.CompareTo(b) > 0).All(b => b);
        }

        private static bool IsDecreasingOrEqualMontonically(this IEnumerable<ISExpression> list)
        {
            var baseList = list.Cast<NumberAtom>().Select(a => a.Val);
            return baseList.Zip(baseList
                     .Skip(1), (a, b) => a.CompareTo(b) >= 0).All(b => b);
        }
    }
}
