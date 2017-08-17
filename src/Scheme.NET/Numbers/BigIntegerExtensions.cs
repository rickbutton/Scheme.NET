using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Numbers
{
    public static class BigIntegerHelpers
    {
        public static string ToString(this BigInteger i, int radix)
        {
            if (radix == 10)
                return i.ToString();

            var str = "";
            while (i != 0)
            {
                str = ConvertDigitToChar(i % radix) + str;
                i = i / radix;
            }
            return str;
        }

        public static string ConvertDigitToChar(BigInteger l)
        {
            var c = (char)(long)l;
            if (l <= 9)
                return ((char)(c + '0')).ToString();
            if (l <= 16)
                return ((char)(c - 10 + 'a')).ToString();
            throw new InvalidOperationException("invalid digit");
        }

        public static bool TryParse(string s, int radix, out BigInteger b)
        {
            b = 0;
            for (var i = 0; i < s.Length; i++)
            {
                BigInteger res;
                var success = TryParseDigit(s[i], radix, out res);
                if (!success) return false;

                res = res * BigInteger.Pow(radix, (s.Length - i - 1));
                b = b + res;
            }
            return true;
        }

        private static bool TryParseDigit(char c, int radix, out BigInteger b)
        {
            if (c == '0' || c == '1')
                b = c - '0';
            else if (radix >= 8 && c >= '0' && c <= '7')
                b = c - '0';
            else if (radix >= 10 && c >= '0' && c <= '9')
                b = c - '0';
            else if (radix == 16 && c >= 'a' && c <= 'f')
                b = c - 'a' + 10;
            else if (radix == 16 && c >= 'A' && c <= 'F')
                b = c - 'A' + 10;
            else
            {
                b = 0;
                return false;
            }
            return true;
        }
    }
}
