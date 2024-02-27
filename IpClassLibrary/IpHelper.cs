using System.Net.Sockets;
using System.Net;

namespace HelperLib
{
    public class IpHelper
    {
        public static IPEndPoint? GetIPEndPoint()
        {
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName(), AddressFamily.InterNetwork);
            IPAddress[] addressList = ipHostEntry.AddressList;
            //TODO Pick IP Address more intelligenteliently
            if (addressList == null || addressList[0] == null) return null;
            return new IPEndPoint(addressList[0], 8090);
        }
    }
}
