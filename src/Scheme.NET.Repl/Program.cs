using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheme.NET.Eval;
using Scheme.NET.Lib;
using Antlr4.Runtime;
using Scheme.NET.Visitors;
using Scheme.NET.Scheme;

namespace Scheme.NET.Repl
{
    class Program
    {
        public static void Main(string[] args)
        {
            var env = Environment.Create();

            var input = "";
            while (true)
            {
                input += Console.ReadLine();
                if (IsBalanced(input))
                {
                    try
                    {
                        var arr = env.Eval(input);

                        foreach (var a in arr)
                        {
                            if (a != null) 
                                Console.WriteLine(a.String());
                        }
                    } catch (Exception e)
                    {
                        Console.WriteLine("[ERROR] " + e.Message);
                    }
                    input = "";

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
                if (count < 0) return false;
            }
            if (count == 0) return true;
            return false;
        }
    }
}
