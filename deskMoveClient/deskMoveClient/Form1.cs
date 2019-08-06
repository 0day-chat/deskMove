using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace deskMoveClient
{
    public partial class Form1 : Form
    {
        public static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        int i = 0;
        public byte[] ImageInArray;
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            startActing();
        }


        public void startActing()
        {
            i = 1;
            string ip;
            ip = textBox1.Text;
            try
            {
                //socket.Connect(ip, 6099);
                socket.BeginConnect(ip, 6099, new AsyncCallback(ConnectCallBack), null);
                //socket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
                /*while (i == 1)
                {
                    byte[] ImageInArray = new byte[999999];
                    //byte[] hello = Encoding.ASCII.GetBytes("Hello!");
                    ImageConverter imageConverter = new ImageConverter();
                    socket.Receive(ImageInArray);
                    //MemoryStream ms = new MemoryStream(ImageInArray);
                    Image img = (Image)imageConverter.ConvertFrom(ImageInArray);
                    Application.DoEvents();
                    pictureBox1.Image = img;

                
                }*/
            }
            catch
            {

            }
            if (i == 0)
            {
                socket.Disconnect(true);
                socket.Close();
                pictureBox1.Image = Image.FromFile("1.jpg");
            }
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            try
            {
                socket.EndConnect(ar);
                ImageInArray = new byte[socket.ReceiveBufferSize];
                socket.BeginReceive(ImageInArray, 0, ImageInArray.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), null);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " ConnectCallBack", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                socket.EndReceive(ar);
                Application.DoEvents();
                ImageConverter imgConv = new ImageConverter();
                Image img = (Image)imgConv.ConvertFrom(ImageInArray);
                pictureBox1.Image = img;
                Array.Resize(ref ImageInArray, socket.ReceiveBufferSize);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " ReceiveCallBack", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            socket.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), null);
        }

        private void DisconnectCallback(IAsyncResult ar)
        {
            socket.EndDisconnect(ar);
        }
    }
}
