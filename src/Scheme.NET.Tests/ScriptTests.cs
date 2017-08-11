using NUnit.Framework;
using Scheme.NET.Scheme;
using Scheme.NET.Tests.ProcedureTests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Tests
{

    [TestFixture]
    public class ScriptTests : TestBase
    {

        [Test]
        public void TestScripts()
        {
            var path = Path.GetDirectoryName(typeof(ScriptTests).GetTypeInfo().Assembly.Location);
            var tests = File.ReadAllLines(Path.Combine(path, "tests.scm"));

            Evaluator.GlobalScope.Define(AtomHelper.SymbolFromString("test"),
                AtomHelper.CreateProcedure("test", Test, true));

            var errors = "";
            for (var i = 0; i < tests.Length; i++)
            {
                if (string.IsNullOrEmpty(tests[i]))
                    continue;

                var result = Eval(tests[i] + "\n");
                if (result == null)
                    continue;

                if (result != AtomHelper.True)
                {
                    errors += $"{(i + 1).ToString().PadLeft(3, '0')} : {result.String()}\n";
                }
            }
            if (errors != "")
                Assert.Fail(errors);
        }

        private ISExpression Test(Scope scope, IEnumerable<ISExpression> args)
        {
            var arr = args.ToArray();

            var actual = arr[0];
            var expected = arr[1];

            var actualResult = Evaluator.Eval(actual, scope);
            var expectedResult = Evaluator.Eval(expected, scope);

            if (actualResult.Equals(expectedResult))
                return AtomHelper.True;

            return AtomHelper.StringFromString(
                $"[FAIL] expected [ {expected.String()} ], but result of [ {actual.String()} ] was [ {actualResult.String()} ]");
        }
    }
}
