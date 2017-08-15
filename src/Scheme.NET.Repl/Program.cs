using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine.Compiler;
using Scheme.NET.VirtualMachine;
using Scheme.NET.VirtualMachine.Natives;
using Scheme.NET.Parser;

namespace Scheme.NET.Repl
{
    class Program
    {



        public static void Main(string[] args)
        {
            Console.WriteLine("Scheme.NET REPL v0.1");
            Console.WriteLine("\n\n");

            var lib = Library.CreateBase();
            var vm = new SchemeVM(lib);

            var input = "";
            while (true)
            {
                if (string.IsNullOrWhiteSpace(input))
                    Console.Write("> ");

                input += Console.ReadLine() + "\n";
                try
                {
                    if (IsBalanced(input))
                    {
                        var arr = ParserHelpers.Parse(input);
                        var carr = arr.Select(a => SchemeCompiler.Compile(a));

                        foreach (var c in carr)
                        {
                            var s = vm.Execute(c);
                            if (s != null)
                                Console.WriteLine(s.String());
                        }
                        input = "";
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                    input = "";
                }
            }
        }

        public static int CountOpen(string input) { return input.Count(c => c == '('); }
        public static int CountClose(string input) { return input.Count(c => c == ')'); }

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
