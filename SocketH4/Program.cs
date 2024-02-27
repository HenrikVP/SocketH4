using System.Net;
using HelperLib;

namespace SocketServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint? iPEndPoint = IpHelper.GetIPEndPoint();
            if (iPEndPoint != null)
            {
                Task t = Task.Run(async () => await new Server(iPEndPoint).StartServerAsync());
                while (true) Thread.Sleep(1000);
            }
            else Console.WriteLine("No Ip EndPoint found!!!");
        }
    }
}