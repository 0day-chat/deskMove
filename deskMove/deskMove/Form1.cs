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
using System.Web;
using System.IO;
using System.Net.Sockets;

namespace deskMove
{
    public partial class Form1 : Form
    {
        int port = 6099;
        public static Bitmap desktop = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        public static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public Socket cl;
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            servStart();
        }

        public void servStart()
        {
            try
            {
                socket.Bind(new IPEndPoint(IPAddress.Any, 6099));
                socket.Listen(10);
                socket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
                //Socket cl = socket.Accept();
                /*while (true)
                {
                    Application.DoEvents();
                    Graphics g = Graphics.FromImage(desktop as Image);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    g.CopyFromScreen(0, 0, 0, 0, desktop.Size);
                    ImageConverter imageConverter = new ImageConverter();
                    byte[] ImageInArray = (byte[])imageConverter.ConvertTo(desktop, typeof(byte[]));
                   
                    //cl.Send(ImageInArray);
                }*/
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " servStart", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MemoryStream ms = new MemoryStream(ImageInArray);
                //pictureBox1.Image = Image.FromStream(ms);
            }

        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            try
            {
                cl = socket.EndAccept(ar);
                Application.DoEvents();
                Graphics g = Graphics.FromImage(desktop as Image);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                g.CopyFromScreen(0, 0, 0, 0, desktop.Size);
                ImageConverter imageConverter = new ImageConverter();
                byte[] ImageInArray = (byte[])imageConverter.ConvertTo(desktop, typeof(byte[]));
                cl.BeginSend(ImageInArray, 0, ImageInArray.Length, SocketFlags.None, new AsyncCallback(SendCallBack), null); //Send image in array
                Array.Resize(ref ImageInArray, socket.SendBufferSize);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " AcceptCallBack", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendCallBack(IAsyncResult ar)
        {
            try
            {
                cl.EndSend(ar);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " AcceptCallBack", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            servStart();
            //picMassive();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            socket.Close();
            //pictureBox1.Image = Image.FromFile("1.jpg");
        }

        private void picMassive(byte[] ImageInArray)
        {
                Graphics g = Graphics.FromImage(desktop as Image);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                g.CopyFromScreen(0, 0, 0, 0, desktop.Size);
                ImageConverter imageConverter = new ImageConverter();
                ImageInArray = (byte[])imageConverter.ConvertTo(desktop, typeof(byte[]));
        }
    }
}
