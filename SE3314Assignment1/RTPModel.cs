using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace SE3314Assignment1
{
    class RTPModel
    {
        //This model sending the frames with UDP
        RTPPacket _rtpPacket;
        short seqNum;
        Random timeStampMaker;
        int timeStamp;
        IPEndPoint theEndPoint;
        Socket clientSocket;
        string clientAddress;
        Stopwatch myWatch;

        public RTPModel(string address)
        {
            timeStampMaker = new Random();
            timeStamp = timeStampMaker.Next(); //Generate a random timestamp and seqence number to start
            seqNum = (short)timeStampMaker.Next();
            clientAddress = address;
            _rtpPacket = new RTPPacket(); 
            myWatch = new Stopwatch();//Needed for incrementing the timestamp
        }

        //Create the socket for sending the frames to the client
        public void createTheSocket(int portNumber)
        {
            //Port number is the port number specified in the setup command
            theEndPoint = new IPEndPoint(IPAddress.Parse(clientAddress), portNumber);

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            myWatch.Start();
        }

        //Get the last header of the last send udp packet for displaying on the form if requested
        public string getLastHeader()
        {
            return _rtpPacket.getLastHeader();
        }

        //Send the given frame to the client
        public int sendFrame(byte[] arrayToSend)
        {
            
            //Add the rtp header to the frame
                byte[] packagedFrame = _rtpPacket.packageFrame(arrayToSend, timeStamp, seqNum++);
            
                TimeSpan ts = myWatch.Elapsed;
            //Adjust the timestamp
                timeStamp += ts.Milliseconds;

                myWatch.Stop();
                myWatch.Start();
            
            //Send the packet to the client
                return clientSocket.SendTo(packagedFrame, theEndPoint);
            
        }
    }
}
