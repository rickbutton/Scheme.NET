using Scheme.NET.Numbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Scheme
{
    public static class SExpressionExtensions
    {
        public static bool IsNumber(this ISExpression sexpr) { return sexpr is NumberAtom; }
        public static bool IsBoolean(this ISExpression sexpr) { return sexpr is BooleanAtom; }
        public static bool IsSymbol(this ISExpression sexpr) { return sexpr is SymbolAtom; }
        public static bool IsString(this ISExpression sexpr) { return sexpr is StringAtom; }
        public static bool IsChar(this ISExpression sexpr) { return sexpr is CharAtom; }
        public static bool IsProcedure(this ISExpression sexpr) { return sexpr is Procedure; }
        public static bool IsCons(this ISExpression sexpr) { return sexpr is Cons; }
        public static bool IsVector(this ISExpression sexpr) { return sexpr is Vector; }
        public static bool IsNil(this ISExpression sexpr) { return sexpr is NilAtom; }

        public static bool IsComplex(this ISExpression sexpr) { return IsNumber(sexpr); }
        public static bool IsReal(this ISExpression sexpr) { return IsComplex(sexpr) && ((NumberAtom)(sexpr)).Val.Imag.IsZero; }
        public static bool IsRational(this ISExpression sexpr) { return IsReal(sexpr) && ((NumberAtom)(sexpr)).IsReal(); }
        public static bool IsInteger(this ISExpression sexpr)
        {
            var c = (sexpr as NumberAtom)?.Val;
            return IsRational(sexpr) &&
                (c == Complex.CreateExactReal(0) || c.IsInteger);
        }

        public static bool IsList(this ISExpression sexpr)
        {
            var c = sexpr as Cons;
            return sexpr.IsNil() || 
                (c != null && (c.Cdr.IsNil() || c.Cdr.IsList()));
        }

        public static IEnumerable<ISExpression> Flatten(this ISExpression s)
        {
            var ss = new List<ISExpression>();

            var ok = s is Cons;
            while (ok)
            {
                var c = s as Cons;
                ss.Add(c.Car);
                s = c.Cdr;
                ok = s is Cons;
            }

            if (s != AtomHelper.Nil)
                throw new InvalidOperationException("list is not flat");

            return ss;
        }

        public static IEnumerable<ISExpression> FlattenImproper(this ISExpression s)
        {
            var ss = new List<ISExpression>();

            var ok = s is Cons;
            while (ok)
            {
                var c = s as Cons;
                ss.Add(c.Car);
                s = c.Cdr;
                ok = s is Cons;
            }
            ss.Add(s);

            return ss;
        }

        public static ISExpression Unflatten(this IEnumerable<ISExpression> ss)
        {
            ISExpression c = AtomHelper.Nil;
            ss = ss.Reverse();
            foreach (var s in ss)
                c = AtomHelper.CreateCons(s, c);
            return c;
        }
    }
}
