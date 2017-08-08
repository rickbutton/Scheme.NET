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
            Assert.AreEqual(ab, a.IsExact);
            Assert.AreEqual(bb, b.IsExact);

            Complex.PromoteExactness(a, b, out a, out b);

            Assert.AreEqual(aa, a.IsExact);
            Assert.AreEqual(ba, b.IsExact);
        }

        [Test]
        public void TestPromoteE()
        {
            Complex a = Complex.FromInteger(1);
            Complex b = Complex.FromInteger(1);
            TestPromote(a, true, true, b, true, true);
        }

        [Test]
        public void TestPromoteI()
        {
            Complex a = Complex.FromDouble(1);
            Complex b = Complex.FromDouble(1);
            TestPromote(a, false, false, b, false, false);
        }

        [Test]
        public void TestPromoteEI()
        {
            Complex a = Complex.FromInteger(1);
            Complex b = Complex.FromDouble(1.0);

            TestPromote(a, true, false, b, false, false);
            TestPromote(b, false, false, a, true, false);
        }

        [Test]
        public void TestPromoteEILinq()
        {
            var ints = new int[] { 1, 1, 1, 1, 2, 1, 2, 1, 1, 1 };

            var complexes = ints
                .Select(i => i == 1 ? 
                    (Complex)Complex.FromInteger(i) : 
                    (Complex)Complex.FromDouble(i)).ToList();

            Assert.AreEqual(2, complexes.Count(c => !c.IsExact));
            complexes = complexes.Select(c => c.PromoteRelative(complexes)).ToList();
            Assert.AreEqual(10, complexes.Count(c => !c.IsExact));

        }

    }
}
