using System;
using System.Linq;
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
            ConfigureOptions(command);
            command.Argument("[status]", "The status to check of the sandbox");

            command.OnExecute(() =>
            {             
                try
                {
                    var sbxInfo = CreateSandboxInfo(command);
                    sbxInfo.Status = command.Arguments.Where(x => x.Name == "[status]").FirstOrDefault().Value ?? "Completed";
                    new CheckSandboxStatusCommand(sbxInfo).Run();
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
            var response = SbxManager.GetSandboxStatus(SbxInfo);
            Console.WriteLine($"\n:: Status :: \n {response}");
        }
    }
}