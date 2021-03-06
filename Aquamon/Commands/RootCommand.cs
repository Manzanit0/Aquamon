using Aquamon.Commands.Configuration;
using Aquamon.Commands.Open;
using Aquamon.Commands.Sandboxes;
using Microsoft.Extensions.CommandLineUtils;

namespace Aquamon.Commands
{
    public class RootCommand : ICommand
    {
        private readonly CommandLineApplication _app;

        private RootCommand(CommandLineApplication app)
        {
            _app = app;
        }

        public void Run()
        {
            _app.ShowHelp();
        }

        public static void Configure(CommandLineApplication app)
        {
            app.Name = "aquamon";
            app.HelpOption("-?|-h|--help");

            // Register commands
            app.Command("create", CreateSandboxCommand.Configure);
            app.Command("refresh", RefreshSandboxCommand.Configure);
            app.Command("status", CheckSandboxStatusCommand.Configure);
            app.Command("config", ConfigCommand.Configure);
            app.Command("open", OpenInstanceCommand.Configure);
            app.Command("open:setup", OpenSetupCommand.Configure);
            app.Command("open:console", OpenConsoleCommand.Configure);
            app.Command("open:sobject", OpenSObjectListCommand.Configure);

            app.OnExecute(() =>
            {
                new RootCommand(app).Run();
                return 0;
            });
        }
    }
}