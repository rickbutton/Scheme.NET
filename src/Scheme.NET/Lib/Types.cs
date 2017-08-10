using Scheme.NET.Numbers;
using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Lib
{
    public static class Types
    {
        public static ISExpression IsBoolean(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.BooleanFromBool(args.First().IsBoolean());
        }

        public static ISExpression IsPair(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.BooleanFromBool(args.First().IsCons());
        }

        public static ISExpression IsSymbol(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.BooleanFromBool(args.First().IsSymbol());
        }

        public static ISExpression IsNumber(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.BooleanFromBool(args.First().IsNumber());
        }

        public static ISExpression IsInteger(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            var n = args.First() as NumberAtom;
            return AtomHelper.BooleanFromBool(args.First().IsInteger());
        }

        public static ISExpression IsRational(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            var n = args.First() as NumberAtom;
            return AtomHelper.BooleanFromBool(args.First().IsRational());
        }

        public static ISExpression IsReal(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            var n = args.First() as NumberAtom;
            return AtomHelper.BooleanFromBool(args.First().IsReal());
        }

        public static ISExpression IsComplex(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            var n = args.First() as NumberAtom;
            return AtomHelper.BooleanFromBool(args.First().IsComplex());
        }

        public static ISExpression IsExact(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            LibHelper.EnsureAllNumber(args);
            var n = args.First() as NumberAtom;
            return AtomHelper.BooleanFromBool(args.First().IsNumber() && n.Val.IsExact);
        }

        public static ISExpression IsInexact(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            LibHelper.EnsureAllNumber(args);
            var n = args.First() as NumberAtom;
            return AtomHelper.BooleanFromBool(args.First().IsNumber() && !n.Val.IsExact);
        }

        public static ISExpression IsChar(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.BooleanFromBool(args.First().IsChar());
        }

        public static ISExpression IsString(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.BooleanFromBool(args.First().IsString());
        }

        public static ISExpression IsProcedure(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.BooleanFromBool(args.First().IsProcedure());
        }

        public static ISExpression IsNil(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            return AtomHelper.BooleanFromBool(args.First().IsNil());
        }

        public static ISExpression Eqv(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 2);
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
            if ((a.IsCons() && b.IsCons()) || 
                (a.IsString() && b.IsString()) || 
                (a.IsVector() && b.IsVector()) || 
                (a.IsProcedure() && b.IsProcedure()))
                return AtomHelper.BooleanFromBool(a == b);

            return AtomHelper.BooleanFromBool(false);
        }

        public static ISExpression Equal(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 2);

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
