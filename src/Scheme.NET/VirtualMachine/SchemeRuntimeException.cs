using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine
{
    public class SchemeRuntimeException : Exception
    {
        public string Context { get; private set; }
        public string ErrorMessage { get; private set; }
        public string ExpressionString { get; private set; }

        public SchemeRuntimeException(string context, string msg, string rep) : base()
        {
            Context = context;
            ErrorMessage = msg;
            ExpressionString = rep;
        }

        public override string Message => $"{Context}: {ErrorMessage}\n    in {ExpressionString}";
    }
}
