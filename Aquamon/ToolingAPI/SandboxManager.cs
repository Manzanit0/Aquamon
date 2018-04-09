using System;

namespace Aquamon
{
    public class SandboxManager
    {
        public const string SANDBOX_INFO_ENDPOINT = "/services/data/v42.0/tooling/sobjects/SandboxInfo/";
        public const string SANDBOX_PROCESS_ENDPOINT = "/services/data/v42.0/tooling/sobjects/SandboxProcess/";

        private ForceClient client { get; set; }

        public SandboxManager(ForceClient client)
        {
            this.client = client;
        }

        public void createSandbox(string name)
        {
            throw new NotImplementedException();
        }

        public void refreshSandbox(string name)
        {
            throw new NotImplementedException();
        }
    }
}