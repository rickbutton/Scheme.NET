using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Compiler.Passes
{
    public class RewriteTwoArmIfPass : PassBase<ISExpression, ISExpression>
    {
        public override ISExpression Compile(ISchemeVM vm, ISExpression expr)
        {
            if (!expr.IsList() || expr == AtomHelper.Nil)
                return expr;

            var c = (Cons)expr;
            var op = c.Get(0);

            if (op == CompilerConstants.If)
            {
                var count = c.ListCount();

                if (count < 3 || count > 4)
                    ThrowErr("if", "invalid number of arguments", expr.String());

                if (count == 3)
                {
                    var test = Compile(vm, c.Get(1));
                    var then = Compile(vm, c.Get(2));
                    return AtomHelper.CreateList(CompilerConstants.If, test, then, AtomHelper.Nil);
                }
            }

            return expr;
        }
    }
}
