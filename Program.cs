using System;
using System.Diagnostics;

namespace AliveAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            var applicationUrl = MessageSubscriber.RecieveRabbitMQMessages();
            var process = new Process();
            process.StartInfo.FileName = "D:/PuppeteerSharp-Example-master/PuppeteerExample/bin/Debug/netcoreapp3.1/PuppeteerExample.exe";
            process.StartInfo.Arguments = applicationUrl;
            process.Start();
           // proc.WaitForExit();
            var exitCode = process.ExitCode;
            process.Close();
        }
    }
}
