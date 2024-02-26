using System.Net.Sockets;
using System.Net;
using System.Text;

namespace SocketClient
{
    internal class Client
    {
        private IPEndPoint iPEndPoint;

        public Client(IPEndPoint iPEndPoint)
        {
            this.iPEndPoint = iPEndPoint;
            while (true)
            {
                string msg = CreateMessage();
                _ = StartClientAsync(msg + "<|EOM|>");
            }
        }

        public async Task StartClientAsync(string message)
        {
            Socket client = new(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                await client.ConnectAsync(iPEndPoint);
                while (true)
                {
                    //string message = "Hi friends 👋!<|EOM|>";
                    //string message = CreateMessage() + "<|EOM|>";
                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    _ = await client.SendAsync(messageBytes, SocketFlags.None);
                    Console.WriteLine($"Socket client sent message: \"{message}\"");

                    // Receive ack.
                    byte[] buffer = new byte[1_024];
                    int received = await client.ReceiveAsync(buffer, SocketFlags.None);
                    string response = Encoding.UTF8.GetString(buffer, 0, received);
                    if (response == "<|ACK|>")
                    {
                        Console.WriteLine(
                            $"Socket client received acknowledgment: \"{response}\"");
                        break;
                    }
                    // Sample output:
                    //     Socket client sent message: "Hi friends 👋!<|EOM|>"
                    //     Socket client received acknowledgment: "<|ACK|>"
                }

                client.Shutdown(SocketShutdown.Both);
        }

        private string? CreateMessage()
        {
            Console.Write("Msg:");
            return Console.ReadLine();
        }
    }
}
