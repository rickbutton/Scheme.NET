using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Lexer
{
    public class Token : IEquatable<Token>
    {
        public string Value { get; private set; }
        public TokenType Type { get; private set; }

        public Token(string value, TokenType type)
        {
            this.Value = value;
            this.Type = type;
        }

        public bool Equals(Token other)
        {
            return Value == other.Value && Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var t = obj as Token;

            if (t == null) return false;

            return Equals(t);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ Type.GetHashCode();
        }

        public static bool operator ==(Token a, Token b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if ((object)a == null || (object)b == null) return false;
            return a.Value == b.Value && a.Type == b.Type;
        }

        public static bool operator !=(Token a, Token b)
        {
            return !(a == b);
        }
    }
}
