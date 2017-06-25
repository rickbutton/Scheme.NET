using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Lexer
{
    public class LexerState
    {
        public Func<LexerState> Step { get; private set; }
        public LexerState(Func<LexerState> step) { this.Step = step; }
    }

    public class SchemeLexer
    {
        private LexerState _state;
        private List<Token> _tokens;
        private Func<char?> _nextRune;

        private char? _save;
        private List<char> _temp;
        private List<char> _escape;

        private static readonly string PARENS = "()";
        private static readonly string WS = "\t\r\n ";
        private static readonly string SPLIT = PARENS + WS + ";";
        private static readonly string PROTECT = "'";

        private static Func<char?> CreateNextRuneFunc(string input)
        {
            var e = input.GetEnumerator();
            return () =>
            {
                if (e.MoveNext())
                {
                    return e.Current;
                }
                return null;
            };
        }

        public SchemeLexer()
        {
        }

        public List<Token> Lex(string input)
        {
            return Lex(CreateNextRuneFunc(input));
        }

        public List<Token> Lex(Func<char?> nextRune)
        {
            _state = new LexerState(ReadyState);
            _tokens = new List<Token>();
            _temp = new List<char>();
            _escape = new List<char>();
            _nextRune = nextRune;

            while (_state != null)
            {
                _state = _state.Step();
            }
            return _tokens;
        }

        private LexerState ReadyState()
        {
            char? c;
            if (_save.HasValue)
            {
                c = _save;
                _save = null;
            }
            else
            {
                c = _nextRune();
            }

            if (c == null)
                return null;

            if (PARENS.Contains(c.Value))
            {
                EmitParens(c.ToString());
            }
            else if (WS.Contains(c.Value))
            {
                // do nothing, skip
            }
            else if (c == ';')
            {
                _temp.Add(c.Value);
                return new LexerState(CommentState);
            }
            else if (c == '"')
            {
                return new LexerState(StringState);
            }
            else if (PROTECT.Contains(c.Value))
            {
                EmitSymbol(c.ToString());
            }
            else
            {
                _temp.Add(c.Value);
                return new LexerState(ReadingState);
            }
            return new LexerState(ReadyState);
        }

        private LexerState CommentState()
        {
            var c = _nextRune();

            if (c == '\n')
                return new LexerState(ReadyState);
            return new LexerState(CommentState);
        }

        private LexerState ReadingState()
        {
            var c = _nextRune();

            if (c == null)
            {
                EmitSymbol(string.Join("", _temp));
                _temp.Clear();
                return null;
            }
            else if (SPLIT.Contains(c.Value))
            {
                EmitSymbol(string.Join("", _temp));
                _temp.Clear();
                _save = c;
                return new LexerState(ReadyState);
            } else
            {
                _temp.Add(c.Value);
            }

            return new LexerState(ReadingState);
        }

        private LexerState StringState()
        {
            var c = _nextRune();
            if (c == null)
                ThrowError("Unterminated string token parsed. (Found one \", but not the ending \")");

            if (c.Value == '\\')
            {
                return new LexerState(EscapeState);
            }
            else
            {
                if (c.Value == '"')
                {
                    EmitString(string.Join("", _temp));
                    _temp.Clear();
                    return new LexerState(ReadyState);
                }
                else
                {
                    _temp.Add(c.Value);
                    return new LexerState(StringState);
                }
            }
        }

        private LexerState EscapeState()
        {
            var c = _nextRune();
            if (c == null)
                ThrowError("Unterminated escape token parsed");

            switch (c.Value)
            {
                case 'a': _temp.Add('\a'); break;
                case 'b': _temp.Add('\b'); break;
                case 't': _temp.Add('\t'); break;
                case 'n': _temp.Add('\n'); break;
                case 'v': _temp.Add('\v'); break;
                case 'f': _temp.Add('\f'); break;
                case 'r': _temp.Add('\r'); break;
                case '"': _temp.Add('"'); break;
                case '\\': _temp.Add('\\'); break;
                default: ThrowError($"Unknown escape code: {c.Value}"); break;
            }
            return new LexerState(StringState);
        }

        private void EmitSymbol(string token) { EmitToken(token, TokenType.Regular); }
        private void EmitParens(string token) { EmitToken(token, TokenType.Parens); }
        private void EmitString(string token) { EmitToken(token, TokenType.String); }
        private void EmitToken(string token, TokenType type)
        {
            _tokens.Add(new Token(token, type));
        }

        private void ThrowError(string err)
        {
            throw new InvalidOperationException($"Lexer Error: {err}");
        }
    }
}
