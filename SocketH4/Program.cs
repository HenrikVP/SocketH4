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
                _ = new Server().StartServerAsync(iPEndPoint);
                while (true) Thread.Sleep(1000);
            }
            else Console.WriteLine("No Ip EndPoint found!!!");
        }
    }
}