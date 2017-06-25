using Scheme.NET.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Lib
{
    public static class Library
    {
        public static IDictionary<SymbolAtom, ISExpression> CreateBase()
        {
            var dict = new Dictionary<SymbolAtom, ISExpression>();

            AddFunction(dict, "+", Arithmetic.Plus);
            AddFunction(dict, "*", Arithmetic.Mul);
            AddFunction(dict, "-", Arithmetic.Minus);
            AddFunction(dict, "/", Arithmetic.Div);

            AddFunction(dict, "=", ArithmeticCompare.Equal);
            AddFunction(dict, "<", ArithmeticCompare.IsIncreasing);
            AddFunction(dict, ">", ArithmeticCompare.IsDecreasing);
            AddFunction(dict, "<=", ArithmeticCompare.IsNonDecreasing);
            AddFunction(dict, ">=", ArithmeticCompare.IsNonIncreasing);

            AddFunction(dict, "min", Arithmetic.Min);
            AddFunction(dict, "max", Arithmetic.Max);
            AddFunction(dict, "abs", Arithmetic.Abs);
            AddFunction(dict, "quotient", Arithmetic.Quotient);
            AddFunction(dict, "remainder", Arithmetic.Remainder);
            AddFunction(dict, "modulo", Arithmetic.Modulo);
            AddFunction(dict, "gcd", Arithmetic.Gcd);
            AddFunction(dict, "lcm", Arithmetic.Lcm);
            AddFunction(dict, "floor", Arithmetic.Floor);
            AddFunction(dict, "ceiling", Arithmetic.Ceiling);
            AddFunction(dict, "truncate", Arithmetic.Truncate);
            AddFunction(dict, "round", Arithmetic.Round);

            AddFunction(dict, "boolean?", Types.IsBoolean);
            AddFunction(dict, "pair?", Types.IsPair);
            AddFunction(dict, "symbol?", Types.IsSymbol);
            AddFunction(dict, "number?", Types.IsNumber);
            AddFunction(dict, "integer?", Types.IsInteger);
            AddFunction(dict, "char?", Types.IsChar);
            AddFunction(dict, "string?", Types.IsString);
            AddFunction(dict, "procedure?", Types.IsProcedure);
            AddFunction(dict, "null?", Types.IsNil);

            AddFunction(dict, "zero?", Arithmetic.Zero);
            AddFunction(dict, "positive?", Arithmetic.Positive);
            AddFunction(dict, "negative?", Arithmetic.Negative);
            AddFunction(dict, "odd?", Arithmetic.Odd);
            AddFunction(dict, "even?", Arithmetic.Even);

            AddFunction(dict, "eqv?", Types.Eqv);
            AddFunction(dict, "eq?", Types.Eqv);
            AddFunction(dict, "equal?", Types.Equal);

            AddFunction(dict, "cons", Pairs.Cons);
            AddFunction(dict, "car", Pairs.Car);
            AddFunction(dict, "cdr", Pairs.Cdr);
            AddFunction(dict, "set-car!", Pairs.SetCar);
            AddFunction(dict, "set-cdr!", Pairs.SetCdr);

            AddPrimitive(dict, "quote", Primitives.Quote);

            return dict;
        }

        private static void AddFunction(IDictionary<SymbolAtom, ISExpression> d, string name, Func<Scope, IEnumerable<ISExpression>, ISExpression> p)
        {
            d[AtomHelper.SymbolFromString(name)] = AtomHelper.CreateProcedure(name, p, false);
        }

        private static void AddPrimitive(IDictionary<SymbolAtom, ISExpression> d, string name, Func<Scope, IEnumerable<ISExpression>, ISExpression> p)
        {
            d[AtomHelper.SymbolFromString(name)] = AtomHelper.CreateProcedure(name, p, true);
        }
    }
}
