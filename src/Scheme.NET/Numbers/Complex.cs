using MiscUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Numbers
{
    public abstract class Complex
    {
        private readonly object real, imaginary;
        public object Real { get { return real; } }
        public object Imaginary { get { return imaginary; } }

        protected Complex(object real, object imaginary)
        {
            if (real.GetType() != imaginary.GetType())
                throw new ArgumentException("Can't create complex with two different types");

            this.real = real;
            this.imaginary = imaginary;
        }

        public Complex<T> As<T>() where T : IEquatable<T>
        {
            return new Complex<T>((T)Real, (T)Imaginary);
        }

        public Type PartType => Real.GetType();

        public abstract Complex Add(Complex y);
        public static Complex operator +(Complex x, Complex y) { return x.Add(y); }

        public abstract Complex Subtract(Complex y);
        public static Complex operator -(Complex x, Complex y) { return x.Subtract(y); }

        public abstract Complex Negate();
        public static Complex operator -(Complex x) { return x.Negate(); }

        public static bool operator ==(Complex x, Complex y)
        {
            if (((object)x) == null && ((object)y) == null) return true;
            else if (((object)x) == null) return false;
            return x.Equals(y);
        }

        public static bool operator !=(Complex x, Complex y)
        {
            if (x == null && y == null) return true;
            else if (x == null) return false;
            return !x.Equals(y);
        }

        public abstract Complex Multiply(Complex y);
        public static Complex operator *(Complex x, Complex y)
        {
            return x.Multiply(y);
        }

        public abstract Complex Divide(Complex y);
        public static Complex operator /(Complex x, Complex y)
        {
            return x.Divide(y);
        }

        public abstract bool RealIsZero { get; }
        public abstract bool RealIsPositive { get; }
        public abstract bool RealIsNegative { get; }
        public abstract bool ImaginaryIsZero { get; }
        public abstract bool ImaginaryIsPositive { get; }
        public abstract bool ImaginaryIsNegative { get; }
        public abstract bool IsZero { get; }

        public abstract Complex Abs();
    }

    public class Complex<T> : Complex, IEquatable<Complex<T>> where T : IEquatable<T>
    {

        public Complex(T real, T imaginary) : base (real, imaginary)
        {
        }

        public T Real { get { return (T)((Complex)this).Real; } }
        public T Imaginary { get { return (T)((Complex)this).Imaginary; } }

        public T RelativeLength()
        {
            return Complex<T>.SquareLength(this);
        }

        public override bool RealIsZero => Real.Equals(Operator<T>.Zero);
        public override bool RealIsPositive => Operator<T>.GreaterThan(Real, Operator<T>.Zero);
        public override bool RealIsNegative => Operator<T>.LessThan(Real, Operator<T>.Zero);
        public override bool ImaginaryIsZero => Imaginary.Equals(Operator<T>.Zero);
        public override bool ImaginaryIsPositive => Operator<T>.GreaterThan(Imaginary, Operator<T>.Zero);
        public override bool ImaginaryIsNegative => Operator<T>.LessThan(Imaginary, Operator<T>.Zero);
        public override bool IsZero => (Real.Equals(Operator<T>.Zero)) && (Imaginary.Equals(Operator<T>.Zero));

        public override Complex Abs()
        {
            if (!ImaginaryIsZero)
                throw new InvalidOperationException("finding abs of complex number with non-zero i is unsupported");

            if (Operator<T>.LessThan(Real, Operator<T>.Zero))
                return new Complex<T>(Operator<T>.Negate(Real), Operator<T>.Zero);
            return this;
        }

        public override string ToString()
        {
            if (Imaginary.Equals(Operator<T>.Zero))
            {
                return Real.ToString();
            }
            return $"{Real}+{Imaginary}i";
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Complex)
            {
                Complex other = (Complex)obj;
                return Equals(this, other);
            }
            return base.Equals(obj);
        }

        public bool Equals(Complex<T> other)
        {
            return Equals(this, other);
        }

        private static bool Equals(Complex<T> x, Complex<T> y)
        {
            return EqualityComparer<T>.Default.Equals(x.Real, y.Real)
                && EqualityComparer<T>.Default.Equals(x.Imaginary, y.Imaginary);
        }

        private static bool Equals(Complex x, Complex y)
        {
            x = x.PromoteRelative(y);
            y = y.PromoteRelative(x);
            return x.Real.Equals(y.Real) && x.Imaginary.Equals(y.Imaginary);
        }

        private void EnsureSameType(Complex c)
        {
            if (c.PartType != PartType)
            {
                throw new InvalidOperationException(
                    "Attempted to perform binary operation with disjoint Complex types");
            }
        }

        public override Complex Add(Complex y)
        {
            EnsureSameType(y);
            return this + y.As<T>();
        }

        public static Complex<T> operator +(Complex<T> x, Complex<T> y)
        {
            return new Complex<T>(
                Operator.Add(x.Real, y.Real),
                Operator.Add(x.Imaginary, y.Imaginary)
            );
        }

        public static Complex<T> operator +(Complex<T> x, T y)
        {
            return new Complex<T>(
                Operator.Add(x.Real, y),
                x.Imaginary
            );
        }

        public override Complex Subtract(Complex y)
        {
            EnsureSameType(y);
            return this - y.As<T>();
        }

        public static Complex<T> operator -(Complex<T> x, Complex<T> y)
        {
            return new Complex<T>(
                Operator.Subtract(x.Real, y.Real),
                Operator.Subtract(x.Imaginary, y.Imaginary)
            );
        }

        public static Complex<T> operator -(Complex<T> x, T y)
        {
            return new Complex<T>(
                Operator.Subtract(x.Real, y),
                x.Imaginary
            );
        }

        public override Complex Negate()
        {
            return -this;
        }

        public static Complex<T> operator -(Complex<T> x)
        {
            return new Complex<T>(
                Operator.Negate(x.Real),
                Operator.Negate(x.Imaginary)
            );
        }

        public static bool operator ==(Complex<T> x, Complex<T> y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Complex<T> x, Complex<T> y)
        {
            return !Equals(x, y);
        }

        public override Complex Multiply(Complex y)
        {
            EnsureSameType(y);
            return this * y.As<T>();
        }

        public static Complex<T> operator *(Complex<T> x, Complex<T> y)
        {
            return new Complex<T>(
                Operator.Subtract(
                    Operator.Multiply(x.Real, y.Real),
                    Operator.Multiply(y.Imaginary, y.Imaginary)
                ), Operator.Add(
                    Operator.Multiply(x.Real, y.Imaginary),
                    Operator.Multiply(x.Imaginary, y.Real)
                )
            );
        }

        public static Complex<T> operator *(Complex<T> x, T y)
        {
            return new Complex<T>(
                Operator.Multiply(x.Real, y),
                Operator.Multiply(x.Imaginary, y)
            );
        }

        public static Complex<T> operator *(Complex<T> x, int y)
        {
            return new Complex<T>(
                Operator.MultiplyAlternative(x.Real, y),
                Operator.MultiplyAlternative(x.Imaginary, y)
            );
        }

        private static T SquareLength(Complex<T> value)
        {
            return Operator.Add(
                Operator.Multiply(value.Real, value.Real),
                Operator.Multiply(value.Imaginary, value.Imaginary)
            );
        }

        public override Complex Divide(Complex y)
        {
            EnsureSameType(y);
            return this / y.As<T>();
        }

        public static Complex<T> operator /(Complex<T> x, Complex<T> y)
        {
            T divisor = SquareLength(y),
              real = Operator.Divide(
                    Operator.Add(
                        Operator.Multiply(x.Real, y.Real),
                        Operator.Multiply(x.Imaginary, y.Imaginary)
                    ), divisor),
              imaginary = Operator.Divide(
                    Operator.Subtract(
                        Operator.Multiply(x.Imaginary, y.Real),
                        Operator.Multiply(x.Real, y.Imaginary)
                    ), divisor);
            return new Complex<T>(real, imaginary);
        }

        public static Complex<T> operator /(Complex<T> x, T y)
        {
            return new Complex<T>(
                Operator.Divide(x.Real, y),
                Operator.Divide(x.Imaginary, y)
            );
        }

        public static Complex<T> operator /(Complex<T> x, int y)
        {
            return new Complex<T>(
                Operator.DivideInt32(x.Real, y),
                Operator.DivideInt32(x.Imaginary, y)
            );
        }

        public static implicit operator Complex<T>(T real)
        {
            return new Complex<T>(real, Operator<T>.Zero);
        }

        public override int GetHashCode()
        {
            return (Real == null ? 0 : 17 * Real.GetHashCode())
                 + (Imaginary == null ? 0 : Imaginary.GetHashCode());
        }
    }
}
