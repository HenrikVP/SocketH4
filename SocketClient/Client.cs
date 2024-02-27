using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Timers;

namespace SocketClient
{
    internal class Client
    {
        private IPEndPoint iPEndPoint;

        DateTime LatestUpdate;

        public Client(IPEndPoint iPEndPoint, string user)
        {
            this.iPEndPoint = iPEndPoint;

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 10000;
            timer.Enabled = true;

            while (true)
            {
                HelperLib.Package p = new HelperLib.Package() { Message = CreateMessage(), MsgDT = DateTime.Now, User = user };
                _ = StartClientAsync(JsonSerializer.Serialize(p));
            }
        }

        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            HelperLib.Package package = new HelperLib.Package();
            package.MsgDT = LatestUpdate;
            package.IsUpdate = true;
            _ = StartClientAsync(JsonSerializer.Serialize(package));
            LatestUpdate = DateTime.Now;
        }

        public async Task StartClientAsync(string message)
        {
            Socket client = new(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            await client.ConnectAsync(iPEndPoint);
            while (true)
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(message + "<|EOM|>");
                _ = await client.SendAsync(messageBytes);

                // Receive ack.
                byte[] buffer = new byte[1024];
                int received = await client.ReceiveAsync(buffer, SocketFlags.None);
                string response = Encoding.UTF8.GetString(buffer, 0, received);

                if (response.Contains("<|ACK|>"))
                {
                    ProcessAnswer(response.Replace("<|ACK|>",""));
                    //Console.WriteLine( $"Socket client received acknowledgment: \"{response}\"");
                    break;
                }
            }

            client.Shutdown(SocketShutdown.Both);
        }

        void ProcessAnswer(string response)
        {
            if (response != "[]" && response != "")
            {
                List<HelperLib.Package> packages = 
                    JsonSerializer.Deserialize<List<HelperLib.Package>>(response);
                foreach (var p in packages)
                {
                    Console.WriteLine($"{p.User}:{p.Message}");
                }
            }
        }

        private string? CreateMessage()
        {
            Console.Write("Msg:");
            return Console.ReadLine();
        }
    }
}
