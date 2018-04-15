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

        public void CreateSandbox(string name, string description)
        {
            var payload = $@"
            {{
                ""AutoActivate"": true,
                ""SandboxName"": ""{name}"",
                ""Description"": ""{description}"",
                ""LicenseType"": ""DEVELOPER""
            }}";

            var response = Client.Post(SANDBOX_INFO_ENDPOINT, payload);
            var stringResponse = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine("\n:: Sandbox created :: " + stringResponse);
        }

        public void RefreshSandbox(string name)
        {
            var response = Client.Get(QUERY_SANDBOX_INFO.Replace("{name}", name));
            var stringResponse = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine("\n:: Queried Sandbox :: " + stringResponse);

            dynamic stuff = JObject.Parse(stringResponse);
            string id = stuff.records[0].Id;

            var payload = $@"
            {{
                ""AutoActivate"": true,
                ""LicenseType"": ""DEVELOPER""
            }}";

            response = Client.Patch(SANDBOX_INFO_ENDPOINT + $"{id}/", payload);
            stringResponse = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine("\n:: Refreshed Sandbox :: " + stringResponse);
        }

        public string GetSandboxStatus(string name)
        {
            var response = Client.Get(QUERY_SANDBOX_PROCESS.Replace("{name}", name));
            var stringResponse = response.Content.ReadAsStringAsync().Result;

            dynamic stuff = JObject.Parse(stringResponse);
            foreach(var record in stuff.records)
            {
                if(record.Status == "Processing")
                {
                    return Client.Get("" + record.attributes.url).Content.ReadAsStringAsync().Result;
                }
            }

            return @"{""Message"": ""There is sandbox In Progress with the given name""}";
        }
    }
}