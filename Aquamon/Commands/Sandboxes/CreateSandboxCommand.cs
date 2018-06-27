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
            ConfigureOptions(command);

            command.OnExecute(() =>
            {
                try
                {
                    var sbxInfo = CreateSandboxInfo(command);
                    new CreateSandboxCommand(sbxInfo).Run();
                }
                catch (AuthenticationException)
                {
                    Console.WriteLine(":: Login attempt unsuccessful - Please verify your credentials ::");
                }
                catch (Exception e)
                {
                    Console.WriteLine($":: {e.Message} ::");
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