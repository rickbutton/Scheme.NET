using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheme.NET.Numbers;
using System.Text.RegularExpressions;
using System.Numerics;
using Complex = Scheme.NET.Numbers.Complex;
using System.Linq.Expressions;
using Scheme.NET.VirtualMachine;
using Scheme.NET.VirtualMachine.Compiler;
using Scheme.NET.VirtualMachine.Natives;
using Scheme.NET.VirtualMachine.Instructions;

namespace Scheme.NET.Scheme
{
    public static class AtomHelper
    {
        private static readonly IDictionary<string, SymbolAtom> _symbolCache 
            = new Dictionary<string, SymbolAtom>();

        public static readonly string NumberRegex = "^((?:#\\S)*)(\\+|-)?(?:([\\da-fA-F]+)(?:([\\/\\.])([\\da-fA-F]+))?)(?:([\\+-])(?:([\\da-fA-F]+)(?:([\\/\\.])([\\da-fA-F]+))?)i)?\r?$";

        private static readonly string ValidExactnessPrefixes = "ei";
        private static readonly string ValidRadixPrefixes = "bodx";
        private static readonly string ValidPrefixes = ValidExactnessPrefixes + ValidRadixPrefixes;

        public static NumberAtom NumberFromComplex(Complex val)
        {
            return new NumberAtom(val);
        }

        public static NumberAtom NumberFromComplex(BigInteger val)
        {
            return new NumberAtom(Complex.CreateExactReal(val));
        }

        public static NumberAtom NumberFromString(string str)
        {
            NumberAtom result;
            if (!TryNumberFromString(str, out result))
                throw new InvalidOperationException("number has invalid syntax");
            return result;
        }

        public static bool TryNumberFromString(string str, out NumberAtom result)
        {
            var match = Regex.Match(str, NumberRegex, RegexOptions.Multiline);

            if (!match.Success)
            {
                result = null;
                return false;
            }

            string[] prefixes = Split(match.Groups[1].Value, 2);

            if (prefixes.Any(p => !ValidPrefixes.Contains(p.Substring(1))))
            {
                result = null;
                return false;
            }

            if (prefixes.Count(p => ValidExactnessPrefixes.Contains(p.Substring(1))) > 1)
            {
                result = null;
                return false;
            }

            if (prefixes.Count(p => ValidRadixPrefixes.Contains(p.Substring(1))) > 1)
            {
                result = null;
                return false;
            }

            var realSign = match.Groups[2].Value;
            var realTop = match.Groups[3].Value;
            var realDiv = match.Groups[4].Value;
            var realBot = match.Groups[5].Value;

            var imagSign = match.Groups[6].Value;
            var imagTop = match.Groups[7].Value;
            var imagDiv = match.Groups[8].Value;
            var imagBot = match.Groups[9].Value;

            if (string.IsNullOrEmpty(realSign)) realSign = "+";
            if (string.IsNullOrEmpty(realDiv))
            {
                realDiv = "/";
                realBot = "1";
            }

            if (string.IsNullOrEmpty(imagTop))
                imagTop = "0";

            if (string.IsNullOrEmpty(imagSign)) imagSign = "+";
            if (string.IsNullOrEmpty(imagDiv))
            {
                imagDiv = "/";
                imagBot = "1";
            }

            bool? exactness = null;
            if (prefixes.Contains("#e")) exactness = true;
            else if (prefixes.Contains("#i")) exactness = false;
            else if (realDiv == "." || imagDiv == ".") exactness = false;
            else exactness = true;

            int radix = 10;
            if (prefixes.Contains("#b")) radix = 2;
            else if (prefixes.Contains("#o")) radix = 8;
            else if (prefixes.Contains("#d")) radix = 10;
            else if (prefixes.Contains("#x")) radix = 16;

            ComplexPart rr, ri;
            var success = ParseComplexPart(realSign, realTop, realDiv, realBot, radix, out rr);
            if (!success) { result = null; return false; }

            success = ParseComplexPart(imagSign, imagTop, imagDiv, imagBot, radix, out ri);
            if (!success) { result = null; return false; }

            if (exactness.Value)
                result = NumberFromComplex(Complex.CreateExact(rr, ri));
            else
                result = NumberFromComplex(Complex.CreateInExact(rr, ri));
            return true;
        }

        private static bool ParseComplexPart(string sign, string top, string div, string bot, int radix, out ComplexPart r)
        {
            var isign = sign == "+" ? 1 : -1;
            if (div == "/" && bot == "0")
            {
                r = ComplexPart.Zero;
                return false;
            }

            ComplexPart result;
            if (div == "/")
            {
                BigInteger t, b;
                var success = BigIntegerHelpers.TryParse(top, radix, out t);
                if (!success) { r = ComplexPart.Zero; return false; }

                success = BigIntegerHelpers.TryParse(bot, radix, out b);
                if (!success) { r = ComplexPart.Zero; return false; }

                result = new ComplexPart(t, b);
            }
            else
            {
                BigInteger n;
                var success = BigIntegerHelpers.TryParse(top + bot, radix, out n);
                if (!success) { r = ComplexPart.Zero; return false; }

                ComplexPart factor = ComplexPart.Pow(ComplexPart.CreateFromBigInteger(radix), -bot.Length);
                result = n * factor;
            }
            r = result * isign;
            return true;
        }

        private static string[] Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize)).ToArray();
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
            if (c == "return")
                return new CharAtom('\r');
            if (c == "tab")
                return new CharAtom('\t');

            throw new InvalidOperationException("Unknown character name: " + c);
        }
        public static SymbolAtom SymbolFromString(string val)
        {
            val = val.ToLower();
            if (!_symbolCache.ContainsKey(val))
                _symbolCache[val] = new SymbolAtom(val);
            return _symbolCache[val];
        }

        public static Vector CreateVector(IEnumerable<ISExpression> items) { return new Vector(items.ToArray()); }
        public static Cons CreateCons(ISExpression car, ISExpression cdr) { return new Cons(car, cdr); }
        public static Procedure CreateProcedure(string name, Func<IEnumerable<ISExpression>, ISExpression> proc, bool primitive) { return new Procedure(name, proc, primitive); }

        public static ISExpression CreateList(params ISExpression[] exprs)
        {
            if (exprs.Length == 0)
                return Nil;

            var head = CreateCons(exprs[exprs.Length - 1], AtomHelper.Nil);
            for (var i = exprs.Length - 2; i >= 0; i--) {
                head = CreateCons(exprs[i], head);
            }
            return head;
        }


        public static IEnvironment CreateEnvironment()
        {
            return new VirtualMachine.Environment();
        }

        public static void PopulateEnvironment(IEnvironment e, ISchemeVM vm)
        {
            var lib = Library.CreateBase();
            foreach (var l in lib)
                CreatePrimitive(l.Key.Val, (Procedure)l.Value, e, vm);
        }

        private static void CreatePrimitive(string name, Procedure p, IEnvironment e, ISchemeVM vm)
        {
            var def = AtomHelper.CreateList(
                            AtomHelper.SymbolFromString("define"),
                AtomHelper.SymbolFromString(name),
                AtomHelper.CreateList(AtomHelper.SymbolFromString("lambda"), AtomHelper.Nil, p)
                );
            var inst = SchemeCompiler.Compile(vm, def);

            var tmp = vm.E;
            vm.E = e;
            vm.Execute(inst);
            vm.E = tmp;
        }

        public static readonly NilAtom Nil = new NilAtom();
        public static readonly BooleanAtom True = new BooleanAtom(true);
        public static readonly BooleanAtom False = new BooleanAtom(false);
    }
}
