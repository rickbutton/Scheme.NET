using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public class ApplyInstruction : InstructionBase
    {
        public override string Name => "apply";

        public override IInstruction Execute(ISchemeVM vm)
        {
            IEnvironment e = vm.E;

            if (vm.A.IsClosure())
            {
                var c = vm.A as Closure;
                if (c.Vars.IsSymbol())
                {
                    e = Extend(vm.E, CreateMap(AtomHelper.CreateList(c.Vars), vm.R));
                }
                else if (c.Vars.IsList())
                {
                    e = Extend(vm.E, CreateMap(c.Vars, vm.R));
                }
                else if (!c.Vars.IsNil())
                    ThrowErr("apply", "attempted application with invalid binding parameter", $"parameter vars: {c.Vars.String()}");

                SetE(vm, e);
                return c.Body;
            }
            else if (vm.A.IsProcedure())
            {
                var p = vm.A as Procedure;

                p.EnsureArgsValid(vm.R);

                SetA(vm, p.Proc(vm.E.Scope, vm.R));
                SetR(vm, new Stack<ISExpression>());
                return new ReturnInstruction();
            }
            ThrowErr("apply", "attempted application of non-function", $"({vm.A.String()})");
            return null;
        }

        private IDictionary<SymbolAtom, ISExpression> CreateMap(ISExpression vars, Stack<ISExpression> rib)
        {
            var map = new Dictionary<SymbolAtom, ISExpression>();

            var vflat = vars.Flatten();

            // this gets weird because native needs the args
            // and we are wrapping all natives in a zero arg lambda
            //if (vflat.Count() != rib.Count)
            //    ThrowErr("apply", "attempted application of function with incorrect number of parameters", $"({vars.String()})");

            if (vars != AtomHelper.Nil)
            {
                foreach (var v in vflat)
                {
                    if (!v.IsSymbol())
                        ThrowErr("apply", "attempted application of function with incorrectly defined bindings", $"({vars.String()})");


                    map[v as SymbolAtom] = rib.Pop();
                }
            }
            return map;
        }

        internal override string Serialize(int nest)
        {
            return $"(apply)";
        }
    }
}
