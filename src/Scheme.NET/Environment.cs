using Antlr4.Runtime;
using Scheme.NET.Eval;
using Scheme.NET.Lib;
using Scheme.NET.Scheme;
using Scheme.NET.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheme.NET
{
    public class Environment
    {
        public Scope GlobalScope { get; private set; }

        protected Environment(IDictionary<SymbolAtom, ISExpression> initial)
        {
            GlobalScope = new Scope(initial);
        }

        public ISExpression[] Eval(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            SchemeLexer lexer = new SchemeLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            SchemeParser parser = new SchemeParser(commonTokenStream);

            var context = parser.body();
            var visitor = new SchemeVisitor();
            var expr = visitor.Visit(context);

            ISExpression[] arr;
            if (expr is ISExpression)
                arr = new ISExpression[] { (ISExpression)expr };
            else if (expr is ISExpression[])
                arr = (ISExpression[])expr;
            else
                throw new InvalidOperationException("parser error, unknown type: " + expr.GetType().Name);

            arr = arr.Select(e => Evaluator.Eval(GlobalScope, e)).ToArray();

            return arr;
        }

        public static Environment Create()
        {
            return new Environment(Library.CreateBase());
        }
    }
}
