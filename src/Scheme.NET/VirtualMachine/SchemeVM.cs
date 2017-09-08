using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine.Compiler;
using Scheme.NET.VirtualMachine.Instructions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Scheme.NET.VirtualMachine
{
    public class SchemeVM : ISchemeVM
    {
        public ISExpression A { get; set; }
        public IEnvironment E { get; set; }
        public Stack<ISExpression> R { get; set; }
        public Stack<IFrame> S { get; set; }

        public SchemeVM() {
            E = new Environment();
            Reset();
        }

        public void Reset(bool env = true)
        {
            A = AtomHelper.Nil;
            R = new Stack<ISExpression>();
            S = new Stack<IFrame>();

            if (env)
            {
                E = AtomHelper.CreateEnvironment();
                AtomHelper.PopulateEnvironment(E, this);
            }
        }

        public ISExpression Execute(IInstruction x)
        {
            var success = false;
            try
            {
                while (x != null)
                {
                    //Console.WriteLine(x.ToString());
                    x = x.Execute(this);
                }
                success = true;
                return this.A;
            }
            finally
            {
                if (!success)
                {
                    Reset(false);
                }
            }
        }

        
    }
}
