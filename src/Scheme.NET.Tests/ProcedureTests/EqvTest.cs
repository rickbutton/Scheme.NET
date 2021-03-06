﻿using NUnit.Framework;
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

            Assert.AreEqual(AtomHelper.True, Eval("(eqv? 1.1 1.1)"));

            Assert.AreEqual(AtomHelper.False, Eval("(eqv? 1.1 1.2)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eqv? 1.1 2.1)"));

            Assert.AreEqual(AtomHelper.False, Eval("(eqv? 1.1 1)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eqv? 1.0 1)"));
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
            VM.E.DefineHere(AtomHelper.SymbolFromString("a"), Eval("(1 . 2)"));
            VM.E.DefineHere(AtomHelper.SymbolFromString("b"), Eval("\"test\""));
            VM.E.DefineHere(AtomHelper.SymbolFromString("c"), Eval("#(1 2)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? a a)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? b b)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? c c)"));
            Assert.AreEqual(AtomHelper.True, Eval("(eqv? + +)"));
            Assert.AreEqual(AtomHelper.False, Eval("(eqv? (1 . 2) (1 . 2))"));
        }
    }
}
