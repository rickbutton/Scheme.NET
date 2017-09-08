using NUnit.Framework;
using Scheme.NET.Parser;
using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine;
using Scheme.NET.VirtualMachine.Compiler;
using Scheme.NET.VirtualMachine.Natives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Tests.ProcedureTests
{
    public abstract class TestBase
    {
        protected ISchemeVM VM { get; private set; }

        [SetUp]
        public void SetUp()
        {
            VM = new SchemeVM();
        }

        protected ISExpression Eval(ISExpression e)
        {
            var a = SchemeCompiler.Compile(VM, e);
            return VM.Execute(a);
        }

        protected ISExpression Eval(string input)
        {
            var arr = ParserHelpers.Parse(input);
            var carr = arr.Select(a => SchemeCompiler.Compile(VM, a));
            var s = carr.Select(c => VM.Execute(c));

            if (s.Count() > 0)
                return s.First();
            else
                return null;
        }

        protected IEnumerable<ISExpression> EvalAll(string input)
        {
            var arr = ParserHelpers.Parse(input);
            var carr = arr.Select(a => SchemeCompiler.Compile(VM, a));
            var s = carr.Select(c => VM.Execute(c));
            return s;
        }
    }
}
