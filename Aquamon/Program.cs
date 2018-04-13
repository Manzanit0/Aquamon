using System;

namespace Aquamon
{
    class Program
    {
        static void Main(string[] args)
        {
            ForceClient client = new ForceClient();
            client.login();

            SandboxManager sbxManager = new SandboxManager(client);
            sbxManager.createSandbox("apitest1", "Sandbox for development.");

            Console.ReadLine();
            Console.ReadKey();
        }
    }
}
