using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class HaltInstruction : InstructionBase
    {
        public override string Name => "halt";

        public override IInstruction Execute(ISchemeVM vm)
        {
            return null;
        }
    }
}
