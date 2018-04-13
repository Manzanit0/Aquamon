using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Aquamon
{
    public class SandboxManager
    {
        public const string SANDBOX_INFO_ENDPOINT = "/services/data/v42.0/tooling/sobjects/SandboxInfo/";
        public const string SANDBOX_PROCESS_ENDPOINT = "/services/data/v42.0/tooling/sobjects/SandboxProcess/";

        private ForceClient Client { get; set; }

        public SandboxManager(ForceClient client)
        {
            this.Client = client;
        }

        public void createSandbox(string name, string description)
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

            Console.WriteLine(":: CreateSandbox Response :: " + stringResponse);
        }

        public void refreshSandbox(string name)
        {
            throw new NotImplementedException();
        }
    }
}