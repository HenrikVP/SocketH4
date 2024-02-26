﻿using System.Net;
using System.Net.Sockets;

namespace SocketServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            IPEndPoint? iPEndPoint = IpClassLibrary.Class1.GetIPEndPoint();
            if (iPEndPoint != null) {

                Task t = Task.Run(async () => await new Server(iPEndPoint).StartServerAsync());
                //Task result = t;
                while (true)
                {
                    Thread.Sleep(1000);
                }
            
            }
            else Console.WriteLine("No Ip EndPoint found!!!");

        }
    }
}
