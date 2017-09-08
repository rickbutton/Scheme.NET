using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class ContiInstruction : InstructionBase
    {
        public override string Name => "conti";

        public IInstruction Next { get; private set; }

        public ContiInstruction(IInstruction next)
        {
            Next = next;
        }

        public override IInstruction Execute(ISchemeVM vm)
        {
            SetA(vm, Continuation(vm.S, vm));
            return Next;
        }
    }
}
