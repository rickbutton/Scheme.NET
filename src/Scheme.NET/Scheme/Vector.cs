using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Scheme
{
    public class Vector : ISExpression
    {
        public ISExpression[] Values { get; set; }

        internal Vector(ISExpression[] values)
        {
            Values = values;
        }

        public string String()
        {
            return $"#({string.Join(" ", Values.Select(v => v.String()))})";
        }

        public bool Equals(ISExpression other)
        {
            if (other is Vector)
            {
                var v = other as Vector;
                if (Values.Count() != v.Values.Count())
                    return false;

                for (var i = 0; i < Values.Count(); i++)
                    if (Values[i] != v.Values[i])
                        return false;

                return true;
            }
            return false;
        }
    }
}
