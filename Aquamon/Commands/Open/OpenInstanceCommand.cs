using System;
using System.Security.Authentication;
using Microsoft.Extensions.CommandLineUtils;

namespace Aquamon.Commands.Open
{
    public class OpenInstanceCommand : OpenUrlCommand
    {   
        protected OpenInstanceCommand() {} 
        
        public static void Configure(CommandLineApplication command)
        {
            command.Description = "Opens the production instance in the browser.";
            command.HelpOption("-?|-h|--help");

            command.OnExecute(() =>
            {
                try
                {
                    new OpenInstanceCommand().Run();
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
            UrlUtilities.OpenBrowser(Client.ServiceUrl);
        }
    }
}