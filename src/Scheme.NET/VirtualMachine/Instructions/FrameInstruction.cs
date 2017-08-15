using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class FrameInstruction : InstructionBase
    {
        public override string Name => "frame";

        public IInstruction Ret { get; private set; }
        public IInstruction Next { get; private set; }

        public FrameInstruction(IInstruction ret, IInstruction next)
        {
            Next = next;
            Ret = ret;
        }

        public override IInstruction Execute(ISchemeVM vm)
        {
            PushFrame(vm, Ret, vm.E, vm.R, vm.S);
            return Next;
        }
    }
}
