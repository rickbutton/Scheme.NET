using System;
using System.Collections.Generic;
using System.Text;
using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine.Instructions;

namespace Scheme.NET.VirtualMachine
{
    public class Frame : IFrame
    {
        public IInstruction X { get; private set; }
        public IEnvironment E { get; private set; }
        public Stack<ISExpression> R { get; private set; }
        public Stack<IFrame> S { get; private set; }

        public Frame(IInstruction x, IEnvironment e, Stack<ISExpression> r, Stack<IFrame> s)
        {
            X = x;
            E = e;
            R = new Stack<ISExpression>(r);
            S = new Stack<IFrame>(s);
        }
    }
}
