using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine.Instructions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine
{
    public class Closure : ISExpression
    {
        public IInstruction Body { get; private set; }
        public IEnvironment Env { get; private set; }
        public ISExpression Vars { get; private set; }

        public Closure(IInstruction b, IEnvironment e, ISExpression a)
        {
            Body = b;
            Env = e;
            Vars = a;
        }

        public bool Equals(ISExpression other)
        {
            if (other is Closure)
            {
                var c = other as Closure;
                return Body == c.Body && Vars == c.Vars;
            }
            return false;
        }

        public static bool operator ==(Closure a, Closure b)
        {
            if (((object)a) == null && ((object)b) == null) return true;
            else if (((object)a) == null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(Closure a, Closure b)
        {
            if (((object)a) == null && ((object)b) == null) return false;
            else if (((object)a) == null) return false;
            return !a.Equals(b);
        }

        public string String()
        {
            return "#closure";
        }
    }
}
