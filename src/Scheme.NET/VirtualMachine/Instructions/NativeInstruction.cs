using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine.Natives.Attributes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class NativeInstruction : InstructionBase
    {
        public override string Name => "native";

        public Procedure Func { get; private set; }
        public IInstruction Next { get; private set; }

        public NativeInstruction(Procedure func, IInstruction next)
        {
            Func = func;
            Next = next;
        }

        public override IInstruction Execute(ISchemeVM vm)
        {
            EnsureValid(vm);
            try
            {
                SetA(vm, Func.Proc(vm.E.Scope, vm.R));
                SetR(vm, new Stack<ISExpression>());
                return Next;
            } catch (Exception e)
            {
                throw new SchemeRuntimeException(Func.Name, "unexpected exception during function application", e.Message);
            }
        }

        private void EnsureValid(ISchemeVM vm)
        {
            var methodInfo = Func.Proc.GetMethodInfo();

            var attrs = methodInfo.GetCustomAttributes<ArgAttribute>(true);

            foreach (var a in attrs)
            {
                a.Validate(vm.R);
            }
        }

        internal override string Serialize(int nest)
        {
            return $"(native {Func.Name}\n{NestInstr(Next, nest)})";
        }
    }
}
