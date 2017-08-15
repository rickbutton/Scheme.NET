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
            var tests = File.ReadAllText(Path.Combine(path, "tests.scm"));

            var def = AtomHelper.CreateList(
                            AtomHelper.SymbolFromString("define"),
                AtomHelper.SymbolFromString("test"),
                AtomHelper.CreateList(AtomHelper.SymbolFromString("lambda"), AtomHelper.Nil, AtomHelper.CreateProcedure("test", Test, false))
                );
            Eval(def);

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
                if (actual.Equals(expected))
                    return AtomHelper.True;

                return AtomHelper.StringFromString(
                    $"[FAIL] expected [ {expected.String()} ], but result of [ {actual.String()} ] was [ {actual.String()} ]");
            } catch (Exception e)
            {
                return AtomHelper.StringFromString(
                       $"[FAIL] expected [ {expected.String()} ], but result of [ {actual.String()} ] was exception [ {e.Message} ]");
            }
        }
    }
}
