using Scheme.NET.Parser;
using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine.Natives.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheme.NET.VirtualMachine.Natives
{
    public static class IO
    {

        [Count(1)]
        public static ISExpression Display(IEnumerable<ISExpression> args)
        {
            var a = args.First();

            // TODO implement R5RS compatible ports
            // for now just dump to console
            Console.Write(a.String());

            return AtomHelper.Nil;
        }

        [Count(0)]
        public static ISExpression Read(IEnumerable<ISExpression> args)
        {
            var input = "";

            while (true)
            {
                input += Console.ReadLine() + "\n";

                if (IsBalanced(input))
                {
                    var expr = ParserHelpers.Parse(input);
                    return expr.First();
                }
            }
        }

        public static bool IsBalanced(string input)
        {
            int count = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(') count++;
                if (input[i] == ')') count--;
                if (count < 0) throw new Exception("invalid syntax");
            }
            if (count == 0) return true;
            return false;
        }
    }
}
