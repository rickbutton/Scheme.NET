using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Scheme
{
    public class NilAtom : ISExpression
    {
        public string String() { return "()"; }

        public bool Equals(ISExpression other)
        {
            return other is NilAtom;
        }

        internal NilAtom() { }
    }
}
