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
        private IDictionary<SymbolAtom, ISExpression> _initial;

        public ISExpression A { get; set; }
        public IEnvironment E { get; set; }
        public Stack<ISExpression> R { get; set; }
        public Stack<IFrame> S { get; set; }

        public SchemeVM(IDictionary<SymbolAtom, ISExpression> initial) {
            _initial = initial;
            Reset();
        }

        public void Reset(bool env = true)
        {
            A = AtomHelper.Nil;
            R = new Stack<ISExpression>();
            S = new Stack<IFrame>();

            if (env)
            {
                E = new Environment();
                PopulateInitialEnvironment();
            }
        }

        public void PopulateInitialEnvironment()
        {
            foreach (var l in _initial)
                CreatePrimitive(l.Key.Val, (Procedure)l.Value);
        }

        private void CreatePrimitive(string name, Procedure p)
        {
            var def = AtomHelper.CreateList(
                AtomHelper.SymbolFromString("define"),
                AtomHelper.SymbolFromString(name),
                AtomHelper.CreateList(AtomHelper.SymbolFromString("lambda"), AtomHelper.Nil, p)
                );

            def = AtomHelper.CreateList(AtomHelper.SymbolFromString("define"), AtomHelper.SymbolFromString(name), p);

            var c = SchemeCompiler.Compile(def);
            Execute(c);
        }

        public ISExpression Execute(IInstruction x)
        {
            var success = false;
            try
            {
                while (x != null)
                {
                    Console.WriteLine(x.ToString());
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
