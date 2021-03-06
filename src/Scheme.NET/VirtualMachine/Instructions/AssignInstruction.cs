﻿using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class AssignInstruction : InstructionBase
    {
        public override string Name => "assign";

        public ISExpression Var { get; private set; }
        public IInstruction Next { get; private set; }

        public AssignInstruction(ISExpression x, IInstruction next)
        {
            Var = x;
            Next = next;
        }

        public override string ToString()
        {
            return $"(assign {Var.String()})";
        }

        public override IInstruction Execute(ISchemeVM vm)
        {
            var success = vm.E.Set(Var, vm.A);
            if (!success)
                ThrowErr("assign", "attempted assignment of undefined indentifier", $"{Var.String()}");
            return Next;
        }
    }
}
