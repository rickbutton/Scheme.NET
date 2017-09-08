using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Compiler
{
    public interface ICompilerPass<I, O>
    {
        O Compile(ISchemeVM vm, I i);
    }
}
