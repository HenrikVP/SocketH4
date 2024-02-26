using System.Net;

namespace SocketServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint? iPEndPoint = IpClassLibrary.Class1.GetIPEndPoint();
            if (iPEndPoint != null)
            {
                Task t = Task.Run(async () => await new Server(iPEndPoint).StartServerAsync());
                while (true) Thread.Sleep(1000);
            }
            else Console.WriteLine("No Ip EndPoint found!!!");
        }
    }
}