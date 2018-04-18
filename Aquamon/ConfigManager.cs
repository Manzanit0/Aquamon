using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Aquamon
{
    public class ConfigManager
    {
        public static IConfiguration GetConfiguration()
        {
            // Get actual path from the assembly, in case we add the app to Enviroment Variables.
            var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*");
            var appRoot = appPathMatcher.Match(exePath).Value;

            var builder = new ConfigurationBuilder()
                .SetBasePath(appRoot)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}