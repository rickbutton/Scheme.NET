using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class ReturnInstruction : InstructionBase
    {
        public override string Name => "return";

        public override IInstruction Execute(ISchemeVM vm)
        {
            var frame = vm.S.Pop();

            SetE(vm, frame.E);
            SetR(vm, frame.R);
            SetS(vm, frame.S);

            return frame.X;
        }

        internal override string Serialize(int nest)
        {
            return "(return)";
        }
    }
}
