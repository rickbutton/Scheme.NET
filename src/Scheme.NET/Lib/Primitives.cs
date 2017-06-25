using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Lib
{
    public static class Primitives
    {
        public static ISExpression Quote(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            return args.First();
        }
    }
}
