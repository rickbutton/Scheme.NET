using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Scheme
{
    public class Cons : ISExpression
    {
        public ISExpression Car { get; set; }
        public ISExpression Cdr { get; set; }

        internal Cons(ISExpression car, ISExpression cdr)
        {
            Car = car;
            Cdr = cdr;
        }

        public string String()
        {
            if (this.IsList())
            {
                return $"({Car.String()} {string.Join(" ", Cdr.Flatten().Select(c => c.String()))})";
            } else
            {
                return $"({Car.String()} . {Cdr.String()})";
            }
        }

        public bool Equals(ISExpression other)
        {
            if (other is Cons)
            {
                var c = other as Cons;
                return Car.Equals(c.Car) && Cdr.Equals(c.Cdr);
            }
            return false;
        }
    }
}
