using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;
using static Scheme.NET.VirtualMachine.Environment;

namespace Scheme.NET.VirtualMachine
{
    public interface IEnvironment
    {
        IDictionary<SymbolAtom, EnvThunk> Map { get; }
        ISExpression Lookup(ISExpression sym);
        bool IsDefined(ISExpression sym);
        bool Set(ISExpression sym, ISExpression val);
        void DefineHere(ISExpression sym, ISExpression val);
    }
}
