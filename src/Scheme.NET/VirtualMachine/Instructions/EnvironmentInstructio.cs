using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class EnvironmentInstruction : InstructionBase
    {
        public override string Name => "assign";

        public bool Populate { get; private set; }
        public IInstruction Next { get; private set; }

        public EnvironmentInstruction(bool populate, IInstruction next)
        {
            Populate = populate;
            Next = next;
        }

        internal override string Serialize(int nest)
        {
            return $"(environment \n{NestInstr(Next, nest)})";
        }

        public override IInstruction Execute(ISchemeVM vm)
        {
            SetE(vm, NewEnv());

            if (Populate)
                vm.PopulateInitialEnvironment();

            return Next;
        }
    }
}
