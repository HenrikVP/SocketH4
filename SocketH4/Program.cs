using System.Net;
using System.Net.Sockets;

namespace SocketH4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            IPEndPoint? iPEndPoint = GetIPEndPoint();
            if (iPEndPoint != null) { new SocketServer(iPEndPoint); }
            Console.WriteLine("No Ip EndPoint found!!!");
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
