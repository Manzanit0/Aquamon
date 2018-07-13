using System;
using System.Security.Authentication;
using Microsoft.Extensions.CommandLineUtils;

namespace Aquamon.Commands.Open
{
    public class OpenConsoleCommand : OpenUrlCommand
    {
        public const string CONSOLE_ENDPOINT = "/_ui/common/apex/debug/ApexCSIPage";
        
        protected OpenConsoleCommand() {} 
        
        public static void Configure(CommandLineApplication command)
        {
            command.Description = "Opens the Developer Console in the browser.";
            command.HelpOption("-?|-h|--help");

            command.OnExecute(() =>
            {
                try
                {
                    new OpenConsoleCommand().Run();
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
            UrlUtilities.OpenBrowser(Client.ServiceUrl + CONSOLE_ENDPOINT);
        }
    }
}