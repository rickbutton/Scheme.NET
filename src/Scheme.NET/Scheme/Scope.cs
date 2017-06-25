using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Scheme
{
    public class Scope
    {
        public IDictionary<SymbolAtom, ISExpression> Data { get; private set; }
        public Scope Parent { get; private set; }

        public Scope(Scope parent = null)
        {
            Data = new Dictionary<SymbolAtom, ISExpression>();
            Parent = parent;
        }

        public Scope(IDictionary<SymbolAtom, ISExpression> data)
        {
            Data = data;
            Parent = null;
        }

        public ISExpression Lookup(SymbolAtom sym)
        {
            if (Data.ContainsKey(sym))
                return Data[sym];

            if (Parent != null)
                return Parent.Lookup(sym);

            return null;
        }

        public bool IsDefinedHere(SymbolAtom sym) { return Data.ContainsKey(sym); }

        public void Define(SymbolAtom sym, ISExpression sexpr)
        {
            Data[sym] = sexpr;
        }

        public void DefineHigh(SymbolAtom sym, ISExpression sexpr)
        {
            if (Parent == null || IsDefinedHere(sym))
                Define(sym, sexpr);
            else
                Parent.DefineHigh(sym, sexpr);
        }

        public Scope GetOuter()
        {
            if (Parent == null)
                return this;
            else
                return Parent.GetOuter();
        }
    }
}
