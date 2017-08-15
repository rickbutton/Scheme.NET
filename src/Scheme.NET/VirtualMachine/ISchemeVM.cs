using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine.Instructions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine
{
    public interface ISchemeVM
    {
        ISExpression A { get; set; }
        IEnvironment E { get; set; }
        Stack<ISExpression> R { get; set; }
        Stack<IFrame> S { get; set; }
        ISExpression Execute(IInstruction x);
        void PopulateInitialEnvironment();

        
    }
}
