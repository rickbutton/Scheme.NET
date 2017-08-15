using Antlr4.Runtime;
using Scheme.NET.Parser.Visitors;
using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheme.NET.Parser
{
    public static class ParserHelpers
    {
        public static ISExpression[] Parse(string input)
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

            return arr;
        }
    }
}
