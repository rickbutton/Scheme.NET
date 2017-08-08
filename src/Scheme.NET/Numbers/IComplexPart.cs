using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Numbers
{
    public interface IComplexPart : IComparable<IComplexPart>, IEquatable<IComplexPart>
    {
        int Sign { get; }
        bool IsPositive { get; }
        bool IsNegative { get; }
        bool IsZero { get; }
        bool IsInteger { get; }
        bool IsEven { get; }
        bool IsOdd { get; }

        object BackingValue { get; }

        IComplexPart Add(IComplexPart y);
        IComplexPart Sub(IComplexPart y);
        IComplexPart Mul(IComplexPart y);
        IComplexPart Div(IComplexPart y);
        IComplexPart GCD(IComplexPart y);
        IComplexPart Modulo(IComplexPart y);
        IComplexPart Negate();
        IComplexPart Abs();
        IComplexPart Floor();
        IComplexPart Ceiling();
        IComplexPart Truncate();
        IComplexPart Round();

        bool LessThan(IComplexPart y);
        bool GreaterThan(IComplexPart y);
        bool LessThanOrEqual(IComplexPart y);
        bool GreaterThanOrEqual(IComplexPart y);

        string ToString(int radix, bool forceSign = false);
    }
}
