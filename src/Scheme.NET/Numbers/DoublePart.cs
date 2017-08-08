using Microsoft.SolverFoundation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Numbers
{
    public class DoublePart : PartBase
    {
        private double _value;

        public DoublePart(double value)
        {
            _value = value;
        }

        public override int Sign => CalcSign(_value);
        public override bool IsPositive => _value > 0;
        public override bool IsNegative => _value < 0;
        public override bool IsZero => _value == 0;
        public override bool IsInteger => _value % 1 == 0;
        public override bool IsEven => _value % 2 == 0;
        public override bool IsOdd => _value % 2 != 0;
        public override object BackingValue => _value;
        public double Double => _value;

        public override IComplexPart Add(IComplexPart b) { return DoBinary(this, b, (x, y) => x + y); }
        public override IComplexPart Sub(IComplexPart b) { return DoBinary(this, b, (x, y) => x - y); }
        public override IComplexPart Mul(IComplexPart b) { return DoBinary(this, b, (x, y) => x * y); }
        public override IComplexPart Div(IComplexPart b) { return DoBinary(this, b, (x, y) => x / y); }
        public override IComplexPart GCD(IComplexPart b) {
            if (!IsInteger || !b.IsInteger)
                throw new InvalidOperationException();

            Func<double, double, double> func = null;
            func = (x, y) => y == 0 ? x : func(y, x % y);
            return DoBinary(this, b, func);
        }
        public override IComplexPart Modulo(IComplexPart b)
        {
            if (!(b is DoublePart))
                throw new InvalidOperationException();

            var db = b as DoublePart;
            return FromDouble(Double % db.Double);
        }
        public override IComplexPart Negate() { return DoUnary(this, (x) => -x); }
        public override IComplexPart Abs() { return DoUnary(this, Math.Abs); }
        public override IComplexPart Floor() { return DoUnary(this, Math.Floor); }
        public override IComplexPart Ceiling() { return DoUnary(this, Math.Ceiling); }
        public override IComplexPart Truncate() { return DoUnary(this, Math.Truncate); }
        public override IComplexPart Round() { return DoUnary(this, Math.Round); }

        public override string ToString(int radix, bool forceSign = false)
        {
            var sign = Double >= 0 ? (forceSign ? "+" : "") : "-";

            var intPart = (long)Double;
            var intStr = ((BigInteger)intPart).AbsoluteValue.ToString(radix);
            var fraPart = Double - intPart;
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

        private static int CalcSign(double d)
        {
            if (d < 0) return -1;
            else if (d > 0) return 1;
            return 0;
        }

        private static DoublePart DoUnary(DoublePart a, Func<double, double> func)
        {
            return new DoublePart(func(a._value));
        }

        private static DoublePart DoBinary(IComplexPart a, IComplexPart b, Func<double, double, double> func)
        {
            if (!(a is DoublePart) || !(b is DoublePart))
                throw new InvalidOperationException();

            var ra = a as DoublePart;
            var rb = b as DoublePart;

            return new DoublePart(func(ra._value, rb._value));
        }

        public override bool Equals(IComplexPart other)
        {
            if (!(other is DoublePart))
                throw new InvalidOperationException();

            return this.Double.Equals(((DoublePart)other).Double);
        }

        public override int CompareTo(IComplexPart other)
        {
            if (!(other is DoublePart))
                throw new InvalidOperationException();

            return this.Double.CompareTo(((DoublePart)other).Double);
        }

        public static DoublePart FromDouble(double d) { return new DoublePart(d); }
    }
}
