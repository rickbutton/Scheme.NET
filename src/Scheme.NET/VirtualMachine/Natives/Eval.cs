using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine.Compiler;
using Scheme.NET.VirtualMachine.Natives.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheme.NET.VirtualMachine.Natives
{
    public static class Eval 
    {
        [Count(2)]
        [Environment(1)]
        public static ISExpression Evaluate(IEnumerable<ISExpression> args)
        {
            var body = args.First();
            var env = args.Skip(1).First();

            var vm = new SchemeVM();
            vm.E = (Environment)env;

            try
            {
                var inst = SchemeCompiler.Compile(vm, body);
                var ret = vm.Execute(inst);
                return ret;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
                return AtomHelper.Nil;
            }
        }
    }
}
