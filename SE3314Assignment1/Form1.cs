using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SE3314Assignment1
{
    public partial class Form1 : Form
    {
        static Controller _controller;

        //Delegate for adding strings to the form
        public delegate void formStringDelegate(string infoToDisplay);
        //Instantiate the delegates
        formStringDelegate addToStatusDelegate;
        formStringDelegate addToRequestDelegate;

        public Form1()
        {
            InitializeComponent();
            //Instantiate the delegates
            addToStatusDelegate = new formStringDelegate(addToStatus);
            addToRequestDelegate = new formStringDelegate(addToRequest);
            _controller = new Controller(this, addToStatusDelegate, addToRequestDelegate);
        }

        public void addToStatus(string newInfo)
        {
            //Make sure that no other threads other than this one access the form
            if (this.statusBox.InvokeRequired)
            {
                try {
                    //If not the correct thread, create a new delegate to handle the call
                    formStringDelegate newStringDel = new formStringDelegate(addToStatus);
                    this.Invoke(newStringDel, newInfo);
                }catch(Exception e)
                {

                }
            }
            else
            {
                statusBox.Items.Add(newInfo + "\n");//Add the information to the statusbox
            }
        }

        //Add a line to the request box
        public void addToRequest(string newInfo)
        {
            if (this.requestTextBox.InvokeRequired)
            {
                try {
                    formStringDelegate newStringDel = new formStringDelegate(addToRequest);
                    this.Invoke(newStringDel, newInfo);
                }catch(Exception e)
                {

                }
            }
            else
            {
                requestTextBox.Items.Add(newInfo + "\n");
            }
            
        }

        public void addRTPToStatus(string newInfo)
        {
            //Only add it if the checkbox is selected
            if (printHeaderCheckBox.Checked)
            {
                //Make sure that no other threads other than this one access the form
                if (this.statusBox.InvokeRequired)
                {
                    try
                    {
                        formStringDelegate newStringDel = new formStringDelegate(addToStatus);
                        this.Invoke(newStringDel, newInfo);
                    }
                    catch (Exception e)
                    {

                    }
                }
                else
                {
                    statusBox.Items.Add(newInfo + "\n");
                }
            }
        }

        private void listenButton_Click(object sender, EventArgs e)
        {
            //Start listening for connections a the specified port
            _controller.startAcceptingConnections();

        }

        //Update the server port number to the desired port
        private void portInput_ValueChanged(object sender, EventArgs e)
        {
            _controller.setCurrentPort((int) portInput.Value);
        }

        //Update the sever ip address to the desired address
        private void ipInput_TextChanged(object sender, EventArgs e)
        {
            _controller.setIPAddress(ipInput.Text);
        }

       
    }
}
