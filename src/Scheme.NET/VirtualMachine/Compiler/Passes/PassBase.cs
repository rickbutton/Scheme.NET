using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Compiler.Passes
{
    public abstract class PassBase<I, O> : ICompilerPass<I, O>
    {
        public abstract O Compile(ISchemeVM vm, I i);

        protected static void ThrowErr(string name, string msg, string rep) { throw new SchemeCompilerException(name, msg, rep); }
    }
}
