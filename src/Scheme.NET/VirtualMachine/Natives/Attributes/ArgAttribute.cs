using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheme.NET.VirtualMachine.Natives.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class ArgAttribute : Attribute
    {
        public abstract void Validate(IEnumerable<ISExpression> args);

        protected void Throw(string msg, IEnumerable<ISExpression> args)
        {
            throw new SchemeNativeException("native", msg, args.Unflatten().String());
        }
    }

    public class CountAttribute : ArgAttribute
    {
        private int c;

        public CountAttribute(int c) { this.c = c; }

        public override void Validate(IEnumerable<ISExpression> args)
        {
            if (args.Count() != c)
                Throw($"required {c} arguments, instead but {args.Count()} were specified", args);
        }
    }

    public class MinCountAttribute : ArgAttribute
    {
        private int c;

        public MinCountAttribute(int c) { this.c = c; }

        public override void Validate(IEnumerable<ISExpression> args)
        {
            if (args.Count() < c)
                Throw($"required at least {c} arguments, instead but {args.Count()} were specified", args);
        }
    }

    public class MaxCountAttribute : ArgAttribute
    {
        private int c;

        public MaxCountAttribute(int c) { this.c = c; }

        public override void Validate(IEnumerable<ISExpression> args)
        {
            if (args.Count() > c)
                Throw($"required at most {c} arguments, instead but {args.Count()} were specified", args);
        }
    }

    public class PredicateAttribute : ArgAttribute
    {
        private string positive, negative;
        private Func<ISExpression, bool> predicate;
        private int? index;

        public PredicateAttribute(string pos, string neg, Func<ISExpression, bool> predicate)
        {
            this.positive = pos;
            this.negative = neg;
            this.predicate = predicate;
            this.index = null;
        }

        public PredicateAttribute(string pos, string neg, Func<ISExpression, bool> predicate, int index)
        {
            this.positive = pos;
            this.negative = neg;
            this.predicate = predicate;
            this.index = index;
        }

        public override void Validate(IEnumerable<ISExpression> args)
        {
            if (index.HasValue)
            {
                if (!predicate(args.ToArray()[index.Value]))
                    Throw($"required {positive} argument in {index.Value} position, but {negative} argument was specified", args);
            }
            else
            {
                if (args.Any(a => !predicate(a)))
                    Throw($"required all {positive} arguments, but {negative} arguments were specified", args);
            }
        }
    }

    public class AllNumbersAttribute : PredicateAttribute
    {
        public AllNumbersAttribute() : base("number", "non-number", (a) => a.IsNumber()) { }
    }

    public class AllExactsAttribute : PredicateAttribute
    {
        public AllExactsAttribute() : base("exact number", "non-exact number", (a) => (a.IsNumber() && ((NumberAtom)a).Val.IsExact)) { }
    }

    public class AllIntegersAttribute : PredicateAttribute
    {
        public AllIntegersAttribute() : base("integer", "non-integer", (a) => a.IsInteger()) { }
    }

    public class AllRationalsAttribute : PredicateAttribute
    {
        public AllRationalsAttribute() : base("rational", "non-rational", (a) => a.IsRational()) { }
    }

    public class AllRealsAttribute : PredicateAttribute
    {
        public AllRealsAttribute() : base("real", "non-real", (a) => a.IsReal()) { }
    }

    public class AllComplexesAttribute : PredicateAttribute
    {
        public AllComplexesAttribute() : base("complex", "non-complex", (a) => a.IsComplex()) { }
    }

    public class AllConsAttribute : PredicateAttribute
    {
        public AllConsAttribute() : base("cons", "non-cons", (a) => a.IsCons()) { }
    }

    public class ConsAttribute : PredicateAttribute
    {
        public ConsAttribute(int i) : base("cons", "non-cons", (a) => a.IsCons(), i) { }
    }

    public class ListAttribute : PredicateAttribute
    {
        public ListAttribute(int i) : base("list", "non-list", (a) => a.IsList(), i) { }
    }

    public class SymbolAttribute : PredicateAttribute
    {
        public SymbolAttribute(int i) : base("symbol", "non-symbol", (a) => a.IsSymbol(), i) { }
    }
}
