using ToolingClient;
using ToolingClient.Sandboxes;

namespace Aquamon.Commands.Sandboxes
{
    public abstract class SandboxCommand : ICommand
    {
        protected SandboxInfo SbxInfo;

        protected SandboxCommand()
        {
            var configuration = ConfigManager.GetConfiguration();
            var loginInfo = new LoginInfo
            {
                ClientId = configuration["credentials:0:ConsumerKey"],
                ClientSecret = configuration["credentials:0:ConsumerSecret"],
                Username = configuration["credentials:0:Username"],
                Password = configuration["credentials:0:Password"],
                SecurityToken = configuration["credentials:0:SecurityToken"]
            };

            Client = new ForceClient();
            Client.Login(loginInfo);

            SbxManager = new SandboxManager(Client);
        }

        private static ForceClient Client { get; set; }
        protected static SandboxManager SbxManager { get; set; }

        public abstract void Run();
    }
}