using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class ReferInstruction : InstructionBase
    {
        public override string Name => "refer";

        public ISExpression Var { get; private set; }
        public IInstruction Next { get; private set; }

        public ReferInstruction(ISExpression x, IInstruction next)
        {
            Var = x;
            Next = next;
        }

        internal override string Serialize(int nest)
        {
            return $"(refer {Var.String()}\n{NestInstr(Next, nest)})";
        }

        public override IInstruction Execute(ISchemeVM vm)
        {
            var a = Lookup(Var, vm.E);

            if (a == null)
                ThrowErr("refer", "attempted reference of undefined indentifier", $"{Var.String()}");

            SetA(vm, a);
            return Next;
        }
    }
}
