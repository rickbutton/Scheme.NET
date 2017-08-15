using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine
{
    public interface IEnvironment
    {
        Scope Scope { get; }
        ISExpression Lookup(ISExpression sym);
        bool IsDefined(ISExpression sym);
        bool Set(ISExpression sym, ISExpression val);
        void DefineHere(ISExpression sym, ISExpression val);
    }
}
