using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class CloseInstruction : InstructionBase
    {
        public override string Name => "close";

        public ISExpression Vars { get; private set; }
        public IInstruction Body { get; private set; }
        public IInstruction Next { get; private set; }

        public CloseInstruction(ISExpression vars, IInstruction body, IInstruction next)
        {
            Vars = vars;
            Body = body;
            Next = next;
        }

        internal override string Serialize(int nest)
        {
            return $"(close {Vars.String()}\n{NestInstr(Body, nest)}\n{NestInstr(Next, nest)})";
        }

        public override IInstruction Execute(ISchemeVM vm)
        {
            var a = Closure(Body, vm.E, Vars);
            SetA(vm, a);
            return Next;
        }
    }
}
