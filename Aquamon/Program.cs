using System;
using Microsoft.Extensions.CommandLineUtils;

namespace Aquamon
{
    class Program
    {
        private static ForceClient _client { get; set; }
        private static SandboxManager _sbxManager { get; set; }
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            RootCommand.Configure(app);
            app.Execute(args);
        }
    }
}
