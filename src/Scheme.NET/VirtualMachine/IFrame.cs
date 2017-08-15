using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine.Instructions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine
{
    public interface IFrame
    {
        IInstruction X { get; }
        IEnvironment E { get; }
        Stack<ISExpression> R { get; }
        Stack<IFrame> S { get; }
    }
}
