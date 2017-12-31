using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheme.NET.VirtualMachine.Compiler.Passes
{
    public abstract class PassBase<I, O> : ICompilerPass<I, O>
    {
        public abstract O Compile(ISchemeVM vm, I i);

        protected static void ThrowErr(string name, string msg, string rep) { throw new SchemeCompilerException(name, msg, rep); }

        protected static void CheckNotIllegalSymbol(ISExpression expr) {
            if (CompilerConstants.IllegalVariables.Contains(expr))
                throw new SchemeCompilerException("compiler", "name cannot be used as a variable", expr.ToString());
        }
    }
}
