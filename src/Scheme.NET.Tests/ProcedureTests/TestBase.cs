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
        private SchemeLexer lexer;
        protected Evaluator Evaluator;

        [SetUp]
        protected void SetUp()
        {
            lexer = new SchemeLexer();
            Evaluator = new Evaluator(Library.CreateBase());
        }

        protected ISExpression Eval(string input)
        {
            var toks = lexer.Lex(input);
            var ss = SchemeParser.Parse(toks);
            ss = ss.Select(Evaluator.Eval);
            return ss.First();
        }
    }
}
