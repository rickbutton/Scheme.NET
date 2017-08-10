using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheme.NET.Numbers;

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

        public static void EnsureMaxArgCount(IEnumerable<ISExpression> args, int c)
        {
            if (args.Count() > c)
                ArgError("argument count must be equal to or less than " + c);
        }

        public static void EnsureAllNumber(IEnumerable<ISExpression> args)
        {
            if (args.Any(a => !a.IsNumber()))
                ArgError("all arguments must be numbers");
        }

        public static void EnsureAllExact(IEnumerable<ISExpression> args)
        {
            EnsureAllNumber(args);
            if (args.Any(a => !((NumberAtom) a).Val.IsExact))
                ArgError("all arguments must be exact");
        }

        public static void EnsureAllInteger(IEnumerable<ISExpression> args)
        {
            if (args.Any(a => !a.IsInteger()))
                ArgError("all arguments must be integers");
        }

        public static void EnsureAllRational(IEnumerable<ISExpression> args)
        {
            if (args.Any(a => !a.IsRational()))
                ArgError("all arguments must be rationals");
        }

        public static void EnsureAllReal(IEnumerable<ISExpression> args)
        {
            if (args.Any(a => !a.IsReal()))
                ArgError("all arguments must be reals");
        }

        public static void EnsureAllComplex(IEnumerable<ISExpression> args)
        {
            if (args.Any(a => !a.IsComplex()))
                ArgError("all arguments must be complex numbers");
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

        public static void EnsureList(IEnumerable<ISExpression> args, int i)
        {
            if (!args.ToArray()[i].IsList())
                ArgError($"argument {i} must be list");
        }

        public static void EnsureSymbol(IEnumerable<ISExpression> args, int i)
        {
            if (!args.ToArray()[i].IsSymbol())
                ArgError($"argument {i} must be symbol");
        }

        public static void ArgError(string msg)
        {
            throw new InvalidOperationException($"Argument error: {msg}");
        }
    }
}
