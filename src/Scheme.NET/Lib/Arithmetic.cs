using Scheme.NET.Numbers;
using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Lib
{
    public static class Arithmetic
    {
        public static ISExpression Plus(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            return args.Cast<NumberAtom>().Sum();
        }

        public static ISExpression Mul(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            return args.Cast<NumberAtom>().Multiply();
        }

        public static ISExpression Minus(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureMinArgCount(args, 1);

            if (args.Count() == 1)
                return AtomHelper.NumberFromBigDecimal(BigRational.Invert((args.First() as NumberAtom).Val));

            var v = (args.First() as NumberAtom).Val;

            foreach (var a in args.Cast<NumberAtom>().Skip(1))
                v = v - a.Val;

            return AtomHelper.NumberFromBigDecimal(v);
        }

        public static ISExpression Div(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureMinArgCount(args, 1);

            if (args.Count() == 1)
                return AtomHelper.NumberFromBigDecimal(BigRational.Divide(BigRational.One, (args.First() as NumberAtom).Val));

            var v = (args.First() as NumberAtom).Val;

            foreach (var a in args.Cast<NumberAtom>().Skip(1))
                v = v / a.Val;

            return AtomHelper.NumberFromBigDecimal(v);
        }

        public static ISExpression Zero(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.BooleanFromBool(args.Cast<NumberAtom>().First().Val == BigRational.Zero);
        }

        public static ISExpression Positive(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.BooleanFromBool(args.Cast<NumberAtom>().First().Val > BigRational.Zero);
        }

        public static ISExpression Negative(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.BooleanFromBool(args.Cast<NumberAtom>().First().Val < BigRational.Zero);
        }

        public static ISExpression Odd(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.BooleanFromBool(BigRational.IsOdd(args.Cast<NumberAtom>().First().Val));
        }

        public static ISExpression Even(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.BooleanFromBool(BigRational.IsEven(args.Cast<NumberAtom>().First().Val));
        }

        public static ISExpression Min(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureMinArgCount(args, 2);
            return AtomHelper.NumberFromBigDecimal(args.Cast<NumberAtom>().Min(n => n.Val));
        }

        public static ISExpression Max(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureMinArgCount(args, 2);
            return AtomHelper.NumberFromBigDecimal(args.Cast<NumberAtom>().Max(n => n.Val));
        }

        public static ISExpression Abs(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.NumberFromBigDecimal(BigRational.Abs(args.Cast<NumberAtom>().First().Val));
        }

        public static ISExpression Quotient(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureArgCount(args, 2);

            var array = args.Cast<NumberAtom>().ToArray();
            var a = array[0];
            var b = array[1];

            var c = a.Val / b.Val;
            return AtomHelper.NumberFromBigDecimal(BigRational.Floor(c));
        }

        public static ISExpression Remainder(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureArgCount(args, 2);

            var array = args.Cast<NumberAtom>().ToArray();
            var a = array[0];
            var b = array[1];

            var c = BigRational.Remainder(a.Val, b.Val);
            return AtomHelper.NumberFromBigDecimal(BigRational.Floor(c));
        }

        public static ISExpression Modulo(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureArgCount(args, 2);

            var array = args.Cast<NumberAtom>().ToArray();
            var a = array[0];
            var b = array[1];

            var c = BigRational.Modulo(a.Val, b.Val);
            return AtomHelper.NumberFromBigDecimal(BigRational.Floor(c));
        }

        public static ISExpression Gcd(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllInteger(args);
            if (args.Count() == 0)
                return AtomHelper.NumberFromBigDecimal(0);

            var ints = args.Cast<NumberAtom>().Select(a => a.Val.Numerator);
            return AtomHelper.NumberFromBigDecimal(GCD(ints));
        }

        public static ISExpression Lcm(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllInteger(args);
            if (args.Count() == 0)
                return AtomHelper.NumberFromBigDecimal(1);

            var ints = args.Cast<NumberAtom>().Select(a => a.Val.Numerator);
            return AtomHelper.NumberFromBigDecimal(BigInteger.Abs(LCM(ints)));
        }

        public static ISExpression Floor(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.NumberFromBigDecimal(BigRational.Floor(args.Cast<NumberAtom>().First().Val));
        }

        public static ISExpression Ceiling(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.NumberFromBigDecimal(BigRational.Ceil(args.Cast<NumberAtom>().First().Val));
        }

        public static ISExpression Truncate(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.NumberFromBigDecimal(BigRational.Truncate(args.Cast<NumberAtom>().First().Val));
        }

        public static ISExpression Round(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.NumberFromBigDecimal(BigRational.Round(args.Cast<NumberAtom>().First().Val));
        }

        private static BigInteger GCD(IEnumerable<BigInteger> numbers)
        {
            return numbers.Aggregate((a, b) => BigInteger.GreatestCommonDivisor(a, b));
        }

        private static BigInteger LCM(IEnumerable<BigInteger> numbers)
        {
            return numbers.Aggregate((a, b) => (a * b) / BigInteger.GreatestCommonDivisor(a, b));
        }

        private static NumberAtom Sum(this IEnumerable<NumberAtom> ee)
        {
            BigRational sum = BigRational.Zero;
            foreach (var e in ee)
                sum = sum + e.Val;

            return AtomHelper.NumberFromBigDecimal(sum);
        }

        private static NumberAtom Multiply(this IEnumerable<NumberAtom> ee)
        {
            BigRational m = BigRational.One;
            foreach (var e in ee)
                m = m * e.Val;

            return AtomHelper.NumberFromBigDecimal(m);
        }
    }
}
