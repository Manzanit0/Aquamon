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
            app.Name = "aquamon";
            app.HelpOption("-?|-h|--help");

            _client = new ForceClient();
            _client.login();

            _sbxManager = new SandboxManager(_client);

            app.OnExecute(() =>
            {
                Console.WriteLine("\n:: Hello World! ::");
                return 0;
            });

            app.Command("create", (command) =>
            {
                command.Description = "Creates a sandbox.";
                command.HelpOption("-?|-h|--help");

                var nameArgument = command.Argument("[name]", "The name of the sandbox");

                command.OnExecute(() =>
                {
                    var name = nameArgument.Value != null ? nameArgument.Value : "ApiSbx";
                    Console.WriteLine($"\n:: Sandbox {name} being created ::");

                    _sbxManager.CreateSandbox(name, "Description");

                    return 0;
                });
            });

            app.Command("refresh", (command) =>
            {
                command.Description = "Refreshes a sandbox.";
                command.HelpOption("-?|-h|--help");

                var nameArgument = command.Argument("[name]", "The name of the sandbox");

                command.OnExecute(() =>
                {
                    var name = nameArgument.Value != null ? nameArgument.Value : "ApiSbx";
                    Console.WriteLine($"\n:: Sandbox {name} being refreshed ::");

                    _sbxManager.RefreshSandbox(name);

                    return 0;
                });
            });

            app.Command("status", (command) =>
            {
                command.Description = "Refreshes a sandbox.";
                command.HelpOption("-?|-h|--help");

                var nameArgument = command.Argument("[name]", "The name of the sandbox");
                var statusArgument = command.Argument("[status]", "The status of the sandbox");

                command.OnExecute(() =>
                {
                    var status = statusArgument.Value != null ? statusArgument.Value : "Completed";
                    var name = nameArgument.Value != null ? nameArgument.Value : "ApiSbx";
                    var response = _sbxManager.GetSandboxStatus(name, status);

                    Console.WriteLine($"\n:: Status :: \n {response}");

                    return 0;
                });
            });

            app.Execute(args);
        }
    }
}
