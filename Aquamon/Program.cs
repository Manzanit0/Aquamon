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
                var descriptionOption = command.Option("-d|--description", "Sandbox description", CommandOptionType.SingleValue);
                var apexOption = command.Option("-a|--apex", "Apex class to execute post-copy", CommandOptionType.SingleValue);

                command.OnExecute(() =>
                {
                    if(nameArgument.Value == null)
                    {
                        throw new ArgumentNullException("A name for the sandbox must be specified.");
                    }

                    SandboxInfo sbxInfo = new SandboxInfo() { SandboxName = nameArgument.Value, Description = descriptionOption.Value(), ApexClassId = apexOption.Value() };
                    
                    Console.WriteLine($"\n:: Sandbox {sbxInfo.SandboxName} is going to be created ::");
                    _sbxManager.CreateSandbox(sbxInfo);

                    return 0;
                });
            });

            app.Command("refresh", (command) =>
            {
                command.Description = "Refreshes a sandbox.";
                command.HelpOption("-?|-h|--help");

                var nameArgument = command.Argument("[name]", "The name of the sandbox");
                var descriptionOption = command.Option("-d|--description", "Sandbox description", CommandOptionType.SingleValue);
                var apexOption = command.Option("-a|--apex", "Apex class to execute post-copy", CommandOptionType.SingleValue);

                command.OnExecute(() =>
                {
                    if(nameArgument.Value == null)
                    {
                        throw new ArgumentNullException("A name for the sandbox must be specified.");
                    }

                    SandboxInfo sbxInfo = new SandboxInfo() { SandboxName = nameArgument.Value, Description = descriptionOption.Value(), ApexClassId = apexOption.Value()};

                    Console.WriteLine($"\n:: Sandbox {sbxInfo.SandboxName} is going to refreshed ::");
                    _sbxManager.RefreshSandbox(sbxInfo);

                    return 0;
                });
            });

            app.Command("status", (command) =>
            {
                command.Description = "Checks the status of a sandbox.";
                command.HelpOption("-?|-h|--help");

                var nameArgument = command.Argument("[name]", "The name of the sandbox");
                var statusArgument = command.Argument("[status]", "The status to check of the sandbox");

                command.OnExecute(() =>
                {
                    var status = statusArgument.Value != null ? statusArgument.Value : "Completed";
                    var name = nameArgument.Value != null ? nameArgument.Value : "ApiSbx";

                    SandboxInfo sbxInfo = new SandboxInfo() { SandboxName = name, Status = status, Description = "Description"};
                    var response = _sbxManager.GetSandboxStatus(sbxInfo);

                    Console.WriteLine($"\n:: Status :: \n {response}");

                    return 0;
                });
            });

            app.Execute(args);
        }
    }
}
