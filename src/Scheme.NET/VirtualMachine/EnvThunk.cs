using Scheme.NET.Scheme;

namespace Scheme.NET.VirtualMachine
{
    public partial class Environment : IEnvironment
    {
        public class EnvThunk
        {
            public ISExpression Val { get; set; }
        }
    }
}
