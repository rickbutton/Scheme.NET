using Microsoft.SolverFoundation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Numbers
{
    public static class NumberTower
    {
        public static Complex<Rational> ExactInteger(BigInteger n) { return Rational.Get(n, 1);  }
        public static Complex<Rational> ExactRational(Rational n) { return n; } 
        public static Complex<Rational> ExactComplex(BigInteger r, BigInteger i) { return new Complex<Rational>(r, i); } 
        public static Complex<Rational> ExactComplex(Rational r, Rational i) { return new Complex<Rational>(r, i); } 

        public static Complex<double> InexactRational(double n) { return new Complex<double>(n, 0); } 
        public static Complex<double> InexactComplex(double r, double i) { return new Complex<double>(r, i); } 
    }
}
