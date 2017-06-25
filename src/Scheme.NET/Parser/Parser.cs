using Scheme.NET.Lexer;
using Scheme.NET.Numbers;
using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Parser
{
    public class SchemeParser
    {
        private static readonly Token LPAREN = new Token("(", TokenType.Parens);
        private static readonly Token RPAREN = new Token(")", TokenType.Parens);
        private static readonly Token PROTECT = new Token("'", TokenType.Regular);

        private static Func<Token> CreateNextTokenFunc(IEnumerable<Token> tokens)
        {
            var e = tokens.GetEnumerator();
            return () =>
            {
                if (e.MoveNext())
                    return e.Current;
                return null;
            };
        }

        public static IEnumerable<ISExpression> Parse(IEnumerable<Token> tokens)
        {
            return Parse(CreateNextTokenFunc(tokens));
        }

        public static IEnumerable<ISExpression> Parse(Func<Token> nextToken)
        {
            var sexprs = new List<ISExpression>();

            Token t = nextToken();
            do
            {
                sexprs.Add(ParseNext(t, nextToken));
                t = nextToken();
            } while (t != null);
            return sexprs;
        }

        private static ISExpression ParseNext(Token t, Func<Token> nextToken)
        {
            if (t == LPAREN)
                return ParseCons(nextToken);
            else if (t == RPAREN)
                ThrowError("unmatched )");
            else if (t == PROTECT)
            {
                var s = ParseNext(nextToken(), nextToken);
                return AtomHelper.CreateCons(
                    AtomHelper.SymbolFromString("quote"),
                    AtomHelper.CreateCons(s, AtomHelper.Nil));
            }
            return ParseAtom(t);
        }

        private static ISExpression ParseCons(Func<Token> nextToken)
        {
            var t = nextToken();
            if (t == null)
                ThrowError("invalid cons format");

            if (t == RPAREN)
                return AtomHelper.Nil;

            if (t.Value == ".")
            {
                t = nextToken();
                if (t == null)
                    ThrowError("expected cdr");

                var ret = ParseNext(t, nextToken);
                t = nextToken();
                if (t != RPAREN)
                    ThrowError("expected )");
                return ret;
            }

            var car = ParseNext(t, nextToken);
            var cdr = ParseCons(nextToken);
            return AtomHelper.CreateCons(car, cdr);
        }

        private static ISExpression ParseAtom(Token t)
        {
            if (IsString(t))
                return AtomHelper.StringFromString(t.Value);
            else if (IsBoolean(t))
                return AtomHelper.BooleanFromString(t.Value);
            else if (IsNumber(t))
                return AtomHelper.NumberFromString(t.Value);
            else if (IsChar(t))
            {
                return AtomHelper.CharFromString(t.Value);
            }
            return AtomHelper.SymbolFromString(t.Value);
        }

        private static bool IsNumber(Token t) {
            if (IsDigits(t.Value))
                return true;

            if (t.Value.StartsWith("#b"))
            {
                if (IsBinary(t.Value.Substring(2)))
                    return true;
                else
                    ThrowError($"Invalid characters in binary number: {t.Value}");
            }

            if (t.Value.StartsWith("#o"))
            {
                if (IsOctal(t.Value.Substring(2)))
                    return true;
                else
                    ThrowError($"Invalid characters in octal number: {t.Value}");
            }

            if (t.Value.StartsWith("#d"))
            {
                if (IsDigits(t.Value.Substring(2)))
                    return true;
                else
                    ThrowError($"Invalid characters in decimal number: {t.Value}");
            }

            if (t.Value.StartsWith("#x"))
            {
                if (IsHex(t.Value.Substring(2)))
                    return true;
                else
                    ThrowError($"Invalid characters in hex number: {t.Value}");
            }

            return false;
        }
        private static bool IsBinary(string s)
        {
            foreach (char c in s)
                if (c != '0' && c != '1')
                    return false;
            return true;
        }

        private static bool IsOctal(string s)
        {
            foreach (char c in s)
                if (c < '0' || c > '7')
                    return false;
            return true;
        }

        private static bool IsDigits(string s)
        {
            BigRational test;
            return BigRational.TryParse(s, out test);
        }

        private static bool IsHex(string s)
        {
            return s.All("0123456789abcdefABCDEF".Contains);
        }

        private static bool IsString(Token t) { return t.Type == TokenType.String; }
        private static bool IsBoolean(Token t) { return t.Value == "#t" || t.Value == "#f"; }
        private static bool IsChar(Token t)
        {
            return t.Value.StartsWith("#\\");
        }

        private static void ThrowError(string msg)
        {
            throw new InvalidOperationException($"Parser Error: {msg}");
        }
    }
}
