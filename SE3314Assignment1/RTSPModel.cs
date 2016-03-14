using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SE3314Assignment1
{
    class RTSPModel
    {
        static byte[] buffer = new byte[1024];
        static IPEndPoint myEndPoint;
        static Socket serverSocket;
        
        public bool createSocket(string ipaddress, int port)
        {
            //Create the socket for listening for incomming connections
            try {
                myEndPoint = new IPEndPoint(IPAddress.Parse(ipaddress), port);
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(myEndPoint); 
                serverSocket.Listen(int.MaxValue);
                return true;
            }
            catch(SocketException e)
            {
                return false;
            }
        }


        public Socket startListening(string ipaddress, int port)
        {
            //Wait for people to connect to the server and return the created socket
            return serverSocket.Accept();
            
        }
    }
}
