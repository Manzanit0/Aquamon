using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Aquamon
{
    public class SandboxManager
    {
        public const string SANDBOX_INFO_ENDPOINT = "/services/data/v42.0/tooling/sobjects/SandboxInfo/";
        public const string SANDBOX_PROCESS_ENDPOINT = "/services/data/v42.0/tooling/sobjects/SandboxProcess/";
        public const string QUERY_SANDBOX_INFO = "/services/data/v42.0/tooling/query?q=SELECT+Id,SandboxName+FROM+SandboxInfo+WHERE+SandboxName+in+('{name}')";
        public const string QUERY_SANDBOX_PROCESS = "/services/data/v42.0/tooling/query?q=SELECT+Id,Status+FROM+SandboxProcess+WHERE+SandboxName+in+('{name}')";
        
        private ForceClient Client { get; set; }

        public SandboxManager(ForceClient client)
        {
            this.Client = client;
        }

        public void CreateSandbox(SandboxInfo info)
        {
            var response = Client.Post(SANDBOX_INFO_ENDPOINT, info.ToJSON());
            var stringResponse = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine("\n:: Sandbox created :: " + stringResponse);
        }

        public void RefreshSandbox(SandboxInfo info)
        {
            var response = Client.Get(QUERY_SANDBOX_INFO.Replace("{name}", info.SandboxName));
            var stringResponse = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine("\n:: Queried Sandbox :: " + stringResponse);

            dynamic queriedSandbox = JObject.Parse(stringResponse);
            string id = queriedSandbox.records[0].Id; // FIXME: Verify that the array is not empty.

            response = Client.Patch(SANDBOX_INFO_ENDPOINT + $"{id}/", info.ToJSON());
            stringResponse = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine("\n:: Refreshed Sandbox :: " + stringResponse);
        }

        public string GetSandboxStatus(SandboxInfo info)
        {
            var response = Client.Get(QUERY_SANDBOX_PROCESS.Replace("{name}", info.SandboxName));
            var stringResponse = response.Content.ReadAsStringAsync().Result;

            dynamic sandboxes = JObject.Parse(stringResponse);
            foreach(var record in sandboxes.records)
            {
                if(record.Status == info.Status)
                {
                    return Client.Get("" + record.attributes.url).Content.ReadAsStringAsync().Result;
                }
            }

            return $@"{{""Message"": ""A sandbox with the name {info.SandboxName} and status {info.Status} does not exist.""}}";
        }
    }
}