using Scheme.NET.Numbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Scheme
{
    public abstract class AtomBase<T> : ISExpression
    {
        public T Val { get; private set; }
        internal AtomBase(T val) { Val = val; }

        public string String() { return Val.ToString(); }

        public bool Equals(ISExpression other)
        {
            if (other is AtomBase<T>)
            {
                var a = other as AtomBase<T>;
                return Val.Equals(a.Val);
            }
            return false;
        }
    }

    public class NumberAtom : AtomBase<BigRational> { internal NumberAtom(BigRational val) : base(val) { } }
    public class BooleanAtom : AtomBase<bool> { internal BooleanAtom(bool val) : base(val) { } }
    public class SymbolAtom : AtomBase<string> { internal SymbolAtom(string val) : base(val) { } }
    public class StringAtom : AtomBase<string> { internal StringAtom(string val) : base(val) { } }
    public class CharAtom : AtomBase<char> { internal CharAtom(char val) : base(val) { } }
}
