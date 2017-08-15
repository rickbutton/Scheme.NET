using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class ArgumentInstruction : InstructionBase
    {
        public override string Name => "argument";

        public IInstruction Next { get; private set; }

        public ArgumentInstruction(IInstruction next)
        {
            Next = next;
        }

        internal override string Serialize(int nest)
        {
            return $"(argument \n{NestInstr(Next, nest)})";
        }

        public override IInstruction Execute(ISchemeVM vm)
        {
            vm.R.Push(vm.A);
            return Next;
        }
    }
}
