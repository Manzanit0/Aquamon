using Aquamon.Commands;
using Microsoft.Extensions.CommandLineUtils;

namespace Aquamon
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            RootCommand.Configure(app);
            app.Execute(args);
        }
    }
}