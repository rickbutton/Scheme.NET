using Rationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Numbers
{
    public class RationalPart : PartBase
    {
        private Rational _value;

        public RationalPart(Rational value)
        {
            _value = value;
        }

        public override int Sign => _value.Sign;
        public override bool IsPositive => _value > 0;
        public override bool IsNegative => _value < 0;
        public override bool IsZero => _value == 0;
        public override bool IsInteger => _value.IsInteger();
        public override bool IsEven => _value.IsEven();
        public override bool IsOdd => _value.IsOdd();
        public override object BackingValue => _value;
        public Rational Rational => _value;

        public override IComplexPart Add(IComplexPart b) { return DoBinary(this, b, (x, y) => x + y); }
        public override IComplexPart Sub(IComplexPart b) { return DoBinary(this, b, (x, y) => x - y); }
        public override IComplexPart Mul(IComplexPart b) { return DoBinary(this, b, (x, y) => x * y); }
        public override IComplexPart Div(IComplexPart b) { return DoBinary(this, b, (x, y) => x / y); }
        public override IComplexPart Negate() { return DoUnary(this, (x) => -x); }
        public override IComplexPart Abs() { return DoUnary(this, (x) => x.Abs()); }
        public override IComplexPart Floor() { return DoUnary(this, (x) => x.Floor()); }
        public override IComplexPart Ceiling() { return DoUnary(this, (x) => x.Ceiling()); }
        public override IComplexPart Truncate() {
            return DoUnary(this, (x) => x.Truncate());
        }
        public override IComplexPart Round() { return DoUnary(this, (x) => x.Round()); }
        public override IComplexPart GCD(IComplexPart b)
        {
            if (!IsInteger || !b.IsInteger)
                throw new InvalidOperationException();

            Func<Rational, Rational, Rational> func = null;
            func = (x, y) => y == 0 ? x : func(y, x.Numerator % y.Numerator);
            return DoBinary(this, b, func);
        }
        public override IComplexPart Modulo(IComplexPart b)
        {
            if (!(b is RationalPart))
                throw new InvalidOperationException();

            var rb = b as RationalPart;

            if (Rational.Denominator == 0)
                return FromRational(0);
            if (Rational.Denominator != 1 || rb.Rational.Denominator != 1)
                throw new InvalidOperationException("Can't perform modulo of non-integers.");

            return FromInteger(Rational.Numerator % rb.Rational.Numerator);
        }

        public override string ToString(int radix, bool forceSign = false)
        {
            var sign = Sign > 0 ? (forceSign ? "+" : "") : "-";

            if (Rational.IsZero) return "0";
            if (Rational.Denominator == 1)
            {
                return $"{sign}{BigInteger.Abs(Rational.Numerator).ToString(radix)}";
            }
            return $"{sign}{BigInteger.Abs(Rational.Numerator).ToString(radix)}/{BigInteger.Abs(Rational.Denominator).ToString(radix)}";
        }

        private static RationalPart DoUnary(RationalPart a, Func<Rational, Rational> func)
        {
            return new RationalPart(func(a._value));
        }

        private static RationalPart DoBinary(IComplexPart a, IComplexPart b, Func<Rational, Rational, Rational> func)
        {
            if (!(a is RationalPart) || !(b is RationalPart))
                throw new InvalidOperationException();

            var ra = a as RationalPart;
            var rb = b as RationalPart;

            return new RationalPart(func(ra._value, rb._value));
        }

        public override bool Equals(IComplexPart other)
        {
            if (!(other is RationalPart))
                throw new InvalidOperationException();

            return this.Rational.Equals(((RationalPart)other).Rational);
        }

        public override int CompareTo(IComplexPart other)
        {
            if (!(other is RationalPart))
                throw new InvalidOperationException();

            return this.Rational.CompareTo(((RationalPart)other).Rational);
        }

        public static RationalPart FromInteger(BigInteger i) { return new RationalPart(new Rational(i, 1)); }
        public static RationalPart FromRational(Rational r) { return new RationalPart(r); }
    }
}
