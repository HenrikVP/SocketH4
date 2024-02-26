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
        }

        public async Task StartClientAsync()
        {
            using Socket client = new(iPEndPoint.AddressFamily,SocketType.Stream,ProtocolType.Tcp);

            await client.ConnectAsync(iPEndPoint);
            while (true)
            {
                // Send message.
                var message = "Hi friends 👋!<|EOM|>";
                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = await client.SendAsync(messageBytes, SocketFlags.None);
                Console.WriteLine($"Socket client sent message: \"{message}\"");

                // Receive ack.
                var buffer = new byte[1_024];
                var received = await client.ReceiveAsync(buffer, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);
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

        static IPEndPoint? GetIPEndPoint()
        {
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName(), AddressFamily.InterNetwork);
            IPAddress[] addressList = ipHostEntry.AddressList;
            //TODO Pick IP Address more intelligenteliently
            if (addressList == null || addressList[0] == null) return null;
            return new IPEndPoint(addressList[0], 8090);
        }

    }
}
