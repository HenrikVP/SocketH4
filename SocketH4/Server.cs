using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using HelperLib;

namespace SocketServer
{
    internal class Server
    {
        private List<Package> packages = new();
        private int idCounter;

        public async Task StartServerAsync(IPEndPoint ipEndPoint)
        {
            Console.WriteLine($"Listening on {ipEndPoint}");
            Socket listener = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(ipEndPoint);
            listener.Listen(100);

            while (true) await ServerService(listener);
        }

        private async Task ServerService(Socket listener)
        {
            Socket handler = await listener.AcceptAsync();
            while (true)
            {
                byte[] buffer = new byte[1024];
                int received = await handler.ReceiveAsync(buffer);
                string response = Encoding.UTF8.GetString(buffer, 0, received);

                if (response.Contains("<|EOM|>"))
                {
                    Package? package = JsonSerializer.Deserialize<Package>(response.Replace("<|EOM|>", ""));

                    if (package == null) break;
                    else if (package.IsUpdate) await GetPackagesToClientAsync(handler, package.Id);
                    else
                    {
                        package.Id = idCounter++;
                        packages.Add(package);
                        Console.Write("+");
                        await SendAnswerAsync(handler, "");
                    }
                    break;
                }
            }
        }

        private async Task GetPackagesToClientAsync(Socket handler, int clientId)
        {
            List<Package> returnPackages = new();
            foreach (Package package in packages)
                if (package.Id >= clientId)
                    returnPackages.Add(package);
            Console.Write(".");
            await SendAnswerAsync(handler, JsonSerializer.Serialize(returnPackages));
        }

        private async Task SendAnswerAsync(Socket handler, string json)
        {
            byte[] echoBytes = Encoding.UTF8.GetBytes(json + "<|ACK|>");
            await handler.SendAsync(echoBytes, 0);
        }
    }
}