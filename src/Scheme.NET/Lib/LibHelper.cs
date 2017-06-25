using Scheme.NET.Numbers;
using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Lib
{
    public static class LibHelper
    {
        public static void EnsureArgCount(IEnumerable<ISExpression> args, int c)
        {
            if (args.Count() != c)
                ArgError("argument count must be " + c);
        }

        public static void EnsureMinArgCount(IEnumerable<ISExpression> args, int c)
        {
            if (args.Count() < c)
                ArgError("argument count must be at least " + c);
        }

        public static void EnsureAllNumber(IEnumerable<ISExpression> args)
        {
            if (args.Any(a => !a.IsNumber()))
                ArgError("all arguments must be numbers");
        }

        public static void EnsureAllInteger(IEnumerable<ISExpression> args)
        {
            if (args.Any(a => !a.IsNumber() && BigRational.Factor((a as NumberAtom).Val).Denominator == 1))
                ArgError("all arguments must be integers");
        }

        public static void EnsureAllCons(IEnumerable<ISExpression> args)
        {
            if (args.Any(a => !a.IsCons()))
                ArgError("all arguments must be cons");
        }

        public static void EnsureCons(IEnumerable<ISExpression> args, int i)
        {
            if (!args.ToArray()[i].IsCons())
                ArgError($"argument {i} must be cons");
        }

        private static void ArgError(string msg)
        {
            throw new InvalidOperationException($"Argument error: {msg}");
        }
    }
}
