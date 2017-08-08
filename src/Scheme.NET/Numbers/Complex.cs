using Microsoft.SolverFoundation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Numbers
{
    public class Complex : IComparable<Complex>, IEquatable<Complex>
    {
        public IComplexPart Real { get; }
        public IComplexPart Imag { get; }
        public bool IsExact { get; private set; }

        public bool IsZero => Real.IsZero && Imag.IsZero;
        public bool IsInteger => Real.IsInteger && Imag.IsInteger;

        private Complex(IComplexPart real, IComplexPart imag, bool isExact)
        {
            Real = real;
            Imag = imag;
            IsExact = isExact;
        }

        public Complex(RationalPart real, RationalPart imag)
        {
            Real = real;
            Imag = imag;
            IsExact = true;
        }

        public Complex(DoublePart real, DoublePart imag)
        {
            Real = real;
            Imag = imag;
            IsExact = false;
        }

        public static Complex operator +(Complex a, Complex b) { return DoBinary(a, b, (x, y) => x.Add(y)); }
        public static Complex operator -(Complex a, Complex b) { return DoBinary(a, b, (x, y) => x.Sub(y)); }
        public static Complex operator -(Complex a) { return DoUnary(a, (x) => x.Negate()); }
        public static Complex operator *(Complex x, Complex y) {
            PromoteExactness(x, y, out x, out y);
            var newReal = x.Real.Mul(y.Real).Sub(y.Imag.Mul(y.Imag));
            var newImag = x.Real.Mul(y.Imag).Add(x.Imag.Mul(y.Real));
            return new Complex(newReal, newImag, x.IsExact);
        }
        public static Complex operator /(Complex x, Complex y) {
            PromoteExactness(x, y, out x, out y);
            IComplexPart divisor = y.Real.Mul(y.Real).Add(y.Imag.Mul(y.Imag));
            var newReal = x.Real.Mul(y.Real).Add(x.Imag.Mul(y.Imag)).Div(divisor);
            var newImag = x.Imag.Mul(y.Real).Sub(x.Real.Mul(y.Imag)).Div(divisor);
            return new Complex(newReal, newImag, x.IsExact);
        }

        public string ToString(int radix)
        {
            string real = Real.ToString(radix);
            string imag = Imag.ToString(radix, true);

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

        private static Complex DoRealUnary(Complex a, Func<IComplexPart, IComplexPart> func)
        {
            if (!a.Imag.IsZero)
                throw new InvalidOperationException();
            return new Complex(func(a.Real), a.Imag, a.IsExact);
        }

        private static Complex DoRealBinary(Complex a, Complex b, Func<IComplexPart, IComplexPart, IComplexPart> func)
        {
            if (!a.Imag.IsZero)
                throw new InvalidOperationException();
            return new Complex(func(a.Real, b.Real), RationalPart.FromInteger(0), a.IsExact);
        }

        private static Complex DoUnary(Complex a, Func<IComplexPart, IComplexPart> func)
        {
            return new Complex(func(a.Real), func(a.Imag), a.IsExact);
        }

        private static Complex DoBinary(Complex a, Complex b, Func<IComplexPart, IComplexPart, IComplexPart> func)
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

            var real = this.Real as RationalPart;
            var imag = this.Imag as RationalPart;

            return new Complex(
                new DoublePart(real.Rational.GetSignedDouble()),
                new DoublePart(imag.Rational.GetSignedDouble()));
        }

        public static Complex FromRationals(Rational a, Rational b)
        {
            return new Complex(RationalPart.FromRational(a), RationalPart.FromRational(b));
        }

        public static Complex FromDoubles(double a, double b)
        {
            return new Complex(DoublePart.FromDouble(a), DoublePart.FromDouble(b));
        }

        public static Complex FromInteger(BigInteger i)
        {
            return new Complex(RationalPart.FromInteger(i), RationalPart.FromInteger(0));
        }

        public static Complex FromDouble(double d)
        {
            return new Complex(DoublePart.FromDouble(d), DoublePart.FromDouble(0));
        }

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
