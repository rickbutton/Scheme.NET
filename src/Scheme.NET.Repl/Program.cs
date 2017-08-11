using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheme.NET.Eval;
using Scheme.NET.Lib;
using Antlr4.Runtime;
using Scheme.NET.Visitors;

namespace Scheme.NET.Repl
{
    class Program
    {
        public static void Main(string[] args)
        {
            var data = Library.CreateBase();
            var eval = new Evaluator(data);

            var input = "";
            while (true)
            {
                input += Console.ReadLine();
                if (IsBalanced(input))
                {
                    AntlrInputStream inputStream = new AntlrInputStream(input);
                    SchemeLexer lexer = new SchemeLexer(inputStream);
                    CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                    SchemeParser parser = new SchemeParser(commonTokenStream);

                    var context = parser.datum();
                    var visitor = new SchemeVisitor();
                    var expr = visitor.Visit(context);

                    try
                    {
                        var result = eval.Eval(expr, eval.GlobalScope);

                        if (result != null) 
                            Console.WriteLine(result.String());
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
