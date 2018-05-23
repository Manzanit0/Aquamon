using System;
using System.Security.Authentication;
using Microsoft.Extensions.CommandLineUtils;
using ToolingClient.Sandboxes;

namespace Aquamon.Commands.Sandboxes
{
    public class CreateSandboxCommand : SandboxCommand
    {
        public CreateSandboxCommand(SandboxInfo sbxInfo)
        {
            SbxInfo = sbxInfo;
        }

        public static void Configure(CommandLineApplication command)
        {
            command.Description = "Creates a sandbox.";
            command.HelpOption("-?|-h|--help");

            var nameArgument = command.Argument("[name]", "The name of the sandbox");
            var descriptionOption =
                command.Option("-d|--description", "Sandbox description", CommandOptionType.SingleValue);
            var apexOption = command.Option("-a|--apex", "Apex class to execute post-copy",
                CommandOptionType.SingleValue);

            command.OnExecute(() =>
            {
                if (nameArgument.Value == null)
                    throw new ArgumentNullException("A name for the sandbox must be specified.");

                var sbxInfo = new SandboxInfo
                {
                    SandboxName = nameArgument.Value,
                    Description = descriptionOption.Value(),
                    ApexClassId = apexOption.Value()
                };
                
                try
                {
                    new CreateSandboxCommand(sbxInfo).Run();
                }
                catch (AuthenticationException)
                {
                    Console.WriteLine(":: Login attempt unsuccessful - Please verify your credentials ::");
                }
                
                return 0;
            });
        }

        public override void Run()
        {
            Console.WriteLine("\n:: Starting to process request ::");
            SbxManager.CreateSandbox(SbxInfo);
        }
    }
}