using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Scheme
{
    public class Procedure : ISExpression
    {
        public string Name { get; private set; }
        public Func<Scope, IEnumerable<ISExpression>, ISExpression> Proc { get; private set; }
        public bool Primitive { get; private set; }

        public Procedure(string name, Func<Scope, IEnumerable<ISExpression>, ISExpression> procedure, bool primitive)
        {
            Name = name;
            Proc = procedure;
            Primitive = primitive;
        }

        public string String() { return Name; }

        public bool Equals(ISExpression other)
        {
            if (other is Procedure)
            {
                var p = other as Procedure;
                return Name == p.Name && Proc == p.Proc && Primitive == p.Primitive;
            }
            return false;
        }
    }
}
