using System;
using System.Security.Authentication;
using Microsoft.Extensions.CommandLineUtils;
using ToolingClient.Sandboxes;

namespace Aquamon.Commands.Sandboxes
{
    public class CheckSandboxStatusCommand : SandboxCommand
    {
        public CheckSandboxStatusCommand(SandboxInfo sbxInfo)
        {
            SbxInfo = sbxInfo;
        }

        public static void Configure(CommandLineApplication command)
        {
            command.Description = "Checks the status of a sandbox.";
            command.HelpOption("-?|-h|--help");

            var statusArgument = command.Argument("[status]", "The status to check of the sandbox");

            command.OnExecute(() =>
            {
                var status = statusArgument.Value != null ? statusArgument.Value : "Completed";

                var sbxInfo = CreateSandboxInfo(command);
                sbxInfo.Status = status;
                
                try
                {
                    new CheckSandboxStatusCommand(sbxInfo).Run();
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
            var response = SbxManager.GetSandboxStatus(SbxInfo);
            Console.WriteLine($"\n:: Status :: \n {response}");
        }
    }
}