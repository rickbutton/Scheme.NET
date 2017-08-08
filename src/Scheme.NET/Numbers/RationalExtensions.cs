using Microsoft.SolverFoundation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Numbers
{
    public static class RationalExtensions
    {
        public static bool IsInteger(this Rational r)
        {
            return r.Denominator == 1;
        }

        public static bool IsEven(this Rational r)
        {
            if (r == 0) return false;
            if (!r.IsInteger()) return false;

            return r.Numerator % 2 == 0;
        }

        public static bool IsOdd(this Rational r)
        {
            if (r == 0) return false;
            if (!r.IsInteger()) return false;

            return r.Numerator % 2 != 0;
        }

        public static Rational Round(this Rational r)
        {
            if (r.GetFractionalPart() < 0.5)
                return r.GetIntegerPart();
            else if (r.GetFractionalPart() > 0.5)
                return r > 0 ? r.GetIntegerPart() + 1 : r.GetIntegerPart() - 1;
            else
            {
                var a = r.GetIntegerPart().Numerator;
                var b = r.GetIntegerPart() + 1;
                if (a % 2 == 0) return a;
                return b;
            }
        }
    }
}
