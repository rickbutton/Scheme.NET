using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public interface IInstruction
    {
        string Name { get; }
        IInstruction Execute(ISchemeVM vm);
    }
}
