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
    public class EqTest : TestBase
    {
        [Test]
        public void TestBool()
        {
            Assert.AreEqual(AtomHelper.True, Eval("(eq? #t #t)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eq? #f #f)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eq? #f #t)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eq? #t #f)"));
        }

        [Test]
        public void TestSymbol()
        {
            Assert.AreEqual(AtomHelper.True, Eval("(eq? 'a 'a)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eq? 'A 'a)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eq? 'a 'b)"));
        }

        [Test]
        public void TestNumber()
        {
            Assert.AreEqual(AtomHelper.True, Eval("(eq? 1 1)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eq? 0 0)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eq? 1 2)"));

            Assert.AreEqual(AtomHelper.True, Eval("(eq? 1.1 1.1)"));

            Assert.AreEqual(AtomHelper.False, Eval("(eq? 1.1 1.2)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eq? 1.1 2.1)"));

            Assert.AreEqual(AtomHelper.False, Eval("(eq? 1.1 1)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eq? 1.0 1)"));
        }

        [Test]
        public void TestChar()
        {
            Assert.AreEqual(AtomHelper.True, Eval("(eq? #\\a #\\a)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eq? #\\space #\\space)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eq? #\\newline #\\newline)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eq? #\\a #\\z)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eq? #\\a #\\space)"));
        }

        [Test]
        public void TestNil()
        {
            Assert.AreEqual(AtomHelper.True, Eval("(eq? () ())"));
            Assert.AreEqual(AtomHelper.False, Eval("(eq? '(1 2) ())"));
        }

        [Test]
        public void TestLocs()
        {
            VM.E.DefineHere(AtomHelper.SymbolFromString("a"), Eval("(1 . 2)"));
            VM.E.DefineHere(AtomHelper.SymbolFromString("b"), Eval("\"test\""));
            VM.E.DefineHere(AtomHelper.SymbolFromString("c"), Eval("#(1 2)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eq? a a)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eq? b b)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eq? c c)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eq? + +)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eq? (1 . 2) (1 . 2))"));
        }
    }
}
