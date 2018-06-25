using System;
using System.Security.Authentication;
using Microsoft.Extensions.CommandLineUtils;
using ToolingClient.Sandboxes;

namespace Aquamon.Commands.Sandboxes
{
    public class RefreshSandboxCommand : SandboxCommand
    {
        public RefreshSandboxCommand(SandboxInfo sbxInfo)
        {
            SbxInfo = sbxInfo;
        }

        public static void Configure(CommandLineApplication command)
        {
            command.Description = "Refreshes a sandbox.";
            command.HelpOption("-?|-h|--help");

            command.OnExecute(() =>
            {
                var sbxInfo = CreateSandboxInfo(command);

                try
                {
                    new RefreshSandboxCommand(sbxInfo).Run();
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
            SbxManager.RefreshSandbox(SbxInfo);
        }
    }
}