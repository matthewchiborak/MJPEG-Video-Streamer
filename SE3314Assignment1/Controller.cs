using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Timers;
using System.Diagnostics;


namespace SE3314Assignment1
{
    class Controller
    {
        Thread _RTPThread;
        Form1 _view; //the view to collect inputted information and display new information
        int currentPort; //The currently selected port number to listen on
        string ipaddress; //The currently selected IP address of the server
        static RTSPModel _rtspModel; //Model to accept new conntections
        bool killThreads; //Bool for kill the threads if needed
        Thread acceptingThread; //Thread for accepting new connections
        
        Random sessionMaker;
        //FileStream videoOne;
        //FileStream videoTwo;
        Byte[] videoOne;
        Byte[] videoTwo;

        //Delegates for adding info to the form
        Form1.formStringDelegate addToStatusDelegate;
        Form1.formStringDelegate addToRequestDelegate;


        public Controller(Form1 mainView, Form1.formStringDelegate addToStatusDelegate, Form1.formStringDelegate addToRequestDelegate)
        {
            _RTPThread = null;
            _rtspModel = new RTSPModel();
            killThreads = false;
            acceptingThread = null;
            _view = mainView;
            sessionMaker = new Random();
            //videoOne = new FileStream(".\\video1.mjpeg", FileMode.Open, FileAccess.Read);
            //videoTwo = new FileStream(".\\video2.mjpeg", FileMode.Open, FileAccess.Read);
            videoOne = File.ReadAllBytes(".\\video1.mjpeg");
            videoTwo = File.ReadAllBytes(".\\video2.mjpeg");
            addToStatusDelegate.GetType();
            addToRequestDelegate.GetType();
            this.addToStatusDelegate = addToStatusDelegate;
            this.addToRequestDelegate = addToRequestDelegate;
        }
        
        public void setCurrentPort(int port)
        {
            currentPort = port;
        }
        public void setIPAddress(string address)//View updates the current information
        {
            ipaddress = address;
        }

        //View tells the controller to start listening for clients' connections
        public void startAcceptingConnections()
        {
            acceptingThread = new Thread(RTSPListen);
            acceptingThread.IsBackground = true;
            acceptingThread.Start();
        }

        //Make the thread waiting on new clients to connect to the server to finish
        public void joinTheAcceptingThread()
        {
            acceptingThread.Join();
        }

        //Create a delegate for creating thread to listen rtp and client models
        public delegate void transmitInfoDelegate(Socket clientConnectedSocket);

        //Thread function for listening for new clients to connect to the server
        private void RTSPListen()
        {
            transmitInfoDelegate _TIDelegate;
            _TIDelegate = new transmitInfoDelegate(transmitInfo);
            _rtspModel.createSocket(ipaddress, currentPort);
            while (!killThreads)
            {
                addToStatusDelegate("Server is waiting for new connection");
                //Start new thread to interact with the connected client
                _RTPThread = new Thread(new ParameterizedThreadStart(transmitInfo));
                _RTPThread.IsBackground = true;
                _RTPThread.Start(_rtspModel.startListening(ipaddress, currentPort));
            }
            
        }

        //Function for handling the timer timeout
        public void HandleTimer(object source, System.Timers.ElapsedEventArgs e, bool okayToSend)
        {
            okayToSend = true;
        }

