using Scheme.NET.VirtualMachine;
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

        public Scope(Scope parent)
        {
            Data = new Dictionary<SymbolAtom, ISExpression>();
            Parent = parent;
        }

        public Scope(IDictionary<SymbolAtom, ISExpression> data)
        {
            Data = data;
            Parent = null;
        }

        public Scope()
        {
            Data = new Dictionary<SymbolAtom, ISExpression>();
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
        public bool IsDefined(SymbolAtom sym) {
            if (IsDefinedHere(sym))
            {
                return true;
            }

            if (Parent == null)
                return false;

            return Parent.IsDefined(sym);
        }

        public void DefineHere(SymbolAtom sym, ISExpression sexpr)
        {
            Data[sym] = sexpr;
        }

        public bool Set(SymbolAtom sym, ISExpression sexpr)
        {
            if (IsDefinedHere(sym))
            {
                DefineHere(sym, sexpr);
                return true;
            }
            else if (Parent == null)
                return false;
            else
                return Parent.Set(sym, sexpr);
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
