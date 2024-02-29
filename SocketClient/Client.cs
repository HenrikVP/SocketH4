using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Timers;
using HelperLib;

namespace SocketClient
{
    internal class Client
    {
        private readonly IPEndPoint iPEndPoint;
        private int latestId = 0;

        public Client(IPEndPoint iPEndPoint, string user)
        {
            this.iPEndPoint = iPEndPoint;

            System.Timers.Timer timer = new();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 100;
            timer.Enabled = true;

            while (true)
            {
                Package p = new() { Message = Console.ReadLine(), User = user };
                _ = StartClientAsync(JsonSerializer.Serialize(p));
            }
        }

        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            _ = StartClientAsync(JsonSerializer.Serialize(new Package(latestId, true)));
        }

        public async Task StartClientAsync(string message)
        {
            Socket client = new(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            await client.ConnectAsync(iPEndPoint);
            while (true)
            {
                //Send
                byte[] messageBytes = Encoding.UTF8.GetBytes(message + "<|EOM|>");
                await client.SendAsync(messageBytes);
                //Recieve
                byte[] buffer = new byte[1024];
                int received = await client.ReceiveAsync(buffer, SocketFlags.None);
                string response = Encoding.UTF8.GetString(buffer, 0, received);

                if (response.Contains("<|ACK|>"))
                {
                    ProcessAnswer(response.Replace("<|ACK|>",""));
                    break;
                }
            }

            client.Shutdown(SocketShutdown.Both);
        }

        void ProcessAnswer(string response)
        {
            if (response != "[]" && response != "")
            {
                List<Package>? packages = JsonSerializer.Deserialize<List<Package>>(response);
                foreach (var p in packages)
                {
                    Console.WriteLine($"{p.Id} {p.User}:{p.Message}");
                    latestId = p.Id + 1;
                }
            }
        }
    }
}
