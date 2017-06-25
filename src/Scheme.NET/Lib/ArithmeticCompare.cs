﻿using Scheme.NET.Numbers;
using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Lib
{
    public static class ArithmeticCompare
    {
        public static ISExpression Equal(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureMinArgCount(args, 2);

            var first = args.First() as NumberAtom;

            return AtomHelper.BooleanFromBool(args.Cast<NumberAtom>().All(a => a.Val == first.Val));
        }

        public static ISExpression IsIncreasing(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureMinArgCount(args, 2);

            return AtomHelper.BooleanFromBool(args.IsIncreasingMontonically());
        }

        public static ISExpression IsDecreasing(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureMinArgCount(args, 2);

            return AtomHelper.BooleanFromBool(args.IsDecreasingMontonically());
        }

        public static ISExpression IsNonIncreasing(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureMinArgCount(args, 2);

            return AtomHelper.BooleanFromBool(args.IsDecreasingOrEqualMontonically());
        }

        public static ISExpression IsNonDecreasing(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureMinArgCount(args, 2);

            return AtomHelper.BooleanFromBool(args.IsIncreasingOrEqualMontonically());
        }

        private static bool IsIncreasingMontonically(this IEnumerable<ISExpression> list)
        {
            return list.Cast<NumberAtom>()
                .Zip(list.Cast<NumberAtom>()
                     .Skip(1), (a, b) => a.Val.CompareTo(b.Val) < 0).All(b => b);
        }

        private static bool IsIncreasingOrEqualMontonically(this IEnumerable<ISExpression> list)
        {
            return list.Cast<NumberAtom>()
                .Zip(list.Cast<NumberAtom>()
                     .Skip(1), (a, b) => a.Val.CompareTo(b.Val) <= 0).All(b => b);
        }

        private static bool IsDecreasingMontonically(this IEnumerable<ISExpression> list)
        {
            return list.Cast<NumberAtom>()
                .Zip(list.Cast<NumberAtom>()
                     .Skip(1), (a, b) => a.Val.CompareTo(b.Val) > 0).All(b => b);
        }

        private static bool IsDecreasingOrEqualMontonically(this IEnumerable<ISExpression> list)
        {
            return list.Cast<NumberAtom>()
                .Zip(list.Cast<NumberAtom>()
                     .Skip(1), (a, b) => a.Val.CompareTo(b.Val) >= 0).All(b => b);
        }
    }
}
