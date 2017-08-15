using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class DefineInstruction : InstructionBase
    {
        public override string Name => "define";

        public ISExpression Var { get; private set; }
        public IInstruction Next { get; private set; }

        public DefineInstruction(ISExpression var, IInstruction next)
        {
            Var = var;
            Next = next;
        }

        public override string ToString()
        {
            return $"(define {Var.String()})";
        }

        public override IInstruction Execute(ISchemeVM vm)
        {
            if (!vm.E.IsDefined(Var))
            {
                vm.E.DefineHere(Var, vm.A);
                return Next;
            }
            ThrowErr("define", "attempted define of already defined indentifier", $"{Var.String()}");
            return null;
        }
    }
}
