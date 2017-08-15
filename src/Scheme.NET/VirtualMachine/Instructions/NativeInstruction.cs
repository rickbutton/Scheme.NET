using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class NativeInstruction : InstructionBase
    {
        public override string Name => "native";

        public Procedure Proc { get; private set; }
        public IInstruction Next { get; private set; }

        public NativeInstruction(Procedure proc, IInstruction next)
        {
            Proc = proc;
            Next = next;
        }

        public override IInstruction Execute(ISchemeVM vm)
        {
            Proc.EnsureArgsValid(vm.R);

            SetA(vm, Proc.Proc(vm.R));
            SetR(vm, new Stack<ISExpression>());
            return Next;
        }
    }
}
