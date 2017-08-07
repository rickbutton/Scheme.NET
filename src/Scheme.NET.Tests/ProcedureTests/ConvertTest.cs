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
    class ConvertTest : TestBase
    {
        [Test]
        public void TestNumberToString()
        {
            // radix 2,8,10,16
            // integer, rational, real, complex

            // radix 2, integer
            Assert.AreEqual(AtomHelper.StringFromString("1100100"), Eval("(number->string 100 2)"));
            Assert.AreEqual(AtomHelper.StringFromString("-1100100"), Eval("(number->string -100 2)"));
            

            // radix 8, integer
            Assert.AreEqual(AtomHelper.StringFromString("144"), Eval("(number->string 100 8)"));
            Assert.AreEqual(AtomHelper.StringFromString("-144"), Eval("(number->string -100 8)"));

            // radix 10, integer
            Assert.AreEqual(AtomHelper.StringFromString("100"), Eval("(number->string 100)"));
            Assert.AreEqual(AtomHelper.StringFromString("100"), Eval("(number->string 100 10)"));
            Assert.AreEqual(AtomHelper.StringFromString("-100"), Eval("(number->string -100)"));
            Assert.AreEqual(AtomHelper.StringFromString("-100"), Eval("(number->string -100 10)"));

            // radix 16, integer
            Assert.AreEqual(AtomHelper.StringFromString("64"), Eval("(number->string 100 16)"));
            Assert.AreEqual(AtomHelper.StringFromString("-64"), Eval("(number->string -100 16)"));

            // radix 2, rational
            Assert.AreEqual(AtomHelper.StringFromString("1101101/1000"), Eval("(number->string 109/8 2)"));
            Assert.AreEqual(AtomHelper.StringFromString("-1101101/1000"), Eval("(number->string -109/8 2)"));

            // radix 8, rational
            Assert.AreEqual(AtomHelper.StringFromString("155/10"), Eval("(number->string 109/8 8)"));
            Assert.AreEqual(AtomHelper.StringFromString("-155/10"), Eval("(number->string -109/8 8)"));

            // radix 10, rational
            Assert.AreEqual(AtomHelper.StringFromString("109/8"), Eval("(number->string 109/8 10)"));
            Assert.AreEqual(AtomHelper.StringFromString("-109/8"), Eval("(number->string -109/8 10)"));

            // radix 16, rational
            Assert.AreEqual(AtomHelper.StringFromString("6d/8"), Eval("(number->string 109/8 16)"));
            Assert.AreEqual(AtomHelper.StringFromString("-6d/8"), Eval("(number->string -109/8 16)"));

            // radix 2, reals
            Assert.AreEqual(AtomHelper.StringFromString("1101.101"), Eval("(number->string 13.625 2)"));
            Assert.AreEqual(AtomHelper.StringFromString("-1101.101"), Eval("(number->string -13.625 2)"));

            // radix 8, reals
            Assert.AreEqual(AtomHelper.StringFromString("15.5"), Eval("(number->string 13.625 8)"));
            Assert.AreEqual(AtomHelper.StringFromString("-15.5"), Eval("(number->string -13.625 8)"));

            // radix 10, reals
            Assert.AreEqual(AtomHelper.StringFromString("13.625"), Eval("(number->string 13.625 10)"));
            Assert.AreEqual(AtomHelper.StringFromString("-13.625"), Eval("(number->string -13.625 10)"));

            // radix 16, reals
            Assert.AreEqual(AtomHelper.StringFromString("d.a"), Eval("(number->string 13.625 16)"));
            Assert.AreEqual(AtomHelper.StringFromString("-d.a"), Eval("(number->string -13.625 16)"));

            // radix 2, complex, rationals
            Assert.AreEqual(AtomHelper.StringFromString("101/11+101/11i"), Eval("(number->string 5/3+5/3i 2)"));
            Assert.AreEqual(AtomHelper.StringFromString("101/11-101/11i"), Eval("(number->string 5/3-5/3i 2)"));
            Assert.AreEqual(AtomHelper.StringFromString("-101/11+101/11i"), Eval("(number->string -5/3+5/3i 2)"));
            Assert.AreEqual(AtomHelper.StringFromString("-101/11-101/11i"), Eval("(number->string -5/3-5/3i 2)"));

            // radix 8, complex, rationals
            Assert.AreEqual(AtomHelper.StringFromString("5/3+5/3i"), Eval("(number->string 5/3+5/3i 8)"));
            Assert.AreEqual(AtomHelper.StringFromString("5/3-5/3i"), Eval("(number->string 5/3-5/3i 8)"));
            Assert.AreEqual(AtomHelper.StringFromString("-5/3+5/3i"), Eval("(number->string -5/3+5/3i 8)"));
            Assert.AreEqual(AtomHelper.StringFromString("-5/3-5/3i"), Eval("(number->string -5/3-5/3i 8)"));

            // radix 10, complex, rationals
            Assert.AreEqual(AtomHelper.StringFromString("5/3+5/3i"), Eval("(number->string 5/3+5/3i 10)"));
            Assert.AreEqual(AtomHelper.StringFromString("5/3-5/3i"), Eval("(number->string 5/3-5/3i 10)"));
            Assert.AreEqual(AtomHelper.StringFromString("-5/3+5/3i"), Eval("(number->string -5/3+5/3i 10)"));
            Assert.AreEqual(AtomHelper.StringFromString("-5/3-5/3i"), Eval("(number->string -5/3-5/3i 10)"));

            // radix 16, complex, rationals
            Assert.AreEqual(AtomHelper.StringFromString("5/3+5/3i"), Eval("(number->string 5/3+5/3i 16)"));
            Assert.AreEqual(AtomHelper.StringFromString("5/3-5/3i"), Eval("(number->string 5/3-5/3i 16)"));
            Assert.AreEqual(AtomHelper.StringFromString("-5/3+5/3i"), Eval("(number->string -5/3+5/3i 16)"));
            Assert.AreEqual(AtomHelper.StringFromString("-5/3-5/3i"), Eval("(number->string -5/3-5/3i 16)"));

            // radix 2, complex, reals
            Assert.AreEqual(AtomHelper.StringFromString("1101.101+1101.101i"), Eval("(number->string 13.625+13.625i 2)"));
            Assert.AreEqual(AtomHelper.StringFromString("1101.101-1101.101i"), Eval("(number->string 13.625-13.625i 2)"));
            Assert.AreEqual(AtomHelper.StringFromString("-1101.101+1101.101i"), Eval("(number->string -13.625+13.625i 2)"));
            Assert.AreEqual(AtomHelper.StringFromString("-1101.101-1101.101i"), Eval("(number->string -13.625-13.625i 2)"));

            // radix 8, complex, reals
            Assert.AreEqual(AtomHelper.StringFromString("15.5+15.5i"), Eval("(number->string 13.625+13.625i 8)"));
            Assert.AreEqual(AtomHelper.StringFromString("15.5-15.5i"), Eval("(number->string 13.625-13.625i 8)"));
            Assert.AreEqual(AtomHelper.StringFromString("-15.5+15.5i"), Eval("(number->string -13.625+13.625i 8)"));
            Assert.AreEqual(AtomHelper.StringFromString("-15.5-15.5i"), Eval("(number->string -13.625-13.625i 8)"));

            // radix 10, complex, reals
            Assert.AreEqual(AtomHelper.StringFromString("13.625+13.625i"), Eval("(number->string 13.625+13.625i 10)"));
            Assert.AreEqual(AtomHelper.StringFromString("13.625-13.625i"), Eval("(number->string 13.625-13.625i 10)"));
            Assert.AreEqual(AtomHelper.StringFromString("-13.625+13.625i"), Eval("(number->string -13.625+13.625i 10)"));
            Assert.AreEqual(AtomHelper.StringFromString("-13.625-13.625i"), Eval("(number->string -13.625-13.625i 10)"));

            // radix 16, complex, reals
            Assert.AreEqual(AtomHelper.StringFromString("d.a+d.ai"), Eval("(number->string 13.625+13.625i 16)"));
            Assert.AreEqual(AtomHelper.StringFromString("d.a-d.ai"), Eval("(number->string 13.625-13.625i 16)"));
            Assert.AreEqual(AtomHelper.StringFromString("-d.a+d.ai"), Eval("(number->string -13.625+13.625i 16)"));
            Assert.AreEqual(AtomHelper.StringFromString("-d.a-d.ai"), Eval("(number->string -13.625-13.625i 16)"));
        }
    }
}
