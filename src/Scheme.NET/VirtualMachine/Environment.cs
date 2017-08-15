using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheme.NET.VirtualMachine
{
    public class Environment : IEnvironment
    {
        public Scope Scope { get; private set; }

        public Environment()
        {
            Scope = new Scope();
        }

        public Environment(IEnvironment e, IDictionary<SymbolAtom, ISExpression> vars)
        {
            Scope = new Scope(e.Scope);

            foreach (var v in vars)
            {
                Scope.DefineHere(v.Key as SymbolAtom, v.Value);
            }
        }

        public bool IsDefined(ISExpression sym)
        {
            return Scope.IsDefined(sym as SymbolAtom);
        }

        public ISExpression Lookup(ISExpression sym)
        {
            if (!sym.IsSymbol())
                throw new InvalidOperationException();

            return Scope.Lookup(sym as SymbolAtom);
        }

        public bool Set(ISExpression sym, ISExpression val)
        {
            return Scope.Set((SymbolAtom)sym, val);
        }

        public void DefineHere(ISExpression sym, ISExpression val)
        {
            Scope.DefineHere((SymbolAtom)sym, val);
        }
    }
}
