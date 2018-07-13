using System;
using System.Linq;
using System.Security.Authentication;
using Microsoft.Extensions.CommandLineUtils;

namespace Aquamon.Commands.Open
{
    public class OpenSObjectListCommand : OpenUrlCommand
    {
        public const string SOBJECT_LIST_ENDPOINT = "/lightning/o/{sobjectName}/list?filterName=Recent";
        public string SObjectName { get; }

        protected OpenSObjectListCommand(string sObject)
        {
            SObjectName = sObject;
        } 
        
        public static void Configure(CommandLineApplication command)
        {
            command.Description = "Opens the \"Recently viewed\" page for the chosen SObject in the browser.";
            command.Argument("[name]", "The name of the sobject");
            command.HelpOption("-?|-h|--help");

            command.OnExecute(() =>
            {
                var name = command.Arguments.Where(x => x.Name == "[name]").FirstOrDefault().Value;

                if (string.IsNullOrEmpty(name))
                    throw new Exception("An SObject name must be specified.");
                
                try
                {
                    new OpenSObjectListCommand(name).Run();
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
            var endpoint = SOBJECT_LIST_ENDPOINT.Replace("{sobjectName}", SObjectName);
            UrlUtilities.OpenBrowser(Client.ServiceUrl + endpoint);
        }
    }
}