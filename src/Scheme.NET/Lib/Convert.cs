using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheme.NET.Scheme;
using Scheme.NET.Numbers;

namespace Scheme.NET.Lib
{
    public static class Convert
    {
        private static readonly int[] ValidRadix = new int[] { 2,8,10,16 };

        public static ISExpression NumberToString(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureMinArgCount(args, 1);
            LibHelper.EnsureMaxArgCount(args, 2);
            LibHelper.EnsureAllNumber(args);

            int radix = 10;
            if (args.Count() > 1)
            {
                var radixArg = (NumberAtom)args.ToArray()[1];
                radix = (int)radixArg.Val.Real;
            }

            if (!ValidRadix.Contains(radix))
            {
                LibHelper.ArgError("Invalid radix specified");
            }

            var val = args.ToArray()[0] as NumberAtom;

            return AtomHelper.StringFromString(val.Val.ToString(radix));
        }
    }
}
