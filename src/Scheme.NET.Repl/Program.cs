using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Scheme.NET.Scheme;
using Scheme.NET.VirtualMachine.Compiler;
using Scheme.NET.VirtualMachine;
using Scheme.NET.VirtualMachine.Natives;
using Scheme.NET.Parser;
using System.IO;

namespace Scheme.NET.Repl
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var ns = typeof(Program).Assembly.GetName().Name;
            var replStream = typeof(Program).Assembly.GetManifestResourceStream(ns + ".repl.ss");

            string repl;
            using (var reader = new StreamReader(replStream))
            {
                repl = reader.ReadToEnd();
            }

            var vm = new SchemeVM();
            var inst = SchemeCompiler.Compile(vm, repl);
            vm.Execute(inst);
        }
    }
}
