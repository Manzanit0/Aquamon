using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace Aquamon
{
    public class ForceClient
    {
        public string AuthToken { get; set; }
        public string ServiceUrl { get; set; }

        public void login()
        {
            // Get actual path from the assembly, in case we add the app to Enviroment Variables.
            var exePath =   Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;

            var builder = new ConfigurationBuilder()
                .SetBasePath(appRoot)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            var Configuration = builder.Build();

            HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"grant_type","password"},
                    {"client_id", Configuration["credentials:0:ConsumerKey"]},
                    {"client_secret", Configuration["credentials:0:ConsumerSecret"]},
                    {"username", Configuration["credentials:0:Username"]},
                    {"password", Configuration["credentials:0:Password"] + Configuration["credentials:0:SecurityToken"]}
                }
            );

            using (HttpClient client = new HttpClient())
            {
                var message = client.PostAsync(new Uri("https://login.salesforce.com/services/oauth2/token"), content).Result;

                var responseString = message.Content.ReadAsStringAsync().Result;

                JObject obj = JObject.Parse(responseString);
                AuthToken = (string)obj["access_token"];
                ServiceUrl = (string)obj["instance_url"];

                Console.WriteLine(":: Logged in :: " + ServiceUrl);
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
            using (HttpClient client = new HttpClient())
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