using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine.Compiler.Passes;
using Scheme.NET.VirtualMachine.Instructions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Compiler
{
    public class SchemeCompiler
    {
        public static IInstruction Compile(ISchemeVM vm, string input)
        {
            var p1 = DoPass<ParserPass, string, ISExpression>(vm, input);
            return Compile(vm, p1);
        }

        public static IInstruction Compile(ISchemeVM vm, ISExpression expr)
        {
            var p1 = DoPass<RewriteLetPass, ISExpression, ISExpression>(vm, expr);
            var p2 = DoPass<RewriteTwoArmIfPass, ISExpression, ISExpression>(vm, p1);
            var p3 = DoPass<InstructionPass, ISExpression, IInstruction>(vm, p2);
            return p3;
        }

        private static O DoPass<T, I, O>(ISchemeVM vm, I i) where T : ICompilerPass<I, O>, new()
        {
            var pass = new T();
            return pass.Compile(vm, i);
        }
    }
}
