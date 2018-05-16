using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ToolingClient
{
    public class ForceClient
    {
        public string AuthToken { get; set; }
        public string ServiceUrl { get; set; }

        public void Login(LoginInfo info)
        {
            HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"client_id", info.ClientId},
                    {"client_secret", info.ClientSecret},
                    {"username", info.Username},
                    {"password", info.Password + info.SecurityToken}
                }
            );

            using (var client = new HttpClient())
            {
                var message = client.PostAsync(new Uri("https://login.salesforce.com/services/oauth2/token"), content)
                    .Result;

                var responseString = message.Content.ReadAsStringAsync().Result;

                var obj = JObject.Parse(responseString);
                AuthToken = (string) obj["access_token"];
                ServiceUrl = (string) obj["instance_url"];

                Console.WriteLine($"\n:: Logged in {ServiceUrl} ::");
            }
        }

        public HttpResponseMessage Get(string endpoint)
        {
            return Callout(HttpMethod.Get, endpoint, "{}");
        }

        public HttpResponseMessage Post(string endpoint, string payload)
        {
            return Callout(HttpMethod.Post, endpoint, payload);
        }

        public HttpResponseMessage Patch(string endpoint, string payload)
        {
            return Callout(new HttpMethod("PATCH"), endpoint, payload);
        }

        private HttpResponseMessage Callout(HttpMethod method, string endpoint, string payload)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(method, new Uri(ServiceUrl + endpoint));

                request.Headers.Add("Authorization", "Bearer " + AuthToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("X-PrettyPrint", "1");

                request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

                return client.SendAsync(request).Result;
            }
        }
    }
}