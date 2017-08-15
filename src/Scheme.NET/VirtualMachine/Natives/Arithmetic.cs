using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheme.NET.Numbers;
using Scheme.NET.VirtualMachine.Natives.Attributes;

namespace Scheme.NET.VirtualMachine.Natives
{
    public static class Arithmetic
    {
        [AllNumbers]
        public static ISExpression Plus(IEnumerable<ISExpression> args)
        {
            return args.Cast<NumberAtom>().Sum();
        }

        [AllNumbers]
        public static ISExpression Mul(IEnumerable<ISExpression> args)
        {
            return args.Cast<NumberAtom>().Multiply();
        }

        [AllNumbers]
        [MinCount(1)]
        public static ISExpression Minus(IEnumerable<ISExpression> args)
        {

            if (args.Count() == 1)
                return AtomHelper.NumberFromComplex(-((NumberAtom) args.First()).Val);

            var v = (args.First() as NumberAtom).Val;

            foreach (var a in args.Cast<NumberAtom>().Skip(1))
                v = v - a.Val;

            return AtomHelper.NumberFromComplex(v);
        }

        [AllNumbers]
        [MinCount(1)]
        public static ISExpression Div(IEnumerable<ISExpression> args)
        {
            if (args.Count() == 1)
                return AtomHelper.NumberFromComplex(Complex.CreateExactReal(1) / (args.First() as NumberAtom).Val);

            var v = (args.First() as NumberAtom).Val;

            foreach (var a in args.Cast<NumberAtom>().Skip(1))
                v = v / a.Val;

            return AtomHelper.NumberFromComplex(v);
        }

        [AllNumbers]
        [Count(1)]
        public static ISExpression Zero(IEnumerable<ISExpression> args)
        {
            return AtomHelper.BooleanFromBool(args.Cast<NumberAtom>().First().Val.IsZero);
        }

        [AllReals]
        [Count(1)]
        public static ISExpression Positive(IEnumerable<ISExpression> args)
        {
            return AtomHelper.BooleanFromBool(args.Cast<NumberAtom>().First().Val.Real.IsPositive);
        }

        [AllReals]
        [Count(1)]
        public static ISExpression Negative(IEnumerable<ISExpression> args)
        {
            return AtomHelper.BooleanFromBool(args.Cast<NumberAtom>().First().Val.Real.IsNegative);
        }

        [AllIntegers]
        [Count(1)]
        public static ISExpression Odd(IEnumerable<ISExpression> args)
        {
            var n = args.Cast<NumberAtom>().First().Val.Real;

            return AtomHelper.BooleanFromBool(n.IsOdd);
        }

        [AllIntegers]
        [Count(1)]
        public static ISExpression Even(IEnumerable<ISExpression> args)
        {
            var n = args.Cast<NumberAtom>().First().Val.Real;

            return AtomHelper.BooleanFromBool(n.IsEven || n.IsZero);
        }

        [AllReals]
        [MinCount(2)]
        public static ISExpression Min(IEnumerable<ISExpression> args)
        {
            var nums = args.Cast<NumberAtom>().Select(n => n.Val);
            return AtomHelper.NumberFromComplex(nums.Min());
        }

        [AllReals]
        [MinCount(2)]
        public static ISExpression Max(IEnumerable<ISExpression> args)
        {
            var nums = args.Cast<NumberAtom>().Select(n => n.Val);
            return AtomHelper.NumberFromComplex(nums.Max());
        }

        [AllReals]
        [Count(1)]
        public static ISExpression Abs(IEnumerable<ISExpression> args)
        {
            return AtomHelper.NumberFromComplex(args.Cast<NumberAtom>().First().Val.RealAbs());
        }

        [AllReals]
        [Count(2)]
        public static ISExpression Quotient(IEnumerable<ISExpression> args)
        {
            var array = args.Cast<NumberAtom>().ToArray();
            var a = array[0];
            var b = array[1];

            var c = a.Val / b.Val;

            if (c.Real.IsInteger())
            {
                return AtomHelper.NumberFromComplex(c);
            }
            else
            {
                return AtomHelper.NumberFromComplex(c.RealTruncate());
            }
        }

        [AllReals]
        [Count(2)]
        public static ISExpression Remainder(IEnumerable<ISExpression> args)
        {
            var array = args.Cast<NumberAtom>().ToArray();
            var a = array[0];
            var b = array[1];

            var c = a.Val.RealModulo(b.Val);
            return AtomHelper.NumberFromComplex(c);
        }

        [AllReals]
        [Count(2)]
        public static ISExpression Modulo(IEnumerable<ISExpression> args)
        {
            var array = args.Cast<NumberAtom>().ToArray();
            var a = array[0].Val;
            var b = array[1].Val;

            var c = (a.RealModulo(b) + b).RealModulo(b);
            return AtomHelper.NumberFromComplex(c);
        }

        [AllIntegers]
        public static ISExpression Gcd(IEnumerable<ISExpression> args)
        {
            if (!args.Any())
                return AtomHelper.NumberFromComplex(0);

            var nums = args.Cast<NumberAtom>().Select(a => a.Val).ToArray();

            return AtomHelper.NumberFromComplex(GCD(nums).RealAbs());
        }

        [AllIntegers]
        public static ISExpression Lcm(IEnumerable<ISExpression> args)
        {
            if (args.Count() == 0)
                return AtomHelper.NumberFromComplex(1);

            var nums = args.Cast<NumberAtom>().Select(a => a.Val).ToArray();
            return AtomHelper.NumberFromComplex(LCM(nums).RealAbs());
        }

        [AllReals]
        [Count(1)]
        public static ISExpression Floor(IEnumerable<ISExpression> args)
        {
            return AtomHelper.NumberFromComplex(args.Cast<NumberAtom>().First().Val.RealFloor());
        }

        [AllReals]
        [Count(1)]
        public static ISExpression Ceiling(IEnumerable<ISExpression> args)
        {
            return AtomHelper.NumberFromComplex(args.Cast<NumberAtom>().First().Val.RealCeiling());
        }

        [AllReals]
        [Count(1)]
        public static ISExpression Truncate(IEnumerable<ISExpression> args)
        {
            return AtomHelper.NumberFromComplex(args.Cast<NumberAtom>().First().Val.RealTruncate());
        }

        [AllReals]
        [Count(1)]
        public static ISExpression Round(IEnumerable<ISExpression> args)
        {
            return AtomHelper.NumberFromComplex(args.Cast<NumberAtom>().First().Val.RealRound());
        }

        private static Complex GCD(Complex[] numbers)
        {
            return numbers.Aggregate((a, b) => a.RealGCD(b));
        }

        private static Complex LCM(Complex[] numbers)
        {
            return numbers.Aggregate((a, b) => (a * b) / a.RealGCD(b));
        }

        private static NumberAtom Sum(this IEnumerable<NumberAtom> ee)
        {
            Complex sum = Complex.CreateExactReal(0);
            foreach (var e in ee)
                sum = sum + e.Val;

            return AtomHelper.NumberFromComplex(sum);
        }

        private static NumberAtom Multiply(this IEnumerable<NumberAtom> ee)
        {
            Complex m = Complex.CreateExactReal(1);
            foreach (var e in ee)
                m = m * e.Val;

            return AtomHelper.NumberFromComplex(m);
        }
    }
}
