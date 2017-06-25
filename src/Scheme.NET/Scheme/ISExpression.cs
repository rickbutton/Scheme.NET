using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.NET.Scheme
{
    public interface ISExpression : IEquatable<ISExpression>
    {
        string String();
    }
}
