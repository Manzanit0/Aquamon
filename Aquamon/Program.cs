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
            sbxManager.createSandbox("Dev Sandbox");

            Console.ReadLine();
            Console.ReadKey();
        }
    }
}