        //Function for each created socket to send information in
        private void transmitInfo(object RTSPSocket)
        {
            string addressOfClient = (((IPEndPoint)((Socket)RTSPSocket).RemoteEndPoint).Address.ToString());
            _view.addToStatus("The client " + addressOfClient + " has joined");
            //Model for listening and sending for RTSP commands
            ClientModel _clientModel = new ClientModel((Socket)RTSPSocket, addressOfClient);
            
            //Model for sending the frames of the video
            RTPModel _rtpModel = new RTPModel((((IPEndPoint)((Socket)RTSPSocket).RemoteEndPoint).Address.ToString()));
            //Create new thread for listening for commands made by the client
            Thread clientModelListeningThread = new Thread(new ParameterizedThreadStart(listenForRTSPCommands));
            clientModelListeningThread.IsBackground = true;
            clientModelListeningThread.Start(_clientModel);
            bool socketCreated = false;
            long lastTicks = 0;

            //Timer for sending frames
            //bool timeToSend = true;
            //System.Timers.Timer myTimer = new System.Timers.Timer(80);
            //myTimer.Elapsed += (timerSender, timerEvent) => HandleTimer(timerSender, timerEvent, timeToSend);
            //myTimer.AutoReset = true;
           // myTimer.Enabled = true;

            int currentLocation = 0;
            byte[] messageReadBuffer = new byte[1];

            //Loop until server is closed
            while(!killThreads && !_clientModel.getFinished())
            {
                //Reset if needed or teardown
                if(_clientModel.getTeardownIssued())
                {
                    _clientModel.setTeardownIssued(false);
                    currentLocation = 0;
                    socketCreated = false;
                }

                //Only proceed if the client want the video to play
                if (_clientModel.getPlayStatus())
                {
                    //Won't proceed if the client has not selected a video
                    if (_clientModel.getVideoName() != "")
                    {
                        //Create the socket if needed
                        if(!socketCreated)
                        {
                            _rtpModel.createTheSocket(_clientModel.getRTPPort());
                            socketCreated = true;
                        }
                        
                        //Time the sending of frames
                        //if (DateTime.Now.Ticks - lastTicks > 1000000)
                        //if(timeToSend)
                        {
                            lastTicks = DateTime.Now.Ticks;

                            //timeToSend = false;
                            
                            if(_clientModel.getVideoName() == "video1.mjpeg")
                            {
                                
                                //Read the header to get the length of the frame
                                messageReadBuffer = new byte[5];

                                //videoOne.Seek(currentLocation, SeekOrigin.Begin);
                                //int bytesReadSize = videoOne.Read(messageReadBuffer, currentLocation, messageReadBuffer.Length);
                                messageReadBuffer[0] = videoOne[currentLocation++];
                                messageReadBuffer[1] = videoOne[currentLocation++];
                                messageReadBuffer[2] = videoOne[currentLocation++];
                                messageReadBuffer[3] = videoOne[currentLocation++];
                                messageReadBuffer[4] = videoOne[currentLocation++];

                                //Convert the bits to a readable integer
                                string frameSizeString = System.Text.Encoding.UTF8.GetString(messageReadBuffer);
                                int frameSize = Int32.Parse(frameSizeString);

                                //Move this thread's current location in the file
                                //currentLocation += 5;

                                //Get the data to send to the client
                                messageReadBuffer = new byte[frameSize];
                                //int bytesReadFrame = videoOne.Read(messageReadBuffer, currentLocation, messageReadBuffer.Length);
                                //currentLocation += frameSize;

                                for(int i=0; i<frameSize && currentLocation<videoOne.Length; i++)
                                {
                                    messageReadBuffer[i] = videoOne[currentLocation++];
                                }

                                //Check if the video is finished
                                if (currentLocation == videoOne.Length)
                                {
                                    _clientModel.setPlayStatus(false);
                                    _clientModel.setVideoName("");
                                    _clientModel.setTeardownIssued(true);
                                }
                            }
                            if(_clientModel.getVideoName() == "video2.mjpeg")
                            {
                                //Read the header to get the length of the frame
                                messageReadBuffer = new byte[5];
                                
                                //int bytesReadSize = videoOne.Read(messageReadBuffer, currentLocation, messageReadBuffer.Length);
                                //int bytesReadSize = videoOne.Read(messageReadBuffer, currentLocation, messageReadBuffer.Length);
                                messageReadBuffer[0] = videoTwo[currentLocation++];
                                messageReadBuffer[1] = videoTwo[currentLocation++];
                                messageReadBuffer[2] = videoTwo[currentLocation++];
                                messageReadBuffer[3] = videoTwo[currentLocation++];
                                messageReadBuffer[4] = videoTwo[currentLocation++];

                                //Convert the bits to a readable integer
                                string frameSizeString = System.Text.Encoding.UTF8.GetString(messageReadBuffer);
                                int frameSize = Int32.Parse(frameSizeString);

                                //Move this thread's current location in the file
                                //currentLocation += 5;

                                //Get the data to send to the client
                                messageReadBuffer = new byte[frameSize];
                                //int bytesReadFrame = videoOne.Read(messageReadBuffer, currentLocation, messageReadBuffer.Length);
                                //currentLocation += frameSize;

                                for (int i = 0; i < frameSize && currentLocation < videoTwo.Length; i++)
                                {
                                    messageReadBuffer[i] = videoTwo[currentLocation++];
                                }

                                //Check if the video is finished
                                if (currentLocation == videoTwo.Length)
                                {
                                    _clientModel.setPlayStatus(false);
                                    _clientModel.setVideoName("");
                                    _clientModel.setTeardownIssued(true);
                                }
                            }
                           
                            //Send the frame
                            int numBytesSent = _rtpModel.sendFrame(messageReadBuffer);
                            _view.addRTPToStatus(_rtpModel.getLastHeader());
                            //Wait for the amount of time between frames
                            Thread.Sleep(80);
                        }
                    }
                }
            }

            clientModelListeningThread.Join();
        }

