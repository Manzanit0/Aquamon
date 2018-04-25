namespace Aquamon {

    public abstract class SandboxCommand : ICommand
    {
        protected static ForceClient _client { get; set; }
        protected static SandboxManager _sbxManager { get; set; }
        protected SandboxInfo _sbxInfo;

        public SandboxCommand()
        {
            _client = new ForceClient();
            _client.login();
            
            _sbxManager = new SandboxManager(_client);
        }

        public abstract void Run();
    }
}