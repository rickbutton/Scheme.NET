using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheme.NET.Lexer;
using Scheme.NET.Parser;
using Scheme.NET.Eval;
using Scheme.NET.Lib;

namespace Scheme.NET.Repl
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = Library.CreateBase();
            var eval = new Evaluator(data);
            var lexer = new SchemeLexer();

            var input = "";
            while (true)
            {
                input += Console.ReadLine();
                if (IsBalanced(input))
                {
                    var tokens = lexer.Lex(input);
                    input = "";

                    var s = SchemeParser.Parse(tokens);

                    try
                    {
                        s = s.Select(ss => eval.Eval(ss, eval.GlobalScope));

                        foreach (var se in s)
                            if (se != null)
                                Console.WriteLine(se.String());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[ERROR] " + e.Message);
                    }
                    Console.WriteLine();
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
