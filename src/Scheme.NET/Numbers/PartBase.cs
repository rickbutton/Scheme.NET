using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Numbers
{
    public abstract class PartBase : IComplexPart
    {
        public abstract int Sign { get; }
        public abstract bool IsPositive { get; }
        public abstract bool IsNegative { get; }
        public abstract bool IsZero { get; }
        public abstract bool IsInteger { get; }
        public abstract bool IsEven { get; }
        public abstract bool IsOdd { get; }
        public abstract object BackingValue { get; }

        public abstract IComplexPart Add(IComplexPart y);
        public abstract IComplexPart Sub(IComplexPart y);
        public abstract IComplexPart Mul(IComplexPart y);
        public abstract IComplexPart Div(IComplexPart y);
        public abstract IComplexPart GCD(IComplexPart y);
        public abstract IComplexPart Modulo(IComplexPart y);
        public abstract IComplexPart Negate();
        public abstract IComplexPart Abs();
        public abstract IComplexPart Floor();
        public abstract IComplexPart Ceiling();
        public abstract IComplexPart Truncate();
        public abstract IComplexPart Round();
        public abstract bool Equals(IComplexPart other);

        public abstract int CompareTo(IComplexPart other);
        public abstract string ToString(int radix, bool forceSign = false);

        public bool GreaterThan(IComplexPart y) { return this.CompareTo(y) > 0; }
        public bool GreaterThanOrEqual(IComplexPart y) { return this.CompareTo(y) >= 0; }
        public bool LessThan(IComplexPart y) { return this.CompareTo(y) < 0; }
        public bool LessThanOrEqual(IComplexPart y) { return this.CompareTo(y) <= 0; }

    }
}
