using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheme.NET.VirtualMachine
{
    public partial class Environment : IEnvironment
    {
        public IDictionary<SymbolAtom, EnvThunk> Map { get; private set; }

        public Environment()
        {
            Map = new Dictionary<SymbolAtom, EnvThunk>();
        }

        public Environment(IDictionary<SymbolAtom, ISExpression> vars)
        {
            Map = new Dictionary<SymbolAtom, EnvThunk>();
            foreach (var v in vars)
            {
                Map[v.Key] = new EnvThunk() { Val = v.Value };
            }
        }

        public Environment(IEnvironment e, IDictionary<SymbolAtom, ISExpression> vars)
        {
            Map = new Dictionary<SymbolAtom, EnvThunk>();
            foreach (var v in e.Map)
            {
                Map[v.Key] = v.Value;
            }
            foreach (var v in vars)
            {
                Map[v.Key] = new EnvThunk() { Val = v.Value };
            }
        }

        public bool IsDefined(ISExpression sym)
        {
            return Map.ContainsKey(sym as SymbolAtom);
        }

        public ISExpression Lookup(ISExpression sym)
        {
            if (!sym.IsSymbol())
                throw new InvalidOperationException();

            if (IsDefined(sym))
                return Map[sym as SymbolAtom].Val;

            return null;
        }

        public bool Set(ISExpression sym, ISExpression val)
        {
            var s = sym as SymbolAtom;
            if (Map.ContainsKey(s))
            {
                Map[s].Val = val;
                return true;
            }
            return false;
        }

        public void DefineHere(ISExpression sym, ISExpression val)
        {
            var s = sym as SymbolAtom;
            Map[s] = new EnvThunk() { Val = val };
        }

        public string String()
        {
            return $"#<environment c:{Map.Count}>";
        }

        public bool Equals(ISExpression other)
        {
            return this == other;
        }
    }
}
