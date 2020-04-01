using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace client
{
    public partial class Form1 : Form
    {

        Socket socket;

        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void establishAConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string host = textBox1.Text;
            int port;
            if (host.Equals(""))
            {
                MessageBox.Show("Host is empty", "Error", MessageBoxButtons.OK);
                return;
            }     
            try
            {
                port = Int32.Parse(textBox2.Text);
            }
            catch(FormatException ex)
            {
                MessageBox.Show("Port is empty", "Error", MessageBoxButtons.OK);
                return;
            }

            IPAddress[] IPs = Dns.GetHostAddresses(host);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(IPs[0], port);
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Connection error", "Error", MessageBoxButtons.OK);
            }

        }

        private void closeConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            textBox1.Text = String.Empty;
            textBox2.Text = String.Empty;
            textBox3.Text = String.Empty;
            textBox4.Text = String.Empty;
        }

        private void getToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string filename = textBox3.Text;
            if (filename.Equals(""))
            {
                MessageBox.Show("Filename is empty", "Error", MessageBoxButtons.OK);
                return;
            }
            byte[] message = new byte[30];
            message = Encoding.UTF8.GetBytes(filename);
            byte[] bytes = new byte[2000];

            try
            {
                int bytesSent = socket.Send(message);
                int bytesReceived = socket.Receive(bytes);
                textBox3.Text = String.Empty;
                textBox4.Text = (filename + " contents:\r\n");
                textBox4.Text += Encoding.UTF8.GetString(bytes).Trim('\0');
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Socket error", "Error", MessageBoxButtons.OK);
            }
        }
    }
}
