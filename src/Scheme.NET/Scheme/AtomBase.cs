using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheme.NET.Numbers;

namespace Scheme.NET.Scheme
{
    public abstract class AtomBase<T> : ISExpression
    {
        public T Val { get; private set; }
        protected AtomBase(T val) { Val = val; }

        public virtual string String() { return Val.ToString(); }

        public bool Equals(ISExpression other)
        {
            if (other is AtomBase<T>)
            {
                var a = other as AtomBase<T>;
                return Val.Equals(a.Val);
            }
            return false;
        }

        public override string ToString()
        {
            return Val.ToString();
        }
    }

    public class NumberAtom : AtomBase<Complex> { internal NumberAtom(Complex val) : base(val) { } }
    public class BooleanAtom : AtomBase<bool> {
        internal BooleanAtom(bool val) : base(val) { }
        public override string String()
        {
            return Val ? "#t" : "#f";
        }
    }
    public class SymbolAtom : AtomBase<string> { internal SymbolAtom(string val) : base(val) { } }
    public class StringAtom : AtomBase<string> { internal StringAtom(string val) : base(val) { } }
    public class CharAtom : AtomBase<char> { internal CharAtom(char val) : base(val) { } }
}
