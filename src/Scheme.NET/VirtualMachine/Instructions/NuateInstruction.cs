using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class NuateInstruction : InstructionBase
    {
        public override string Name => "nuate";

        public Stack<IFrame> Stack { get; private set; }
        public ISExpression Var { get; private set; }

        public NuateInstruction(Stack<IFrame> stack, ISExpression var)
        {
            Stack = stack;
            Var = var;
        }

        internal override string Serialize(int nest)
        {
            return $"(nuate STACKHERE\n{Var.String()})";
        }

        public override IInstruction Execute(ISchemeVM vm)
        {
            var a = Lookup(Var, vm.E);
            SetA(vm, a);
            SetS(vm, Stack);
            return new ReturnInstruction();
        }
    }
}
