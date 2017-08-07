using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheme.NET.Scheme;
using Scheme.NET.Numbers;
using Microsoft.SolverFoundation.Common;

namespace Scheme.NET.Lib
{
    public static class Convert
    {

        private static readonly Complex[] ValidRadix = new Complex[] 
        {
            NumberTower.ExactInteger(2),
            NumberTower.ExactInteger(8),
            NumberTower.ExactInteger(10),
            NumberTower.ExactInteger(16),
        };

        public static ISExpression NumberToString(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureMinArgCount(args, 1);
            LibHelper.EnsureMaxArgCount(args, 2);
            LibHelper.EnsureAllNumber(args);

            Complex radix = NumberTower.ExactInteger(10);
            if (args.Count() > 1)
            {
                var radixArg = (NumberAtom)args.ToArray()[1];
                radix = radixArg.Val;
            }

            if (!ValidRadix.Contains(radix))
            {
                LibHelper.ArgError("Invalid radix specified");
            }

            var iRadix = (int)radix.AsInexact().Real;

            var val = args.ToArray()[0] as NumberAtom;

            return AtomHelper.StringFromString(val.Val.ToString(iRadix));
        }
    }
}