        //Thread for listening for RTSP commands
        private void listenForRTSPCommands(object myClientModel)
        {
            //Generate a sessionNumber
            int mySession = sessionMaker.Next();

            while (!killThreads && !((ClientModel)myClientModel).getFinished())
            {
                //Wait for a command issued by client
                string convertedString = ((ClientModel)myClientModel).listenForCommand();
                
                

                //Parse the command
                char splitChar = '\r';

                string[] splitMessage = convertedString.Split(splitChar);
                string method = "";

                try {
                    //Add the rtspRequest to the form
                    addToRequestDelegate(splitMessage[0]);
                    addToRequestDelegate(splitMessage[1]);
                    addToRequestDelegate(splitMessage[2]);
                }catch(Exception e)
                {
                    //Client disconnected
                    addToStatusDelegate("The client " + ((ClientModel)myClientModel).getClientAddress() + " has disconnected\n");
                    break;
                }

                //The method is in string number 0
                for (int i = 0; i < 9; i++)
                {
                    //Response is an error
                    if(method == "error")
                    {
                        addToStatusDelegate("The client " + ((ClientModel)myClientModel).getClientAddress() + " has disconnected\n");
                        break;
                    }

                    method += splitMessage[0].ElementAt(i);

                    if (method == "PLAY")
                    {
                        ((ClientModel)myClientModel).setPlayStatus(true);
                        addToStatusDelegate("The client " + ((ClientModel)myClientModel).getClientAddress() + " is playing " + ((ClientModel)myClientModel).getVideoName() + "\n");
                        break;
                    }
                    if (method == "PAUSE")
                    {
                        ((ClientModel)myClientModel).setPlayStatus(false);
                        addToStatusDelegate("The client " + ((ClientModel)myClientModel).getClientAddress() + " paused " + ((ClientModel)myClientModel).getVideoName() + "\n");
                        break;
                    }
                    if (method == "TEARDOWN")
                    {
                        ((ClientModel)myClientModel).setPlayStatus(false);
                        ((ClientModel)myClientModel).setVideoName("");
                        ((ClientModel)myClientModel).setTeardownIssued(true);
                        addToStatusDelegate("The client " + ((ClientModel)myClientModel).getClientAddress() + " has torn down the video" + "\n");
                        break;
                    }
                    if(method == "SETUP")
                    {
                        char splitChar3 = ' ';
                        string[] splitMessage3 = splitMessage[0].Split(splitChar3);

                        //Get the portNumber
                        string[] splitPort = splitMessage[2].Split(splitChar3);
                        int wantedPort = Int32.Parse(splitPort[3]);

                        //Set the port
                        ((ClientModel)myClientModel).setRTPPort(wantedPort);

                        char splitChar4 = '/';
                        string[] splitMessage4 = splitMessage3[1].Split(splitChar4);
                       ((ClientModel)myClientModel).setVideoName(splitMessage4[3]);
                        addToStatusDelegate("The client " + ((ClientModel)myClientModel).getClientAddress() + " has setup video " + splitMessage4[3] +"\n");
                        break;
                    }
                }

                if (method != "error")
                {
                    //Get the sequence number
                    char splitChar2 = ':';
                    try {
                        string[] splitMessage2 = splitMessage[1].Split(splitChar2);

                        //Need to send responce to the client
                        string RTSPResponse = "RTSP/1.0 200 OK\nCSeq: " + splitMessage2[1] + "\nSession: " + mySession + "\n";
                        ((ClientModel)myClientModel).sendResponse(RTSPResponse);
                    }catch(Exception e)
                    {
                        break;
                    }
                }
            }
        }
    }
}
