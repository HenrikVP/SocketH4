using System.Net;

namespace SocketClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string? name = null;
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Write("Name: ");
                name = Console.ReadLine();
            }

            IPEndPoint? iPEndPoint = HelperLib.IpHelper.GetIPEndPoint();

            if (iPEndPoint != null)
            {
                new Client(iPEndPoint, name);
                //var t = Task.Run(async () => await new Client(iPEndPoint));
                //var result = t;
                while (true)
                {
                    Thread.Sleep(1000);
                }

            }
            else Console.WriteLine("No Ip EndPoint found!!!");
        }
    }
}
