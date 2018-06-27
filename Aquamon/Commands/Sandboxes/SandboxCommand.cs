using System;
using System.Linq;
using AutoMapper;
using Configuration;
using Configuration.Configurations;
using Microsoft.Extensions.CommandLineUtils;
using ToolingClient;
using ToolingClient.Sandboxes;

namespace Aquamon.Commands.Sandboxes
{
    public abstract class SandboxCommand : ICommand
    {
        protected SandboxCommand()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<LoginConfiguration, LoginInfo>());
            var mapper = config.CreateMapper();
            
            var configurator = new Configurator<SalesforceConfiguration>();           
            var loginInfo = mapper.Map<LoginInfo>(configurator.Configuration.LoginInfo);
            
            Client = new ForceClient();
            Client.Login(loginInfo);

            SbxManager = new SandboxManager(Client);
        }

        protected SandboxInfo SbxInfo { get; set; }
        protected static SandboxManager SbxManager { get; set; }
        private static ForceClient Client { get; set; }
        
        public abstract void Run();

        protected static void ConfigureOptions(CommandLineApplication command)
        {
            command.HelpOption("-?|-h|--help");
            command.Argument("[name]", "The name of the sandbox");
            command.Option("-d|--description", "Sandbox description", CommandOptionType.SingleValue);
            command.Option("-a|--apex", "Apex class to execute post-copy", CommandOptionType.SingleValue);
        }
        
        protected static SandboxInfo CreateSandboxInfo()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<SandboxConfiguration, SandboxInfo>());
            var mapper = config.CreateMapper();
            
            var configurator = new Configurator<SalesforceConfiguration>(); // TODO - verify if it's possible to get just the SandboxInfo config without traversing the whole thing.
            var sandboxConfig = configurator.Configuration.SandboxInfo;           
            return mapper.Map<SandboxInfo>(sandboxConfig);
        }

        protected static SandboxInfo CreateSandboxInfo(CommandLineApplication command)
        {
            var sbxInfo = CreateSandboxInfo();

            var name = command.Arguments.Where(x => x.Name == "[name]").FirstOrDefault().Value;
            
            if (string.IsNullOrEmpty(name))
                throw new Exception("A name for the sandbox must be specified.");

            sbxInfo.SandboxName = name;
            sbxInfo.Description = command.Options.Where(x => x.ShortName == "d").FirstOrDefault().ValueName ?? "Default Description.";
            sbxInfo.ApexClassId = command.Options.Where(x => x.ShortName == "a").FirstOrDefault().ValueName ?? sbxInfo.ApexClassId; // Override if forced via console parameter.
            
            return sbxInfo;
        }
    }
}