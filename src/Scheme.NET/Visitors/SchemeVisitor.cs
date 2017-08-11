using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Linq;

namespace Scheme.NET.Visitors
{
    public class SchemeVisitor : SchemeBaseVisitor<ISExpression>
    {
        public override ISExpression VisitCharacter([NotNull] SchemeParser.CharacterContext context)
        {
            var text = context.CHARACTER().GetText();
            return AtomHelper.CharFromString(text);
        }

        public override ISExpression VisitFalse([NotNull] SchemeParser.FalseContext context)
        {
            return AtomHelper.False;
        }

        public override ISExpression VisitIdentifier([NotNull] SchemeParser.IdentifierContext context)
        {
            return AtomHelper.SymbolFromString(context.IDENTIFIER().GetText());
        }

        public override ISExpression VisitPair([NotNull] SchemeParser.PairContext context)
        {
            var elements = context.datum().Select(Visit).ToArray();
            return AtomHelper.CreateCons(elements[0], elements[1]);
        }

        public override ISExpression VisitList([NotNull] SchemeParser.ListContext context)
        {
            var elements = context.datum().Select(Visit);
            if (elements.Count() == 0)
                return AtomHelper.Nil;
            return CreateList(elements);
        }

        private ISExpression CreateList(IEnumerable<ISExpression> elements)
        {
            if (elements.Count() == 0)
                return AtomHelper.Nil;
            else
                return AtomHelper.CreateCons(elements.First(), CreateList(elements.Skip(1)));
        }

        public override ISExpression VisitNum([NotNull] SchemeParser.NumContext context)
        {
            return AtomHelper.NumberFromString(context.GetText());
        }

        public override ISExpression VisitQuote([NotNull] SchemeParser.QuoteContext context)
        {
            var expr = Visit(context.datum());
            return AtomHelper.CreateCons(
                AtomHelper.SymbolFromString("quote"),
                AtomHelper.CreateCons(expr, AtomHelper.Nil));
        }

        public override ISExpression VisitString([NotNull] SchemeParser.StringContext context)
        {
            var str = context.STRING().GetText();
            return AtomHelper.StringFromString(str.Substring(1, str.Length - 2));
        }

        public override ISExpression VisitTrue([NotNull] SchemeParser.TrueContext context)
        {
            return AtomHelper.True;
        }

        public override ISExpression VisitVector([NotNull] SchemeParser.VectorContext context)
        {
            var elements = context.datum().Select(d => Visit(d));
            return AtomHelper.CreateVector(elements);
        }
    }
}
