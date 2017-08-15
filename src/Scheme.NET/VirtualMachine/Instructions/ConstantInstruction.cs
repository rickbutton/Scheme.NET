using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class ConstantInstruction : InstructionBase
    {
        public override string Name => "constant";

        public ISExpression Obj { get; private set; }
        public IInstruction Next { get; private set; }

        public ConstantInstruction(ISExpression x, IInstruction next)
        {
            Obj = x;
            Next = next;
        }

        public override string ToString()
        {
            return $"(constant {Obj.String()})";
        }

        public override IInstruction Execute(ISchemeVM vm)
        {
            SetA(vm, Obj);
            return Next;
        }
    }
}
