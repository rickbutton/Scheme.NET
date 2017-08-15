using Scheme.NET.Numbers;
using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine.Natives.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.VirtualMachine.Natives
{
    public static class Types
    {
        [Count(1)]
        public static ISExpression IsBoolean(Scope scope, IEnumerable<ISExpression> args)
        {
            return AtomHelper.BooleanFromBool(args.First().IsBoolean());
        }

        [Count(1)]
        public static ISExpression IsPair(Scope scope, IEnumerable<ISExpression> args)
        {
            return AtomHelper.BooleanFromBool(args.First().IsCons());
        }

        [Count(1)]
        public static ISExpression IsSymbol(Scope scope, IEnumerable<ISExpression> args)
        {
            return AtomHelper.BooleanFromBool(args.First().IsSymbol());
        }

        [Count(1)]
        public static ISExpression IsNumber(Scope scope, IEnumerable<ISExpression> args)
        {
            return AtomHelper.BooleanFromBool(args.First().IsNumber());
        }

        [Count(1)]
        public static ISExpression IsInteger(Scope scope, IEnumerable<ISExpression> args)
        {
            var n = args.First() as NumberAtom;
            return AtomHelper.BooleanFromBool(args.First().IsInteger());
        }

        [Count(1)]
        public static ISExpression IsRational(Scope scope, IEnumerable<ISExpression> args)
        {
            var n = args.First() as NumberAtom;
            return AtomHelper.BooleanFromBool(args.First().IsRational());
        }

        [Count(1)]
        public static ISExpression IsReal(Scope scope, IEnumerable<ISExpression> args)
        {
            var n = args.First() as NumberAtom;
            return AtomHelper.BooleanFromBool(args.First().IsReal());
        }

        [Count(1)]
        public static ISExpression IsComplex(Scope scope, IEnumerable<ISExpression> args)
        {
            var n = args.First() as NumberAtom;
            return AtomHelper.BooleanFromBool(args.First().IsComplex());
        }

        [Count(1)]
        [AllNumbers]
        public static ISExpression IsExact(Scope scope, IEnumerable<ISExpression> args)
        {
            var n = args.First() as NumberAtom;
            return AtomHelper.BooleanFromBool(args.First().IsNumber() && n.Val.IsExact);
        }

        [Count(1)]
        [AllNumbers]
        public static ISExpression IsInexact(Scope scope, IEnumerable<ISExpression> args)
        {
            var n = args.First() as NumberAtom;
            return AtomHelper.BooleanFromBool(args.First().IsNumber() && !n.Val.IsExact);
        }

        [Count(1)]
        public static ISExpression IsChar(Scope scope, IEnumerable<ISExpression> args)
        {
            return AtomHelper.BooleanFromBool(args.First().IsChar());
        }

        [Count(1)]
        public static ISExpression IsString(Scope scope, IEnumerable<ISExpression> args)
        {
            return AtomHelper.BooleanFromBool(args.First().IsString());
        }

        [Count(1)]
        public static ISExpression IsProcedure(Scope scope, IEnumerable<ISExpression> args)
        {
            return AtomHelper.BooleanFromBool(args.First().IsProcedure());
        }

        [Count(1)]
        public static ISExpression IsNil(Scope scope, IEnumerable<ISExpression> args)
        {
            return AtomHelper.BooleanFromBool(args.First().IsNil());
        }

        [Count(2)]
        public static ISExpression Eqv(Scope scope, IEnumerable<ISExpression> args)
        {
            var a = args.ToArray()[0];
            var b = args.ToArray()[1];

            if (a.IsBoolean() && b.IsBoolean())
                return AtomHelper.BooleanFromBool(a.Equals(b));
            if (a.IsSymbol() && b.IsSymbol())
                return AtomHelper.BooleanFromBool(a.Equals(b));
            if (a.IsNumber() && b.IsNumber())
            {
                NumberAtom x = (NumberAtom)a, y = (NumberAtom)b;
                return AtomHelper.BooleanFromBool(
                    x.Val.IsExact == y.Val.IsExact &&
                    a.Equals(b));
            }
            if (a.IsChar() && b.IsChar())
                return AtomHelper.BooleanFromBool(a.Equals(b));
            if (a.IsNil() && b.IsNil())
                return AtomHelper.BooleanFromBool(true);
            if ((a.IsCons() && b.IsCons()))
                return AtomHelper.BooleanFromBool(((Cons)a) == ((Cons)b));
            if ((a.IsString() && b.IsString()))
                return AtomHelper.BooleanFromBool(((StringAtom)a) == ((StringAtom)b));
            if ((a.IsVector() && b.IsVector()))
                return AtomHelper.BooleanFromBool(((Vector)a) == ((Vector)b));
            if ((a is Procedure && b is Procedure))
                return AtomHelper.BooleanFromBool(((Procedure)a) == ((Procedure)b));
            if ((a.IsClosure() && b.IsClosure()))
                return AtomHelper.BooleanFromBool(((Closure)a) == ((Closure)b));

            return AtomHelper.BooleanFromBool(false);
        }

        [Count(2)]
        public static ISExpression Equal(Scope scope, IEnumerable<ISExpression> args)
        {

            var a = args.ToArray()[0];
            var b = args.ToArray()[1];

            if (
                (a.IsCons() && b.IsCons()) ||
                (a.IsVector() && b.IsVector()) ||
                (a.IsString() && b.IsString())
                )
            {
                return AtomHelper.BooleanFromBool((a.Equals(b)));
            }
            else
            {
                return Eqv(scope, args);
            }

        }
    }
}
