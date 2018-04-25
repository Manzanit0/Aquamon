using System;
using Microsoft.Extensions.CommandLineUtils;

namespace Aquamon {

    public class CreateSandboxCommand : SandboxCommand
    {
        public static void Configure(CommandLineApplication command)
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
                (new CreateSandboxCommand(sbxInfo)).Run();
                return 0;
            });
        }

        public CreateSandboxCommand(SandboxInfo sbxInfo) : base()
        {
            _sbxInfo = sbxInfo;
        }

        public override void Run() 
        {
            Console.WriteLine($"\n:: Sandbox {_sbxInfo.SandboxName} is going to be created ::");
            _sbxManager.CreateSandbox(_sbxInfo);
        }
    }
}