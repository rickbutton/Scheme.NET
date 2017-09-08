using Rationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Numbers
{
    internal static class RationalExtensions
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

        public static bool IsInteger(this Rational r) { return r.FractionPart == 0; }
        public static bool IsPositive(this Rational r) { return r > 0; }
        public static bool IsNegative(this Rational r) { return r < 0; }

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
                var b = r.GetIntegralPart() + (r.GetIntegralPart() >= 0 ? 1 : -1);
                if (a % 2 == 0) return a;
                return b;
            }
        }

        public static Rational GCD(this Rational a, Rational b)
        {
            if (!a.IsInteger() || !b.IsInteger())
                throw new InvalidOperationException();

            Func<Rational, Rational, Rational> func = null;
            func = (x, y) => y == 0 ? x : func(y, x.Numerator % y.Numerator);
            return func(a, b);
        }

        public static Rational Modulo(this Rational a, Rational b)
        {
            if (a.Denominator == 0)
                return 0;
            if (a.FractionPart != Rational.Zero || b.FractionPart != Rational.Zero)
                throw new InvalidOperationException("Can't perform modulo of non-integers.");

            return a.Numerator % b.Numerator;
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

        public static string ToString(this Rational r, int radix, bool forceSign = false)
        {
            var sign = r.Sign > 0 ? (forceSign ? "+" : "") : "-";

            if (r.IsZero) return "0";
            if (r.FractionPart == Rational.Zero)
            {
                return $"{sign}{BigInteger.Abs(r.CanonicalForm.Numerator).ToString(radix)}";
            }
            return $"{sign}{BigInteger.Abs(r.Numerator).ToString(radix)}/{BigInteger.Abs(r.Denominator).ToString(radix)}";
        }

        public static string ToDecimalString(this Rational r, int radix, bool forceSign = false)
        {
            var d = (decimal)r;
            var sign = d >= 0 ? (forceSign ? "+" : "") : "-";

            var intPart = (long)d;
            var intStr = BigInteger.Abs(intPart).ToString(radix);
            var fraPart = d - intPart;
            var fraStr = "";

            while (fraPart != 0)
            {
                fraPart = fraPart * radix;
                var integral = (long)fraPart;
                fraPart = fraPart - integral;
                fraStr = fraStr + BigIntegerHelpers.ConvertDigitToChar(Math.Abs(integral));
            }

            if (string.IsNullOrEmpty(fraStr))
            {
                return $"{sign}{intStr}";
            }
            return $"{sign}{intStr}.{fraStr}";
        }
    }
}
