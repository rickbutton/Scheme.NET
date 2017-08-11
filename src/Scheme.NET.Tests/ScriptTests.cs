using NUnit.Framework;
using Scheme.NET.Eval;
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
            var tests = File.ReadAllText(Path.Combine(path, "tests.scm"));

            Env.GlobalScope.Define(AtomHelper.SymbolFromString("test"),
                AtomHelper.CreateProcedure("test", Test, true));

            var errors = "";
            var exprs = EvalAll(tests).ToArray();
            for (var i = 0; i < exprs.Count(); i++)
            {
                var result = exprs[i];
                if (result == null)
                    continue;

                if (result.IsString())
                {
                    errors += result.String() + "\n";
                }
            }
            if (errors != "")
                Assert.Fail(errors);
        }

        private ISExpression Test(Scope scope, IEnumerable<ISExpression> args)
        {
            var arr = args.ToArray();

            var actual = arr[1];
            var expected = arr[0];

            try
            {

                var actualResult = Evaluator.Eval(scope, actual);
                var expectedResult = Evaluator.Eval(scope, expected);

                if (actualResult.Equals(expectedResult))
                    return AtomHelper.True;

                return AtomHelper.StringFromString(
                    $"[FAIL] expected [ {expected.String()} ], but result of [ {actual.String()} ] was [ {actualResult.String()} ]");
            } catch (Exception e)
            {
                return AtomHelper.StringFromString(
                       $"[FAIL] expected [ {expected.String()} ], but result of [ {actual.String()} ] was exception [ {e.Message} ]");
            }
        }
    }
}
