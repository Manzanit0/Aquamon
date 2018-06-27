using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using Configuration.Configurations;
using Microsoft.Extensions.Configuration;

namespace Configuration
{
    public class Configurator<T> where T : class, new()
    {
        private string AppSettingsDirectory
        {
            get
            {
                var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                var appPathMatcher = new Regex(@"(?<=file:\\?)[^\\].+");
                return appPathMatcher.Match(exePath).Value;
            }
        }
        
        public T Configuration { get; }
        
        public Configurator()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppSettingsDirectory)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            var config  = builder.Build();
            
            Configuration = new T();
            config.Bind(Configuration.GetType().Name, Configuration);    
        }

        public void Save()
        {
            var jsonContent = string.Empty;
            
            // Serialize the SalesforceConfiguration instance
            using (var stream1 = new MemoryStream())
            {
                var ser = new DataContractJsonSerializer(typeof(SalesforceConfiguration));  
                ser.WriteObject(stream1, Configuration);
            
                stream1.Position = 0;  
                var sr = new StreamReader(stream1);
                
                jsonContent = $"{{\"{Configuration.GetType().Name}\": {sr.ReadToEnd()} }}";
            }
            
            // Overwrite the appsettings.json with current values.
            using (var file = new StreamWriter(Path.Combine(AppSettingsDirectory, "appsettings.json"), false))
            {
                file.Write(jsonContent);
            }
        }
    }
}