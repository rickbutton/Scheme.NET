using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Linq;

namespace Scheme.NET.Visitors
{
    public class SchemeVisitor : SchemeBaseVisitor<object>
    {
        public override object VisitBody([NotNull] SchemeParser.BodyContext context)
        {
            var elements = context.datum();
            return elements
                .Select(Visit)
                .Cast<ISExpression>()
                .ToArray();
        }

        public override object VisitCharacter([NotNull] SchemeParser.CharacterContext context)
        {
            var text = context.CHARACTER().GetText();
            return AtomHelper.CharFromString(text);
        }

        public override object VisitFalse([NotNull] SchemeParser.FalseContext context)
        {
            return AtomHelper.False;
        }

        public override object VisitIdentifier([NotNull] SchemeParser.IdentifierContext context)
        {
            return AtomHelper.SymbolFromString(context.IDENTIFIER().GetText());
        }

        public override object VisitPair([NotNull] SchemeParser.PairContext context)
        {
            var elements = context.datum().Select(Visit)
                .Cast<ISExpression>()
                .ToArray();
            return AtomHelper.CreateCons(elements[0], elements[1]);
        }

        public override object VisitList([NotNull] SchemeParser.ListContext context)
        {
            var elements = context.datum().Select(Visit);
            if (elements.Count() == 0)
                return AtomHelper.Nil;
            return CreateList(elements.Cast<ISExpression>());
        }

        private object CreateList(IEnumerable<ISExpression> elements)
        {
            if (elements.Count() == 0)
                return AtomHelper.Nil;
            else
                return AtomHelper.CreateCons(
                    elements.First(), 
                    (ISExpression)CreateList(elements.Skip(1)));
        }

        public override object VisitNum([NotNull] SchemeParser.NumContext context)
        {
            return AtomHelper.NumberFromString(context.GetText());
        }

        public override object VisitQuote([NotNull] SchemeParser.QuoteContext context)
        {
            var expr = (ISExpression)Visit(context.datum());
            return AtomHelper.CreateCons(
                AtomHelper.SymbolFromString("quote"),
                AtomHelper.CreateCons(expr, AtomHelper.Nil));
        }

        public override object VisitString([NotNull] SchemeParser.StringContext context)
        {
            var str = context.STRING().GetText();
            return AtomHelper.StringFromString(str.Substring(1, str.Length - 2));
        }

        public override object VisitTrue([NotNull] SchemeParser.TrueContext context)
        {
            return AtomHelper.True;
        }

        public override object VisitVector([NotNull] SchemeParser.VectorContext context)
        {
            var elements = context.datum().Select(d => Visit(d))
                .Cast<ISExpression>();
            return AtomHelper.CreateVector(elements);
        }
    }
}
