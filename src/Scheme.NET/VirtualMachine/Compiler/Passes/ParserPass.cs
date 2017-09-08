using Scheme.NET.Parser;
using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Compiler.Passes
{
    public class ParserPass : PassBase<string, ISExpression>
    {
        public override ISExpression Compile(ISchemeVM vm, string i)
        {
            var exprs = ParserHelpers.Parse(i);

            if (exprs.Length == 1)
                return exprs[0];

            return AtomHelper.CreateCons(CompilerConstants.Begin, exprs.Unflatten());
        }
    }
}
