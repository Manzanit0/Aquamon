using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Aquamon.ToolingAPI
{
    public class TestRunner
    {
        public const string RUN_TESTS_ENDPOINT = "/services/data/v42.0/tooling/runLocalTestsAsynchronous";

        private ForceClient ForceClient { get; set; }

        public TestRunner(ForceClient client)
        {
            ForceClient = client;
        }
        
        public void runLocalTests()
        {
            using (HttpClient client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(ForceClient.ServiceUrl + RUN_TESTS_ENDPOINT));
                request.Headers.Add("Authorization", "Bearer " + ForceClient.AuthToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("X-PrettyPrint", "1");

                var stringPayload = @"{""testLevel"":""RunLocalTests""}";
                request.Content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                var response = client.SendAsync(request).Result;

                Console.WriteLine("Run tests response :: " + response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}