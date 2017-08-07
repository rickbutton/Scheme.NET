using NUnit.Framework;
using Scheme.NET.Numbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Tests.NumberTests
{
    [TestFixture]
    class NumberTests
    {
        private void TestPromote(Complex a, bool ab, bool aa, Complex b, bool bb, bool ba)
        {
            Assert.AreEqual(ab, a.IsExact());
            Assert.AreEqual(bb, b.IsExact());
            
            a = a.PromoteRelative(b);
            Assert.AreEqual(aa, a.IsExact());

            b = b.PromoteRelative(a);
            Assert.AreEqual(ba, b.IsExact());
        }

        [Test]
        public void TestPromoteE()
        {
            Complex a = NumberTower.ExactInteger(1);
            Complex b = NumberTower.ExactInteger(1);
            TestPromote(a, true, true, b, true, true);
        }

        [Test]
        public void TestPromoteI()
        {
            Complex a = NumberTower.InexactRational(1);
            Complex b = NumberTower.InexactRational(1);
            TestPromote(a, false, false, b, false, false);
        }

        [Test]
        public void TestPromoteEI()
        {
            Complex a = NumberTower.ExactInteger(1);
            Complex b = NumberTower.InexactRational(1.0);

            TestPromote(a, true, false, b, false, false);
            TestPromote(b, false, false, a, true, false);
        }

        [Test]
        public void TestPromoteEILinq()
        {
            var ints = new int[] { 1, 1, 1, 1, 2, 1, 2, 1, 1, 1 };

            var complexes = ints
                .Select(i => i == 1 ? 
                    (Complex)NumberTower.ExactInteger(i) : 
                    (Complex)NumberTower.InexactRational(i)).ToList();

            Assert.AreEqual(2, complexes.Count(c => !c.IsExact()));
            complexes = complexes.Select(c => c.PromoteRelative(complexes)).ToList();
            Assert.AreEqual(10, complexes.Count(c => !c.IsExact()));

        }

    }
}
