using Microsoft.Extensions.CommandLineUtils;

namespace Aquamon {

    public class RootCommand : ICommand
    {
        public static void Configure(CommandLineApplication app) {
            app.Name = "aquamon";
            app.HelpOption("-?|-h|--help");
            
            // Register commands
            app.Command("create", CreateSandboxCommand.Configure);
            app.Command("refresh", RefreshSandboxCommand.Configure);
            app.Command("status", CheckSandboxStatusCommand.Configure);

            app.OnExecute(() => 
            {
                (new RootCommand(app)).Run();
                return 0;
            });
        }

        private readonly CommandLineApplication _app;

        public RootCommand(CommandLineApplication app) {
            _app = app;
        }

        public void Run() {
            _app.ShowHelp();
    }
    }
}