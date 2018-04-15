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
            var result = sbxManager.GetSandboxStatus("apitest1");
            Console.WriteLine($":: Sandbox Status :: {result}");

            Console.ReadLine();
            Console.ReadKey();
        }
    }
}
