using NUnit.Framework;
using Scheme.NET.Eval;
using Scheme.NET.Lib;
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
        protected Environment Env;

        [SetUp]
        public void SetUp()
        {
            Env = Environment.Create();
        }

        protected ISExpression Eval(string input)
        {
            var s = Env.Eval(input);
            if (s.Count() > 0)
                return s.First();
            else
                return null;
        }

        protected IEnumerable<ISExpression> EvalAll(string input)
        {
            return Env.Eval(input);
        }
    }
}
