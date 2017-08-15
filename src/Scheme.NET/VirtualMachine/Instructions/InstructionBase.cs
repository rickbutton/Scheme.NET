using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public abstract class InstructionBase : IInstruction
    {
        public abstract string Name { get; }

        public abstract IInstruction Execute(ISchemeVM vm);

        protected ISExpression Lookup(ISExpression v, IEnvironment e)
        {
            return e.Lookup(v);
        }

        protected ISExpression Closure(IInstruction body, IEnvironment e, ISExpression args)
        {
            return new Closure(body, e, args);
        }

        protected ISExpression Continuation(Stack<IFrame> s)
        {
            var v = AtomHelper.SymbolFromString("v");
            return Closure(new NuateInstruction(s, v), NewEnv(), AtomHelper.CreateList(v));
        }

        protected IEnvironment Extend(IEnvironment e, IDictionary<SymbolAtom, ISExpression> vars)
        {
            return new Environment(e, vars);
        }

        protected IEnvironment NewEnv()
        {
            return new Environment();
        }

        protected void PushFrame(ISchemeVM vm, IInstruction x, IEnvironment e, Stack<ISExpression> r, Stack<IFrame> s)
        {
            vm.S.Push(new Frame(x, e, r, s));
        }

        protected void SetA(ISchemeVM vm, ISExpression a)
        {
            vm.A = a;
        }

        protected void SetE(ISchemeVM vm, IEnvironment e)
        {
            vm.E = e;
        }

        protected void SetR(ISchemeVM vm, Stack<ISExpression> r)
        {
            vm.R = r;
        }

        protected void SetS(ISchemeVM vm, Stack<IFrame> s)
        {
            vm.S = s;
        }

        protected void ThrowErr(string context, string msg, string rep)
        {
            throw new SchemeRuntimeException(context, msg, rep);
        }

        public override string ToString()
        {
            return $"({Name})";
        }
    }
}
