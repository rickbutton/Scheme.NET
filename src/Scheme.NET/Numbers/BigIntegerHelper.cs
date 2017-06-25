using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Numbers
{
    public static class BigIntegerHelper
    {
        public static BigInteger Modulo(BigInteger a, BigInteger b)
        {
            return ((a % b) + b) % b;
        }
    }
}
