using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Configuration
{
    public class Configurator<T> where T : class, new()
    {
        public T Configuration { get; }
        
        public Configurator()
        {
            // Get actual path from the assembly, in case we add the app to Enviroment Variables.
            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            
            // TODO - Make this regex OS friendly.
            //var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*"); // Windows
            var appPathMatcher = new Regex(@"(?<!file\/)\/+[\S\s]*"); // MacOs 
            
            var appRoot = appPathMatcher.Match(exePath).Value;

            var builder = new ConfigurationBuilder()
                .SetBasePath(appRoot)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            var config  = builder.Build();
            
            Configuration = new T();
            config.Bind(Configuration.GetType().Name, Configuration);    
        }

        public void Save() => throw new NotImplementedException();
    }
}