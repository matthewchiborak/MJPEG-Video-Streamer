using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SE3314Assignment1
{
    //This model waits on and responds to the rtsp requests
    class ClientModel
    {
        bool isPlaying;
        bool finished;
        Socket clientSocket;
        string videoName;
        int RTPPort;
        string clientAddress;
        bool teardownIssued;

        public ClientModel(Socket RTSPSocket, string clientAddress)
        {
            isPlaying = false;
            clientSocket = RTSPSocket;
            finished = false;
            videoName = "";
            RTPPort = 0;
            this.clientAddress = clientAddress;
            teardownIssued = false;
        }

        //Closes the socket sending and recieving the rtsp commands
        public void closeConnection()
        {
            clientSocket.Close();
        }

        //Get the ip address of the client who as connected so the frames can be sent to the correct location
        public string getClientAddress()
        {
            return clientAddress;
        }

        //Check if a teardown command has been issued so that the state for reading a video can be reset
        public bool getTeardownIssued()
        {
            return teardownIssued;
        }

        public void setTeardownIssued(bool want)
        {
            teardownIssued = want;
        }

        //Get the name of the video the client wants to play
        public string getVideoName()
        {
            return videoName;
        }

        public void setVideoName(string name)
        {
            videoName = name;
        }

        //Get the port number that the client has requested the video frames to be sent to
        public int getRTPPort()
        {
            return RTPPort;
        }

        public void setRTPPort(int port)
        {
            RTPPort = port;
        }

        //CHeck whether the video is playing or not
        public bool getPlayStatus()
        {
            return isPlaying;
        }

        //Check whether the video has finished playing
        public bool getFinished()
        {
            return finished;
        }

        public void setPlayStatus(bool status)
        {
            isPlaying = status;
        }

        public void setFinished(bool status)
        {
            finished = status;
        }

        //Listen for RTSP commands made by the client
        public string listenForCommand()
        {
            byte[] receivedMessage = new byte[1024];

            try
            {
                clientSocket.Receive(receivedMessage);
            }
            catch(SocketException e)
            {
                return "error";
            }

            //Convert the bytearray to a string for parsing
             return System.Text.Encoding.Default.GetString(receivedMessage);
            
            

        }//End of listen for command function

        //Send the client a responce based on the RTSP command issued by the client
        public bool sendResponse(string theResponse)
        {
             byte[] byteArrayToSend = System.Text.ASCIIEncoding.ASCII.GetBytes(theResponse); 
            try {
                clientSocket.Send(byteArrayToSend);
                return true;
            }
            catch(SocketException err)
            {
                return false;
            }
        }
    }
}
