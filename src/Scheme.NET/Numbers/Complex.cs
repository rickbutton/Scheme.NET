using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Numbers
{
    public class Complex : IComparable<Complex>, IEquatable<Complex>
    {
        public ComplexPart Real { get; }
        public ComplexPart Imag { get; }
        public bool IsExact { get; private set; }

        public bool IsZero => Real.IsZero && Imag.IsZero;
        public bool IsInteger => Real.IsInteger() && Imag.IsInteger();

        public Complex(ComplexPart real, ComplexPart imag, bool isExact)
        {
            Real = real;
            Imag = imag;
            IsExact = isExact;
        }

        public static Complex operator +(Complex a, Complex b) { return DoBinary(a, b, (x, y) => x + y); }
        public static Complex operator -(Complex a, Complex b) { return DoBinary(a, b, (x, y) => x - y); }
        public static Complex operator -(Complex a) { return DoUnary(a, (x) => -x); }
        public static Complex operator *(Complex x, Complex y) {
            PromoteExactness(x, y, out x, out y);
            var newReal = (x.Real * y.Real) - (y.Imag * y.Imag);
            var newImag = (x.Real * y.Imag) + (x.Imag * y.Real);
            return new Complex(newReal, newImag, x.IsExact);
        }
        public static Complex operator /(Complex x, Complex y) {
            PromoteExactness(x, y, out x, out y);
            ComplexPart divisor = (y.Real * y.Real) + (y.Imag * y.Imag);
            var newReal = ((x.Real * y.Real) + (x.Imag * y.Imag)) / divisor;
            var newImag = ((x.Imag * y.Real) - (x.Real * y.Imag)) / divisor;
            return new Complex(newReal, newImag, x.IsExact);
        }

        public string ToString(int radix)
        {
            string real, imag;
            if (IsExact)
            {
                real = Real.ToString(radix);
                imag = Imag.ToString(radix, true);
            }
            else
            {
                real = Real.ToDecimalString(radix);
                imag = Imag.ToDecimalString(radix, true);
            }

            if (!Imag.IsZero)
            {
                return $"{real}{imag}i";
            }
            return real;
        }

        public override string ToString()
        {
            return ToString(10);
        }

        public Complex RealAbs()
        {
            if (!Imag.IsZero)
                throw new InvalidOperationException();
            return new Complex(Real.Abs(), Imag, IsExact);
        }

        public Complex RealFloor() { return DoRealUnary(this, (x) => x.Floor()); }
        public Complex RealCeiling() { return DoRealUnary(this, (x) => x.Ceiling()); }
        public Complex RealTruncate() { return DoRealUnary(this, (x) => x.Truncate()); }
        public Complex RealRound() { return DoRealUnary(this, (x) => x.Round()); }
        public Complex RealGCD(Complex b) { return DoRealBinary(this, b, (x, y) => x.GCD(y)); }
        public Complex RealModulo(Complex b) { return DoRealBinary(this, b, (x, y) => x.Modulo(y)); }

        private static Complex DoRealUnary(Complex a, Func<ComplexPart, ComplexPart> func)
        {
            if (!a.Imag.IsZero)
                throw new InvalidOperationException();
            return new Complex(func(a.Real), a.Imag, a.IsExact);
        }

        private static Complex DoRealBinary(Complex a, Complex b, Func<ComplexPart, ComplexPart, ComplexPart> func)
        {
            if (!a.Imag.IsZero)
                throw new InvalidOperationException();
            return new Complex(func(a.Real, b.Real), ComplexPart.Zero, a.IsExact);
        }

        private static Complex DoUnary(Complex a, Func<ComplexPart, ComplexPart> func)
        {
            return new Complex(func(a.Real), func(a.Imag), a.IsExact);
        }

        private static Complex DoBinary(Complex a, Complex b, Func<ComplexPart, ComplexPart, ComplexPart> func)
        {
            PromoteExactness(a, b, out a, out b);
            return new Complex(func(a.Real, b.Real), func(a.Imag, b.Imag), a.IsExact);
        }

        public int CompareTo(Complex b)
        {
            if (!Imag.IsZero)
                throw new InvalidOperationException();
            var a = this;
            PromoteExactness(a, b, out a, out b);
            return a.Real.CompareTo(b.Real);
        }

        public Complex PromoteRelative(Complex to)
        {
            var a = this;
            PromoteExactness(a, to, out a, out to);
            return a;
        }

        public Complex PromoteRelative(IEnumerable<Complex> tos)
        {
            var a = this;
            foreach (var b in tos)
            {
                var to = b;
                PromoteExactness(a, to, out a, out to);
            }
            return a;
        }

        public static void PromoteExactness(Complex a, Complex b, out Complex outA, out Complex outB)
        {
            if (!a.IsExact || !b.IsExact)
            {
                if (a.IsExact)
                    outA = a.ExactToInexact();
                else
                    outA = a;

                if (b.IsExact)
                    outB = b.ExactToInexact();
                else
                    outB = b;
            }
            else
            {
                outA = a;
                outB = b;
            }
        }

        public Complex ExactToInexact()
        {
            if (!this.IsExact)
                throw new InvalidOperationException();

            return new Complex(Real, Imag, false);
        }

        public static Complex CreateExactReal(BigInteger a) { return new Complex(new ComplexPart(a), ComplexPart.Zero, true); }
        public static Complex CreateExact(ComplexPart a, ComplexPart b) { return new Complex(a, b, true); }
        public static Complex CreateInexactReal(double a) { return new Complex(ComplexPart.CreateFromDouble(a), ComplexPart.Zero, false); }
        public static Complex CreateInExact(ComplexPart a, ComplexPart b) { return new Complex(a, b, false); }

        public override int GetHashCode()
        {
            return Real.GetHashCode() ^ Imag.GetHashCode();
        }

        public static bool operator ==(Complex a, Complex b)
        {
            if (((object)a) == null && ((object)b) == null) return true;
            else if (((object)a) == null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(Complex a, Complex b)
        {
            if (((object)a) == null && ((object)b) == null) return false;
            else if (((object)a) == null) return false;
            return !a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is Complex) return this.Equals((Complex)obj);
            return false;
        }

        public bool Equals(Complex b)
        {
            if (((object)b) == null) return false;

            var a = this;
            PromoteExactness(a, b, out a, out b);
            return a.Real.Equals(b.Real) && a.Imag.Equals(b.Imag);
        }
    }
}
