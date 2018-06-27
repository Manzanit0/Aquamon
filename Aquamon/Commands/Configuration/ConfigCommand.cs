using System;
using System.Linq;
using Configuration;
using Configuration.Configurations;
using Microsoft.Extensions.CommandLineUtils;

namespace Aquamon.Commands.Configuration
{
    public class ConfigCommand : ICommand
    {
        public ConfigCommand(string configurationSection, string configurationValue)
        {
            ConfigSection = configurationSection;
            ConfigValue = configurationValue;
        }

        public string ConfigSection { get; }
        public string ConfigValue { get; }

        public void Run()
        {
            var configurator = new Configurator<SalesforceConfiguration>();
            var section = ConfigSection.Split(".");
            var block = configurator.Configuration.GetType().GetProperty(section[0])
                .GetValue(configurator.Configuration);
            var property = block.GetType().GetProperty(section[1]);

            if (!string.IsNullOrEmpty(ConfigValue))
            {
                property.SetValue(block, ConfigValue);
                configurator.Save();
                Console.WriteLine($":: Configuration updated {{{property.Name} -> {property.GetValue(block)}}} ::");
            }
            else
            {
                if (property == null) throw new Exception("The requested configuration does not exist.");
                Console.WriteLine(property.GetValue(block));
            }
        }

        public static void Configure(CommandLineApplication command)
        {
            command.Description = "Allows interaction with the application's settings.";
            command.HelpOption("-?|-h|--help");
            command.Argument("[name]", "The configuration section, i. e. \"LoginInfo.Username\"");
            command.Argument("[value]", "The value to set (Optional)");

            command.OnExecute(() =>
            {
                var name = command.Arguments.Where(x => x.Name == "[name]").FirstOrDefault().Value;
                var value = command.Arguments.Where(x => x.Name == "[value]").FirstOrDefault().Value;

                try
                {
                    if (string.IsNullOrEmpty(name))
                        throw new Exception("A config section must be specified");

                    new ConfigCommand(name, value).Run();
                }
                catch (Exception e)
                {
                    Console.WriteLine($":: {e.Message} ::");
                }

                return 0;
            });
        }
    }
}