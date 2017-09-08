using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class EnvironmentInstruction : InstructionBase
    {
        public override string Name => "environment";
        public bool Populate { get; private set; }
        public IInstruction Next { get; private set; }

        public EnvironmentInstruction(bool populate, IInstruction next)
        {
            Populate = populate;
            Next = next;
        }

        public override IInstruction Execute(ISchemeVM vm)
        {
            var e = AtomHelper.CreateEnvironment();

            if (Populate)
                AtomHelper.PopulateEnvironment(e, vm);

            SetA(vm, e);
            return Next;
        }
    }
}
