using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheme.NET.Numbers;
using Microsoft.SolverFoundation.Common;

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
                return AtomHelper.NumberFromComplex(-((NumberAtom) args.First()).Val);

            var v = (args.First() as NumberAtom).Val;

            foreach (var a in args.Cast<NumberAtom>().Skip(1))
                v = v - a.Val;

            return AtomHelper.NumberFromComplex(v);
        }

        public static ISExpression Div(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureMinArgCount(args, 1);

            if (args.Count() == 1)
                return AtomHelper.NumberFromComplex(1.ToComplex() / (args.First() as NumberAtom).Val);

            var v = (args.First() as NumberAtom).Val;

            foreach (var a in args.Cast<NumberAtom>().Skip(1))
                v = v / a.Val;

            return AtomHelper.NumberFromComplex(v);
        }

        public static ISExpression Zero(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllNumber(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.BooleanFromBool(args.Cast<NumberAtom>().First().Val.IsZero);
        }

        public static ISExpression Positive(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllReal(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.BooleanFromBool(args.Cast<NumberAtom>().First().Val.RealIsPositive);
        }

        public static ISExpression Negative(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllReal(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.BooleanFromBool(args.Cast<NumberAtom>().First().Val.RealIsNegative);
        }

        public static ISExpression Odd(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllInteger(args);
            LibHelper.EnsureArgCount(args, 1);

            var n = args.Cast<NumberAtom>().First().Val.AsExact().Real;

            return AtomHelper.BooleanFromBool(n.Modulo(2) != 0);
        }

        public static ISExpression Even(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllInteger(args);
            LibHelper.EnsureArgCount(args, 1);

            var n = args.Cast<NumberAtom>().First().Val.AsExact().Real;

            return AtomHelper.BooleanFromBool(n.Modulo(2) == 0);
        }

        public static ISExpression Min(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllReal(args);
            LibHelper.EnsureMinArgCount(args, 2);
            var nums = args.Cast<NumberAtom>().Select(n => n.Val);
            return AtomHelper.NumberFromComplex(
                NumberExtensions.ToComplex(
                nums.Min(a => 
                a.PromoteRelative(nums).Real)));
        }

        public static ISExpression Max(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllReal(args);
            LibHelper.EnsureMinArgCount(args, 2);
            var nums = args.Cast<NumberAtom>().Select(n => n.Val);
            return AtomHelper.NumberFromComplex(
                NumberExtensions.ToComplex(
                nums.Max(a => 
                a.PromoteRelative(nums).Real)));
        }

        public static ISExpression Abs(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllReal(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.NumberFromComplex(args.Cast<NumberAtom>().First().Val.Abs());
        }

        public static ISExpression Quotient(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllReal(args);
            LibHelper.EnsureArgCount(args, 2);

            var array = args.Cast<NumberAtom>().ToArray();
            var a = array[0];
            var b = array[1];

            var c = a.Val / b.Val;

            return AtomHelper.NumberFromComplex(c.RealFloor());
        }

        public static ISExpression Remainder(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllReal(args);
            LibHelper.EnsureArgCount(args, 2);

            var array = args.Cast<NumberAtom>().ToArray();
            var a = array[0];
            var b = array[1];

            var c = a.Val.RealModulo(b.Val);
            return AtomHelper.NumberFromComplex(c);
        }

        public static ISExpression Modulo(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllReal(args);
            LibHelper.EnsureArgCount(args, 2);

            var array = args.Cast<NumberAtom>().ToArray();
            var a = array[0].Val;
            var b = array[1].Val;

            var c = (a.RealModulo(b) + b).RealModulo(b);
            return AtomHelper.NumberFromComplex(c);
        }

        public static ISExpression Gcd(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllInteger(args);
            if (!args.Any())
                return AtomHelper.NumberFromComplex(0);

            var nums = args.Cast<NumberAtom>().Select(a => a.Val.AsExact().Real.Numerator).ToArray();

            return AtomHelper.NumberFromComplex(GCD(nums).AbsoluteValue);
        }

        public static ISExpression Lcm(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllInteger(args);
            if (args.Count() == 0)
                return AtomHelper.NumberFromComplex(1);

            var nums = args.Cast<NumberAtom>().Select(a => a.Val.AsExact().Real.Numerator).ToArray();
            return AtomHelper.NumberFromComplex(LCM(nums).AbsoluteValue);
        }

        public static ISExpression Floor(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllReal(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.NumberFromComplex(args.Cast<NumberAtom>().First().Val.RealFloor());
        }

        public static ISExpression Ceiling(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllReal(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.NumberFromComplex(args.Cast<NumberAtom>().First().Val.RealCeiling());
        }

        public static ISExpression Truncate(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllReal(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.NumberFromComplex(args.Cast<NumberAtom>().First().Val.RealTruncate());
        }

        public static ISExpression Round(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureAllReal(args);
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.NumberFromComplex(args.Cast<NumberAtom>().First().Val.RealRound());
        }

        private static BigInteger GCD(BigInteger[] numbers)
        {
            return numbers.Aggregate(GCD);
        }

        private static BigInteger GCD(BigInteger a, BigInteger b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        private static BigInteger LCM(BigInteger[] numbers)
        {
            return numbers.Aggregate((a, b) => (a * b) / GCD(a, b));
        }

        private static NumberAtom Sum(this IEnumerable<NumberAtom> ee)
        {
            Complex sum = new Complex<Rational>(0, 0);
            foreach (var e in ee)
                sum = sum + e.Val;

            return AtomHelper.NumberFromComplex(sum);
        }

        private static NumberAtom Multiply(this IEnumerable<NumberAtom> ee)
        {
            Complex m = new Complex<Rational>(1, 0);
            foreach (var e in ee)
                m = m * e.Val;

            return AtomHelper.NumberFromComplex(m);
        }
    }
}
