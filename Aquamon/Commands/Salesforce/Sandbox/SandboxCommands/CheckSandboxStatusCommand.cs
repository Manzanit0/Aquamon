using System;
using Microsoft.Extensions.CommandLineUtils;

namespace Aquamon {

    public class CheckSandboxStatusCommand : SandboxCommand
    {
        public static void Configure(CommandLineApplication command)
        {
            command.Description = "Checks the status of a sandbox.";
            command.HelpOption("-?|-h|--help");

            var nameArgument = command.Argument("[name]", "The name of the sandbox");
            var statusArgument = command.Argument("[status]", "The status to check of the sandbox");

            command.OnExecute(() => 
            {
                if(nameArgument.Value == null)
                {
                    throw new ArgumentNullException("A name for the sandbox must be specified.");
                }

                var status = statusArgument.Value != null ? statusArgument.Value : "Completed";
                var name = nameArgument.Value != null ? nameArgument.Value : "ApiSbx";

                SandboxInfo sbxInfo = new SandboxInfo() { SandboxName = name, Status = status, Description = "Description"};
                (new CheckSandboxStatusCommand(sbxInfo)).Run();
                return 0;
            });
        }

        public CheckSandboxStatusCommand(SandboxInfo sbxInfo) : base()
        {
            _sbxInfo = sbxInfo;
        }

        public override void Run() 
        {
            var response = _sbxManager.GetSandboxStatus(_sbxInfo);
            Console.WriteLine($"\n:: Status :: \n {response}");
        }
    }
}