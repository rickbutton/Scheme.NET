using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Compiler
{
    public static class CompilerConstants
    {
        public static readonly ISExpression Quote = AtomHelper.SymbolFromString("quote");
        public static readonly ISExpression Let = AtomHelper.SymbolFromString("let");
        public static readonly ISExpression LetStar = AtomHelper.SymbolFromString("let*");
        public static readonly ISExpression Lambda = AtomHelper.SymbolFromString("lambda");
        public static readonly ISExpression If = AtomHelper.SymbolFromString("if");
        public static readonly ISExpression SetBang = AtomHelper.SymbolFromString("set!");
        public static readonly ISExpression Define = AtomHelper.SymbolFromString("define");
        public static readonly ISExpression Eval = AtomHelper.SymbolFromString("eval");
        public static readonly ISExpression CallCC = AtomHelper.SymbolFromString("call/cc");
        public static readonly ISExpression Begin = AtomHelper.SymbolFromString("begin");

        public static readonly ISExpression SchemeReportEnvironment = AtomHelper.SymbolFromString("scheme-report-environment");
        public static readonly ISExpression NullEnvironment = AtomHelper.SymbolFromString("null-environment");

        public static readonly ISExpression[] IllegalVariables = new ISExpression[]
        {
            CompilerConstants.Quote, CompilerConstants.Let, CompilerConstants.LetStar, CompilerConstants.Lambda,
            CompilerConstants.If, CompilerConstants.SetBang, CompilerConstants.Define, CompilerConstants.CallCC,
            CompilerConstants.Begin, CompilerConstants.SchemeReportEnvironment, CompilerConstants.NullEnvironment
        };
    }
}
