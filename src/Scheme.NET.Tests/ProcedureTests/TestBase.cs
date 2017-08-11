using NUnit.Framework;
using Scheme.NET.Eval;
using Scheme.NET.Lexer;
using Scheme.NET.Lib;
using Scheme.NET.Parser;
using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Tests.ProcedureTests
{
    public abstract class TestBase
    {
        protected SchemeLexer Lexer;
        protected Evaluator Evaluator;

        [SetUp]
        public void SetUp()
        {
            Lexer = new SchemeLexer();
            Evaluator = new Evaluator(Library.CreateBase());
        }

        protected ISExpression Eval(string input)
        {
            var toks = Lexer.Lex(input);
            var s = SchemeParser.Parse(toks);
            s = s.Select(ss => Evaluator.Eval(ss, Evaluator.GlobalScope));
            if (s.Count() > 0)
                return s.First();
            else
                return null;
        }

        protected IEnumerable<ISExpression> EvalAll(string input)
        {
            var toks = Lexer.Lex(input);
            var s = SchemeParser.Parse(toks);
            return s.Select(ss => Evaluator.Eval(ss, Evaluator.GlobalScope));
        }
    }
}
