using NUnit.Framework;
using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Tests.ProcedureTests
{
    [TestFixture]
    class PredicateTest : TestBase
    {
        [Test] public void TestBoolean() { TestPredicate("boolean"); }
        [Test] public void TestPair() { TestPredicate("pair"); }
        [Test] public void TestSymbol() { TestPredicate("symbol"); }
        [Test] public void TestChar() { TestPredicate("char"); }
        [Test] public void TestString() { TestPredicate("string"); }
        [Test] public void TestProcedure() { TestPredicate("procedure"); }
        [Test] public void TestNull() { TestPredicate("null"); }

        [Test]
        public void TestNumber()
        {
            TestPredicate("number");
            Assert.AreEqual(AtomHelper.True, Eval("(integer? 3)"));
            Assert.AreEqual(AtomHelper.True, Eval("(integer? 3.0)"));
            Assert.AreEqual(AtomHelper.False, Eval("(integer? 3/5)"));
            Assert.AreEqual(AtomHelper.False, Eval("(integer? 3.5)"));
            Assert.AreEqual(AtomHelper.False, Eval("(integer? 3+3i)"));

            Assert.AreEqual(AtomHelper.True, Eval("(rational? 3)"));
            Assert.AreEqual(AtomHelper.True, Eval("(rational? 3.0)"));
            Assert.AreEqual(AtomHelper.True, Eval("(rational? 3/5)"));
            Assert.AreEqual(AtomHelper.True, Eval("(rational? 3.5)"));
            Assert.AreEqual(AtomHelper.False, Eval("(real? 3+3i)"));

            Assert.AreEqual(AtomHelper.True, Eval("(real? 3)"));
            Assert.AreEqual(AtomHelper.True, Eval("(real? 3.0)"));
            Assert.AreEqual(AtomHelper.True, Eval("(real? 3/5)"));
            Assert.AreEqual(AtomHelper.True, Eval("(real? 3.5)"));
            Assert.AreEqual(AtomHelper.False, Eval("(real? 3+3i)"));

            Assert.AreEqual(AtomHelper.True, Eval("(complex? 3)"));
            Assert.AreEqual(AtomHelper.True, Eval("(complex? 3.0)"));
            Assert.AreEqual(AtomHelper.True, Eval("(complex? 3/5)"));
            Assert.AreEqual(AtomHelper.True, Eval("(complex? 3.5)"));
            Assert.AreEqual(AtomHelper.True, Eval("(complex? 3+3i)"));

            Assert.AreEqual(AtomHelper.True, Eval("(zero? 0.0)"));
            Assert.AreEqual(AtomHelper.True, Eval("(zero? 0)"));
            Assert.AreEqual(AtomHelper.True, Eval("(zero? 0/10)"));
            Assert.AreEqual(AtomHelper.True, Eval("(zero? 0+0i)"));
            Assert.AreEqual(AtomHelper.False, Eval("(zero? 1)"));
            Assert.AreEqual(AtomHelper.False, Eval("(zero? -1)"));

            Assert.AreEqual(AtomHelper.False, Eval("(positive? 0)"));
            Assert.AreEqual(AtomHelper.True, Eval("(positive? 1)"));
            Assert.AreEqual(AtomHelper.False, Eval("(positive? -1)"));
            Assert.AreEqual(AtomHelper.True, Eval("(positive? 1.1)"));
            Assert.AreEqual(AtomHelper.False, Eval("(positive? -1.1)"));
            Assert.AreEqual(AtomHelper.False, Eval("(negative? 0)"));
            Assert.AreEqual(AtomHelper.False, Eval("(negative? 1)"));
            Assert.AreEqual(AtomHelper.True, Eval("(negative? -1)"));
            Assert.AreEqual(AtomHelper.False, Eval("(negative? 1.1)"));
            Assert.AreEqual(AtomHelper.True, Eval("(negative? -1.1)"));

            Assert.AreEqual(AtomHelper.True, Eval("(odd? 1)"));
            //Assert.AreEqual(AtomHelper.False, Eval("(odd? 1.1)"));
            Assert.AreEqual(AtomHelper.False, Eval("(odd? 2)"));
            //Assert.AreEqual(AtomHelper.False, Eval("(odd? 2.1)"));
            Assert.AreEqual(AtomHelper.False, Eval("(odd? 0)"));

            Assert.AreEqual(AtomHelper.True, Eval("(even? 2)"));
            //Assert.AreEqual(AtomHelper.False, Eval("(even? 1.1)"));
            Assert.AreEqual(AtomHelper.False, Eval("(even? 1)"));
            //Assert.AreEqual(AtomHelper.False, Eval("(even? 2.1)"));
            Assert.AreEqual(AtomHelper.True, Eval("(even? 0)"));
        }

        private void TestPredicate(string name)
        {
            Assert.AreEqual(B2B(name == "boolean"), Eval($"({name}? #f)"));
            Assert.AreEqual(B2B(name == "pair"), Eval($"({name}? (1 . 2))"));
            Assert.AreEqual(B2B(name == "symbol"), Eval($"({name}? 'a)"));
            Assert.AreEqual(B2B(name == "number"),  Eval($"({name}? 3)"));
            Assert.AreEqual(B2B(name == "char"), Eval($"({name}? #\\a)"));
            Assert.AreEqual(B2B(name == "string"), Eval($"({name}? \"test\")"));
            Assert.AreEqual(B2B(name == "procedure"), Eval($"({name}? +)"));
            Assert.AreEqual(B2B(name == "null"), Eval($"({name}? ())"));
        }

        private BooleanAtom B2B(bool b) { return b ? AtomHelper.True : AtomHelper.False; }
    }
}
