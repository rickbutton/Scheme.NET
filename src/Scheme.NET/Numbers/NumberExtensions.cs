using Microsoft.SolverFoundation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Numbers
{
    public static class NumberExtensions
    {
        public static Complex<double> ExactToInexact(this Complex<Rational> c)
        {
            return new Complex<double>(c.Real.GetSignedDouble(), c.Imaginary.GetSignedDouble());
        }

        public static bool IsExact(this Complex c)
        {
            return c.PartType == typeof(Rational);
        }

        public static Complex<Rational> AsExact(this Complex c)
        {
            if (!c.IsExact())
                throw new InvalidOperationException("Attempted to cast number to exact that was not exact");
            return c.As<Rational>();
        }

        public static Complex<double> AsInexact(this Complex c)
        {
            if (c.IsExact())
            {
                return c.AsExact().ExactToInexact();
            } else
            {
                return c.As<double>();
            }
        }

        public static bool IsInteger(this Complex c)
        {
            if (!c.ImaginaryIsZero) return false;

            if (c.IsExact())
                return c.AsExact().Real.Denominator == 1;
            else
                return c.AsInexact().Real % 1 == 0;
        }

        public static Complex PromoteRelative(this Complex a, params Complex[] b)
        {
            return PromoteRelative(a, b.AsEnumerable());
        }

        public static Complex PromoteRelative(this Complex a, IEnumerable<Complex> b)
        {
            if (b.All(c => c.IsExact()))
                return a;

            return a.AsInexact();
        }

        public static Complex ToComplex(this int i)
        {
            return NumberTower.ExactInteger(i);
        }

        public static Complex ToComplex(object o)
        {
            if (o is Rational)
                return new Complex<Rational>((Rational)o, 0);
            else if (o is double)
                return new Complex<double>((double)o, 0);
            throw new InvalidOperationException("invalid complex part type");
        }

        

        public static BigInteger Modulo(this BigInteger a, BigInteger b)
        {
            return ((a % b) + b) % b;
        }

        public static Rational Modulo(this Rational a, Rational b)
        {
            if (a.Denominator == 0)
                return 0;
            if (a.Denominator != 1 || b.Denominator != 1)
                throw new InvalidOperationException("Can't perform modulo of non-integers.");

            return a.Numerator % b.Numerator;
        }

        public static Rational Abs(this Rational r)
        {
            return r.AbsoluteValue;
        }

        public static Rational Round(this Rational r)
        {
            if (r.GetFractionalPart() < 0.5)
                return r.GetIntegerPart();
            else if (r.GetFractionalPart() > 0.5)
                return r > 0 ? r.GetIntegerPart() + 1 : r.GetIntegerPart() - 1;
            else
            {
                var a = r.GetIntegerPart();
                var b = r.GetIntegerPart() + 1;
                if (a.Modulo(2) == 0) return a;
                return b;
            }
        }

        public static string ToString(this Complex c, int radix)
        {
            string real, imag;

            if (c.IsExact())
            {
                var ce = c.AsExact();
                real = ce.Real.ToString(radix);
                imag = ce.Imaginary.ToString(radix, true);
            }
            else
            {
                var ci = c.AsInexact();
                real = ci.Real.ToString(radix);
                imag = ci.Imaginary.ToString(radix, true);
            }

            if (!c.ImaginaryIsZero)
            {
                return $"{real}{imag}i";
            }
            return real;
        }

        public static string ToString(this Rational r, int radix, bool forceSign = false)
        {
            var sign = r.Sign > 0 ? (forceSign ? "+" : "") : "-";
            if (r.Denominator == 1)
            {
                return $"{sign}{r.Numerator.AbsoluteValue.ToString(radix)}";
            }
            return $"{sign}{r.Numerator.AbsoluteValue.ToString(radix)}/{r.Denominator.AbsoluteValue.ToString(radix)}";
        }

        public static string ToString(this BigInteger i, int radix)
        {
            var str = "";
            while (i != 0)
            {
                str = ConvertDigitToChar(i % radix) + str;
                i = i / radix;
            }
            return str;
        }

        public static string ToString(this double d, int radix, bool forceSign = false)
        {
            var sign = d >= 0 ? (forceSign ? "+" : "") : "-";

            var intPart = (long)d;
            var intStr = ((BigInteger)intPart).AbsoluteValue.ToString(radix);
            var fraPart = d - intPart;
            var fraStr = "";

            while (fraPart != 0)
            {
                fraPart = fraPart * radix;
                var integral = (long)fraPart;
                fraPart = fraPart - integral;
                fraStr = fraStr + ConvertDigitToChar(Math.Abs(integral));
            }

            if (string.IsNullOrEmpty(fraStr))
            {
                return $"{sign}{intStr}";
            }
            return $"{sign}{intStr}.{fraStr}";
        }

        public static bool Convert(string s, int radix, out BigInteger b)
        {
            b = 0;
            for (var i = 0; i < s.Length; i++)
            {
                BigInteger res;
                var success = Convert(s[i], radix, out res);
                if (!success) return false;

                res = res * BigInteger.Power(radix, (s.Length - i - 1));
                b = b + res;
            }
            return true;
        }

        private static string ConvertDigitToChar(BigInteger l)
        {
            var c = (char)(long)l;
            if (l <= 9)
                return ((char)(c + '0')).ToString();
            if (l <= 16)
                return ((char)(c - 10 + 'a')).ToString();
            throw new InvalidOperationException("invalid digit");
        }

        private static bool Convert(char c, int radix, out BigInteger b)
        {
            if (c == '0' || c == '1')
                b = c - '0';
            else if (radix >= 8 && c >= '0' && c <= '7')
                b = c - '0';
            else if (radix >= 10 && c >= '0' && c <= '9')
                b = c - '0';
            else if (radix == 16 && c >= 'a' && c <= 'f')
                b = c - 'a' + 10;
            else if (radix == 16 && c >= 'A' && c <= 'F')
                b = c - 'A' + 10;
            else
            {
                b = 0;
                return false;
            }
            return true;
        }

        public static int RealCompare(this Complex a, Complex b)
        {
            a = a.PromoteRelative(b);
            b = b.PromoteRelative(a);

            if (a.IsExact())
            {
                Complex<Rational> ae = a.AsExact(), be = b.AsExact();
                return ae.Real.CompareTo(be.Real);
            }
            else
            {
                Complex<double> ai = a.AsInexact(), bi = b.AsInexact();
                return ai.Real.CompareTo(bi.Real);
            }
        }
        
        public static Complex RealFloor(this Complex c) { return DoRealTransform(c, r => r.GetFloor(), Math.Floor); }
        public static Complex RealCeiling(this Complex c) { return DoRealTransform(c, r => r.GetCeiling(), Math.Ceiling); }
        public static Complex RealTruncate(this Complex c) { return DoRealTransform(c, r => r.GetIntegerPart(), Math.Truncate); }
        public static Complex RealRound(this Complex c) { return DoRealTransform(c, r => Round(r), Math.Round); }

        public static Complex RealModulo(this Complex c, Complex d) {
            return DoRealBinary(c, d, (a, b) => a.Modulo(b), (a, b) => a % b);
        }

        private static Complex DoRealBinary(Complex a, Complex b, Func<Rational, Rational, Rational> eFunc, Func<double, double, double> iFunc)
        {
            a = a.PromoteRelative(b);
            b = b.PromoteRelative(a);

            if (a.IsExact())
            {
                Complex<Rational> ae = a.AsExact(), be = b.AsExact();
                return new Complex<Rational>(eFunc(ae.Real, be.Real), 0);
            }
            else
            {
                Complex<double> ae = a.AsInexact(), be = b.AsInexact();
                return new Complex<Rational>(iFunc(ae.Real, be.Real), 0);
            }
        }

        private static Complex DoRealTransform(Complex c, Func<Rational, Rational> eFunc, Func<double, double> iFunc)
        {
            if (c.IsExact())
            {
                var e = c.AsExact();
                return new Complex<Rational>(eFunc(e.Real), 0);
            }
            else
            {
                var i = c.AsInexact();
                return new Complex<double>(iFunc(i.Real), 0);
            }
        }
    }
}
