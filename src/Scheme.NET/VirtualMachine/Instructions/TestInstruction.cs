using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class TestInstruction : InstructionBase
    {
        public override string Name => "test";

        public IInstruction Then { get; private set; }
        public IInstruction Else { get; private set; }

        public TestInstruction(IInstruction t, IInstruction e)
        {
            Then = t;
            Else = e;
        }

        internal override string Serialize(int nest)
        {
            return $"(test \n{NestInstr(Then, nest)}\n{NestInstr(Else, nest)})";
        }

        public override IInstruction Execute(ISchemeVM vm)
        {
            if (vm.A == AtomHelper.True)
            {
                return Then;
            }
            return Else;
        }
    }
}
