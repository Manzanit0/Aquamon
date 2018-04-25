using System;
using Microsoft.Extensions.CommandLineUtils;

namespace Aquamon {

    public class RefreshSandboxCommand : SandboxCommand
    {
        public static void Configure(CommandLineApplication command)
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

                SandboxInfo sbxInfo = new SandboxInfo() { SandboxName = nameArgument.Value, Description = descriptionOption.Value(), ApexClassId = apexOption.Value() };
                (new RefreshSandboxCommand(sbxInfo)).Run();
                return 0;
            });
        }

        public RefreshSandboxCommand(SandboxInfo sbxInfo) : base()
        {
            _sbxInfo = sbxInfo;
        }

        public override void Run() 
        {
            Console.WriteLine($"\n:: Sandbox {_sbxInfo.SandboxName} is going to refreshed ::");
            _sbxManager.RefreshSandbox(_sbxInfo);
        }
    }
}