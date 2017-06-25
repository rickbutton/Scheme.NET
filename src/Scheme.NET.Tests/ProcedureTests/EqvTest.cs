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
    [TestFixture]
    public class EqvTest : TestBase
    {
        [Test]
        public void TestBool()
        {
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? #t #t)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? #f #f)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eqv? #f #t)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eqv? #t #f)"));
        }

        [Test]
        public void TestSymbol()
        {
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? 'a 'a)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? 'A 'a)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eqv? 'a 'b)"));
        }

        [Test]
        public void TestNumber()
        {
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? 1 1)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? 0 0)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eqv? 1 2)"));
        }

        [Test]
        public void TestChar()
        {
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? #\\a #\\a)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? #\\space #\\space)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? #\\newline #\\newline)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eqv? #\\a #\\z)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eqv? #\\a #\\space)"));
        }

        [Test]
        public void TestNil()
        {
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? () ())"));
            Assert.AreEqual(AtomHelper.False, Eval("(eqv? '(1 2) ())"));
        }

        [Test]
        public void TestLocs()
        {
            Evaluator.GlobalScope.Define(AtomHelper.SymbolFromString("a"), Eval("(1 . 2)"));
            Evaluator.GlobalScope.Define(AtomHelper.SymbolFromString("b"), Eval("\"test\""));
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? a a)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? b b)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? + +)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eqv? (1 . 2) (1 . 2))"));
        }
    }
}
