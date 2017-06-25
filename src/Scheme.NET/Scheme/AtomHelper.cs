using Scheme.NET.Numbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Scheme
{
    public static class AtomHelper
    {
        private static readonly IDictionary<string, SymbolAtom> _symbolCache 
            = new Dictionary<string, SymbolAtom>();

        public static NumberAtom NumberFromBigDecimal(BigRational val)
        {
            return new NumberAtom(val);
        }

        public static NumberAtom NumberFromString(string val)
        {
            BigRational bd;
            if (BigRational.TryParse(val, out bd))
                return new NumberAtom(bd);

            if (val.StartsWith("#b"))
                return new NumberAtom(Convert.ToInt64(val.Substring(2), 2));
            if (val.StartsWith("#o"))
                return new NumberAtom(Convert.ToInt64(val.Substring(2), 8));
            if (val.StartsWith("#d"))
                return new NumberAtom(Convert.ToInt64(val.Substring(2)));
            if (val.StartsWith("#x"))
                return new NumberAtom(Convert.ToInt64(val.Substring(2), 16));

            throw new InvalidOperationException($"invalid number " + val);
        }
        
        public static BooleanAtom BooleanFromBool(bool val) { return val ? True : False; }
        public static BooleanAtom BooleanFromString(string val) { return val == "#f" ? False: True; }
        public static StringAtom StringFromString(string val) { return new StringAtom(val); }
        public static CharAtom CharFromString(string val) {
            var c = val.Substring(2);

            if (c.Length == 0)
                return new CharAtom(' ');
            if (c.Length == 1)
                return new CharAtom(c[0]);

            if (c == "space")
                return new CharAtom(' ');
            if (c == "newline")
                return new CharAtom('\n');

            throw new InvalidOperationException("Unknown character name: " + c);
        }
        public static SymbolAtom SymbolFromString(string val)
        {
            val = val.ToLower();
            if (!_symbolCache.ContainsKey(val))
                _symbolCache[val] = new SymbolAtom(val);
            return _symbolCache[val];
        }

        public static Cons CreateCons(ISExpression car, ISExpression cdr) { return new Cons(car, cdr); }
        public static Procedure CreateProcedure(string name, Func<Scope, IEnumerable<ISExpression>, ISExpression> proc, bool primitive) { return new Procedure(name, proc, primitive); }

        public static readonly NilAtom Nil = new NilAtom();
        public static readonly BooleanAtom True = new BooleanAtom(true);
        public static readonly BooleanAtom False = new BooleanAtom(false);
    }
}
