using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SE3314Assignment1
{
    class RTPPacket
    {
        byte[] byteArrayToSend;
        string lastHeader;

        //Get the last header of the udp packet sent to the client so it can be displayed on the form if wanted
        public string getLastHeader()
        {
            return lastHeader;
        }

        //Take the given payload and add the rtp header to it
        public byte[] packageFrame(byte[] frameArray, int currentTime, short seqNum)
        {
            
            
            byte[] byteThreeFour = BitConverter.GetBytes(seqNum); //Sequence Number
            byte[] byteFiveEight = BitConverter.GetBytes(currentTime);//Timestamp
            

            int length = 12 + (frameArray.Length);
            byteArrayToSend = new byte[length];
            byte[] lastHeaderaArray = new byte[12];

            lastHeaderaArray[0] = 128;//V P X CC
            lastHeaderaArray[1] = 26;//M and PT
            lastHeaderaArray[2] = byteThreeFour[1];
            lastHeaderaArray[3] = byteThreeFour[0];
            lastHeaderaArray[4] = byteFiveEight[3];
            lastHeaderaArray[5] = byteFiveEight[2];
            lastHeaderaArray[6] = byteFiveEight[1];
            lastHeaderaArray[7] = byteFiveEight[0];
            lastHeaderaArray[8] = 0;//SSRC
            lastHeaderaArray[9] = 0;
            lastHeaderaArray[10] = 0;
            lastHeaderaArray[11] = 0;

            //Records the last rtp header in a binary format
            lastHeader = string.Concat(lastHeaderaArray.Select(b => Convert.ToString(b, 2)));

            //Build the header for the new packet
            byteArrayToSend[0] = 128;//V P X CC
            byteArrayToSend[1] = 26;//M and PT
            byteArrayToSend[2] = byteThreeFour[1];
            byteArrayToSend[3] = byteThreeFour[0];
            byteArrayToSend[4] = byteFiveEight[3];
            byteArrayToSend[5] = byteFiveEight[2];
            byteArrayToSend[6] = byteFiveEight[1];
            byteArrayToSend[7] = byteFiveEight[0];
            byteArrayToSend[8] = 0;//SSRC
            byteArrayToSend[9] = 0;
            byteArrayToSend[10] = 0;
            byteArrayToSend[11] = 0;

            //Add the frame to the packet
            for(int i=12; i< length; i++)
            {
                byteArrayToSend[i] = frameArray[i - 12];
            }

            
            return byteArrayToSend;
        }
    }
}
