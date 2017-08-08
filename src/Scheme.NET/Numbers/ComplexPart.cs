using Rationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Numbers
{
    public class ComplexPart : IComparable<ComplexPart>, IEquatable<ComplexPart>
    {
        public static readonly ComplexPart Zero = new ComplexPart(0);

        private Rational _value;

        internal ComplexPart(BigInteger n, BigInteger d) { _value = new Rational(n, d); }
        internal ComplexPart(BigInteger n) { _value = new Rational(n, 1); }

        public BigInteger Numerator => _value.Numerator;
        public BigInteger Denominator => _value.Denominator;
        public bool IsZero => _value.IsZero;
        public bool IsPositive => _value.IsPositive();
        public bool IsNegative => _value.IsNegative();
        public bool IsOdd => _value.IsOdd();
        public bool IsEven => _value.IsEven();
        public bool IsInteger() { return _value.IsInteger(); }

        public ComplexPart Abs() { return ComplexPart.Create(_value.Abs()); }
        public ComplexPart Floor() { return ComplexPart.Create(_value.Floor()); }
        public ComplexPart Ceiling() { return ComplexPart.Create(_value.Ceiling()); }
        public ComplexPart Truncate() { return ComplexPart.Create(_value.Truncate()); }
        public ComplexPart Round() { return ComplexPart.Create(_value.Round()); }
        public ComplexPart GCD(ComplexPart b) { return ComplexPart.Create(_value.GCD(b._value)); }
        public ComplexPart Modulo(ComplexPart b) { return ComplexPart.Create(_value.Modulo(b._value)); }

        public string ToString(int radix, bool forceSign = false) { return _value.ToString(radix, forceSign); }
        public string ToDecimalString(int radix, bool forceSign = false) { return _value.ToDecimalString(radix, forceSign); }

        public int CompareTo(ComplexPart other) { return _value.CompareTo(other._value); }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(ComplexPart a, ComplexPart b)
        {
            if (((object)a) == null && ((object)b) == null) return true;
            else if (((object)a) == null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(ComplexPart a, ComplexPart b)
        {
            if (((object)a) == null && ((object)b) == null) return false;
            else if (((object)a) == null) return false;
            return !a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is ComplexPart) return this.Equals((ComplexPart)obj);
            return false;
        }

        public bool Equals(ComplexPart other)
        {
            if (((object)other) == null) return false;
            return this._value.Equals(other._value);
        }

        public static ComplexPart operator +(ComplexPart x, ComplexPart y)
            { return ComplexPart.Create(x._value + y._value); }
        public static ComplexPart operator +(ComplexPart x, BigInteger y)
            { return ComplexPart.Create(x._value + y); }
        public static ComplexPart operator +(BigInteger x, ComplexPart y)
            { return ComplexPart.Create(x + y._value); }

        public static ComplexPart operator -(ComplexPart x, ComplexPart y)
            { return ComplexPart.Create(x._value - y._value); }
        public static ComplexPart operator -(ComplexPart x, BigInteger y)
            { return ComplexPart.Create(x._value - y); }
        public static ComplexPart operator -(BigInteger x, ComplexPart y)
            { return ComplexPart.Create(x- y._value); }

        public static ComplexPart operator -(ComplexPart x)
            { return ComplexPart.Create(-x._value); }

        public static ComplexPart operator *(ComplexPart x, ComplexPart y)
            { return ComplexPart.Create(x._value * y._value); }
        public static ComplexPart operator *(ComplexPart x, BigInteger y)
            { return ComplexPart.Create(x._value * y); }
        public static ComplexPart operator *(BigInteger x, ComplexPart y)
            { return ComplexPart.Create(x * y._value); }

        public static ComplexPart operator /(ComplexPart x, ComplexPart y)
            { return ComplexPart.Create(x._value / y._value); }
        public static ComplexPart operator /(ComplexPart x, BigInteger y)
            { return ComplexPart.Create(x._value / y); }
        public static ComplexPart operator /(BigInteger x, ComplexPart y)
            { return ComplexPart.Create(x/ y._value); }

        public static explicit operator int(ComplexPart a)
        {
            return (int)a._value;
        }

        public static ComplexPart Pow(ComplexPart x, int y)
        {
            return Create(Rational.Pow(x._value, y));
        }

        public static ComplexPart CreateFromDouble(double d) {
            var r = (Rational)d;
            return new ComplexPart(r.Numerator, r.Denominator);
        }

        public static ComplexPart CreateFromBigInteger(BigInteger b)
        {
            var r = (Rational)b;
            return new ComplexPart(r.Numerator, r.Denominator);
        }

        private static ComplexPart Create(Rational r)
        {
            return new ComplexPart(r.Numerator, r.Denominator);
        }

        
    }
}
