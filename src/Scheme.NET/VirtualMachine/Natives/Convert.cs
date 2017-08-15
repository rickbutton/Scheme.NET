using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheme.NET.Scheme;
using Scheme.NET.Numbers;
using Scheme.NET.VirtualMachine.Natives.Attributes;

namespace Scheme.NET.VirtualMachine.Natives
{
    public static class Convert
    {
        private static readonly int[] ValidRadix = new int[] { 2,8,10,16 };

        [MinCount(1)]
        [MaxCount(2)]
        [AllNumbers]
        public static ISExpression NumberToString(Scope scope, IEnumerable<ISExpression> args)
        {
            int radix = 10;
            if (args.Count() > 1)
            {
                var radixArg = (NumberAtom)args.ToArray()[1];
                radix = (int)radixArg.Val.Real;
            }

            if (!ValidRadix.Contains(radix))
            {
                throw new SchemeNativeException("number->string", "invalid radix specified", radix.ToString());
            }

            var val = args.ToArray()[0] as NumberAtom;

            return AtomHelper.StringFromString(val.Val.ToString(radix));
        }
    }
}
