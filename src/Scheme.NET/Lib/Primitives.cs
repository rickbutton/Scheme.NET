using Scheme.NET.Eval;
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

        private enum LambdaType { List, Variable, ManyVar }

        public static ISExpression Lambda(Scope scope, IEnumerable<ISExpression> args)
        {
            LibHelper.EnsureMinArgCount(args, 2);

            var formals = args.First();
            var body = args.Skip(1);
            var evalScopeParent = new Scope(scope);
            LambdaType lambdaType;

            if (formals.IsList())
            {
                var flattened = formals.Flatten();
                foreach (var f in flattened)
                {
                    if (!f.IsSymbol())
                    {
                        throw new InvalidOperationException("invalid lambda parameter specification");
                    }
                }
                lambdaType = LambdaType.List;
            }
            else if (formals.IsSymbol())
            {
                lambdaType = LambdaType.Variable;
            }
            else if (IsManyVarList(formals))
            {
                lambdaType = LambdaType.ManyVar;
            }
            else
                throw new InvalidOperationException("invalid lambda parameter specification");

            Func<Scope, IEnumerable<ISExpression>, ISExpression> func = (s, a) =>
            {
                var evalScope = new Scope(evalScopeParent);

                if (lambdaType == LambdaType.List)
                {
                    var flattened = formals.Flatten();
                    var fa = flattened.ToArray();
                    var aa = a.ToArray();

                    if (fa.Length != aa.Length)
                        throw new InvalidOperationException("invalid number of arguments to lambda");

                    for (var i = 0; i < fa.Length; i++)
                    {
                        evalScope.Define(fa[i] as SymbolAtom, aa[i]);
                    }
                }
                else if (lambdaType == LambdaType.Variable)
                {
                    evalScope.Define(formals as SymbolAtom, a.Unflatten());
                }
                else if (lambdaType == LambdaType.ManyVar)
                {
                    var fa = formals.FlattenImproper().ToArray();
                    var aa = a.ToArray();

                    if (aa.Length < fa.Length - 1)
                        throw new InvalidOperationException("invalid number of arguments to lambda");

                    for (var i = 0; i < fa.Length - 1; i++)
                    {
                        evalScope.Define(fa[i] as SymbolAtom, aa[i]);
                    }
                    evalScope.Define(fa[fa.Length - 1] as SymbolAtom, aa.Skip(fa.Length - 1).Unflatten());
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
