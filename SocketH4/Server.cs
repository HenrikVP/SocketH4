using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using HelperLib;

namespace SocketServer
{
    internal class Server
    {
        List<Package> packages;

        private IPEndPoint ipEndPoint;

        public Server(IPEndPoint ipEndPoint)
        {
            packages = new List<Package>();
            this.ipEndPoint = ipEndPoint;
        }

        public async Task StartServerAsync()
        {
            Socket listener = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(ipEndPoint);
            listener.Listen(100);

            while (true)
            {
                await Console.Out.WriteLineAsync($"Listening on {ipEndPoint}");

                Socket handler = await listener.AcceptAsync();

                while (true)
                {
                    // Receive message.
                    byte[] buffer = new byte[1024];
                    int received = await handler.ReceiveAsync(buffer);
                    string response = Encoding.UTF8.GetString(buffer, 0, received);

                    string eom = "<|EOM|>";
                    if (response.IndexOf(eom) > -1)
                    {
                        string json = response.Replace(eom, "");
                        Package? package = JsonSerializer.Deserialize<Package>(json);

                        if (package == null ) { continue; }
                        if (package.IsUpdate)
                        {
                            //TODO SEND RETURN ANSWER WITH PACKAGES
                            //WITH DATETIME AFTER UPD DATETIME
                            SendPackagesToClient(handler, package.MsgDT);
                        }
                        else
                        {
                            await Console.Out.WriteLineAsync("Got Mail" + package.Message);
                            packages.Add(package);
                            await SendAnswer(handler, "");
                        }
                        break;
                    }
                }
            }
        }

        private void SendPackagesToClient(Socket handler, DateTime msgDT)
        {
            List<Package> returnPackages = new List<Package>();
            foreach (Package package in packages)
                if (package.MsgDT >= msgDT) returnPackages.Add(package);
           
            string json = JsonSerializer.Serialize(returnPackages);
            SendAnswer(handler, json);
        }

        private static async Task SendAnswer(Socket handler, string json)
        {
            json += "<|ACK|>";
            byte[] echoBytes = Encoding.UTF8.GetBytes(json);
            await handler.SendAsync(echoBytes, 0);
            Console.WriteLine($"Socket server sent acknowledgment: \"{json}\"");
        }
    }
}