using System.Net;

namespace SocketClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            IPEndPoint? iPEndPoint = IpClassLibrary.Class1.GetIPEndPoint();
            if (iPEndPoint != null) { new Client(iPEndPoint); }

            if (iPEndPoint != null)
            {
                new Client(iPEndPoint);
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
