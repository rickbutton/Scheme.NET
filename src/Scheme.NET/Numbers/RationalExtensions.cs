using Rationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Numbers
{
    public static class RationalExtensions
    {
        public static Rational Abs(this Rational p)
        {
            if (p.IsZero)
                return Rational.Zero;

            BigInteger numerator = BigInteger.Abs(p.Numerator);
            BigInteger denominator = BigInteger.Abs(p.Denominator);
            var result = new Rational(numerator, denominator);
            return result;
        }

        public static bool IsInteger(this Rational r)
        {
            return r.Denominator == 1;
        }

        public static bool IsEven(this Rational r)
        {
            if (r == 0) return false;
            if (!r.IsInteger()) return false;

            return r.Numerator % 2 == 0;
        }

        public static bool IsOdd(this Rational r)
        {
            if (r == 0) return false;
            if (!r.IsInteger()) return false;

            return r.Numerator % 2 != 0;
        }

        public static Rational GetIntegralPart(this Rational r)
        {
            if (r < 0) return r.WholePart + 1;
            return r.WholePart;
        }

        public static Rational GetFractionalPart(this Rational r)
        {
            if (r < 0) return (1 - r.FractionPart).Abs();
            return r.FractionPart;
        }

        public static Rational Round(this Rational r)
        {
            if (r.GetFractionalPart() < (Rational)0.5)
                return r.GetIntegralPart();
            else if (r.GetFractionalPart() > (Rational)0.5)
                return r > 0 ? r.GetIntegralPart() + 1 : r.GetIntegralPart() - 1;
            else
            {
                var a = r.GetIntegralPart().Numerator;
                var b = r.GetIntegralPart() + 1;
                if (a % 2 == 0) return a;
                return b;
            }
        }

        public static Rational Truncate(this Rational r)
        {
            if (r.IsInteger()) return r;
            return r.GetIntegralPart();
        }

        public static Rational Floor(this Rational r)
        {
            if (r.IsInteger() || r == 0) return r;

            if (r > 0) return r.GetIntegralPart();
            return r.GetIntegralPart() - 1;
        }

        public static Rational Ceiling(this Rational r)
        {
            if (r.IsInteger() || r == 0) return r;

            if (r > 0) return r.GetIntegralPart() + 1;
            return r.GetIntegralPart();
        }
    }
}
