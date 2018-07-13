using AutoMapper;
using Configuration;
using Configuration.Configurations;
using ToolingClient;

namespace Aquamon.Commands.Open
{
    public abstract class OpenUrlCommand : ICommand
    {
        protected static ForceClient Client { get; set; }
        
        protected OpenUrlCommand()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<LoginConfiguration, LoginInfo>());
            var mapper = config.CreateMapper();

            var configurator = new Configurator<SalesforceConfiguration>();
            var loginInfo = mapper.Map<LoginInfo>(configurator.Configuration.LoginInfo);
            
            Client = new ForceClient();
            Client.Login(loginInfo);
        }

        public abstract void Run();
    }
}