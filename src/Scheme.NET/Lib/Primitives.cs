using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Lib
{
    public static class Primitives
    {
        public static ISExpression Quote(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 1);
            return args.First();
        }

        public static ISExpression Set(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureArgCount(args, 2);
            LibHelper.EnsureSymbol(args, 0);

            var arr = args.ToArray();
            var sym = arr[0];
            var value = arr[1];
            var result = scope.Eval(value);

            if (scope.IsDefinedHere(sym as SymbolAtom))
            {
                scope.Define(sym as SymbolAtom, result);
                return result;
            }
            throw new InvalidOperationException($"variable {sym.String()} is not defined");
        }

        public static ISExpression If(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureMinArgCount(args, 2);
            LibHelper.EnsureMaxArgCount(args, 3);
            var arr = args.ToArray();

            var test = arr[0];
            var consequent = arr[1];
            ISExpression alternate = arr.Length > 2 ? arr[2] : null;

            var result = scope.Eval(test);

            if (result == AtomHelper.True)
            {
                return scope.Eval(consequent);
            }
            else if (alternate != null)
            {
                return scope.Eval(alternate);
            }
            return AtomHelper.False;
        }

        public static ISExpression Lambda(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureMinArgCount(args, 2);
            LibHelper.EnsureList(args, 0);

            var formals = args.First().Flatten();
            var body = args.Skip(1);
            var evalScopeParent = new Scope(scope);

            foreach (var f in formals)
            {
                if (!f.IsSymbol())
                {
                    throw new InvalidOperationException("invalid lambda parameter specification");
                }
            }

            Func<Scope, IEnumerable<ISExpression>, ISExpression> func = (s, a) =>
            {
                var evalScope = new Scope(evalScopeParent);
                var fa = formals.ToArray();
                var aa = a.ToArray();

                if (fa.Length != aa.Length)
                    throw new InvalidOperationException("invalid number of arguments to lambda");

                for (var i = 0; i < fa.Length; i++)
                {
                    evalScope.Define(fa[i] as SymbolAtom, aa[i]);
                }

                ISExpression result = AtomHelper.Nil;
                foreach (var b in body)
                {
                    result = evalScope.Eval(b);
                }

                return result;
            };
            return AtomHelper.CreateProcedure("<anonymous>", func, true);
        }
    }
}
