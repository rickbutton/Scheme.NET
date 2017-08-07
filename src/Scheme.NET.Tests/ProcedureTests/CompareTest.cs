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
    class CompareTest : TestBase
    {
        [Test]
        public void TestQuotient()
        {
            Assert.AreEqual(AtomHelper.NumberFromComplex(5), Eval("(quotient 5 1)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(3), Eval("(quotient 13 4)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-4), Eval("(quotient -13 4)"));
        }

        [Test]
        public void TestRemainder()
        {
            Assert.AreEqual(AtomHelper.NumberFromComplex(1), Eval("(remainder 13 4)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-1), Eval("(remainder -13 4)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(1), Eval("(remainder 13 -4)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-1), Eval("(remainder -13 -4)"));
        }

        [Test]
        public void TestModulo()
        {
            Assert.AreEqual(AtomHelper.NumberFromComplex(1), Eval("(modulo 13 4)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(3), Eval("(modulo -13 4)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-3), Eval("(modulo 13 -4)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-1), Eval("(modulo -13 -4)"));
        }

        [Test]
        public void TestGcd()
        {
            Assert.AreEqual(AtomHelper.NumberFromComplex(4), Eval("(gcd 32 -36 32)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(32), Eval("(gcd 32)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(0), Eval("(gcd)"));
        }

        [Test]
        public void TestLcm()
        {
            Assert.AreEqual(AtomHelper.NumberFromComplex(288), Eval("(lcm 32 -36)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(32), Eval("(lcm 32)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(1), Eval("(lcm)"));
        }

        [Test]
        public void TestFloor()
        {
            Assert.AreEqual(AtomHelper.NumberFromComplex(1), Eval("(floor 1.5)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(1), Eval("(floor 1)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(2000), Eval("(floor 2000.00001)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-1), Eval("(floor -0.9)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-100), Eval("(floor -99.5)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-100), Eval("(floor -100)"));
        }

        [Test]
        public void TestCeiling()
        {
            Assert.AreEqual(AtomHelper.NumberFromComplex(2), Eval("(ceiling 1.5)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(1), Eval("(ceiling 1)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(2001), Eval("(ceiling 2000.00001)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(0), Eval("(ceiling -0.9)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-99), Eval("(ceiling -99.5)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-100), Eval("(ceiling -100)"));
        }

        [Test]
        public void TestTruncate()
        {
            Assert.AreEqual(AtomHelper.NumberFromComplex(1), Eval("(truncate 1.5)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(1), Eval("(truncate 1)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(2000), Eval("(truncate 2000.00001)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(0), Eval("(truncate -0.9)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-99), Eval("(truncate -99.5)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-100), Eval("(truncate -100)"));
        }

        [Test]
        public void TestRound()
        {
            Assert.AreEqual(AtomHelper.NumberFromComplex(2), Eval("(round 1.5)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(1), Eval("(round 1)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(2000), Eval("(round 2000.00001)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-1), Eval("(round -0.9)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-100), Eval("(round -99.5)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-100), Eval("(round -100)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(4), Eval("(round 4.4)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(4), Eval("(round 4.5)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(5), Eval("(round 4.6)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-4), Eval("(round -4.4)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-4), Eval("(round -4.5)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-5), Eval("(round -4.6)"));
        }

        [Test]
        public void TestMinMax()
        {
            Assert.AreEqual(AtomHelper.NumberFromComplex(3), Eval("(max 0 1 2 3)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(3), Eval("(max -1 1 2 3)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(0), Eval("(min 0 1 2 3)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(-1), Eval("(min -1 1 2 3)"));
        }

        [Test]
        public void TestAbs()
        {
            Assert.AreEqual(AtomHelper.NumberFromComplex(3), Eval("(abs 3)"));
            Assert.AreEqual(AtomHelper.NumberFromComplex(3), Eval("(abs -3)"));
        }

        [Test]
        public void TestEquals()
        {
            Assert.AreEqual(AtomHelper.True, Eval("(= 25 25.0 #b11001 #o31 #d25 #x19)"));
            Assert.AreEqual(AtomHelper.True, Eval("(= 25.2 25.20)"));
            Assert.AreEqual(AtomHelper.False, Eval("(= 25.0 25.20)"));
        }

        [Test]
        public void TestIncreasing()
        {
            Assert.AreEqual(AtomHelper.True, Eval("(< 1 2 3 4 5 6 6.1)"));
            Assert.AreEqual(AtomHelper.True, Eval("(< 1.1 1.11 1.111)"));
            Assert.AreEqual(AtomHelper.False, Eval("(< 1 2 2)"));
        }

        [Test]
        public void TestDecreasing()
        {
            Assert.AreEqual(AtomHelper.True, Eval("(> 6 5 4 3 2 1)"));
            Assert.AreEqual(AtomHelper.True, Eval("(> 1.111 1.11 1.1 1.0)"));
            Assert.AreEqual(AtomHelper.False, Eval("(> 2 2 1)"));
        }

        [Test]
        public void TestNonIncreasing()
        {
            Assert.AreEqual(AtomHelper.True, Eval("(>= 6 6 5 5 4 4 3 3 2 2)"));
            Assert.AreEqual(AtomHelper.True, Eval("(>= 1.111 1.111 1.1 1.1)"));
            Assert.AreEqual(AtomHelper.False, Eval("(>= 6 6 5 5 4 4 5)"));
        }

        [Test]
        public void TestNonDecreasing()
        {
            Assert.AreEqual(AtomHelper.True, Eval("(<= 1 1 2 2 3 3 4 4)"));
            Assert.AreEqual(AtomHelper.True, Eval("(<= 1.1 1.11 1.11 1.111 1.111)"));
            Assert.AreEqual(AtomHelper.False, Eval("(<= 1 1 2 2 3 3 2 2)"));
        }
    }
}
