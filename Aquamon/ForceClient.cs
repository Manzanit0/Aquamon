using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;

namespace Aquamon
{
    public class ForceClient
    {
        public string AuthToken { get; set; }
        public string ServiceUrl { get; set; }

        public void login()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

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

                Console.WriteLine("Logged in :: " + ServiceUrl);
            }
        }
    }
}