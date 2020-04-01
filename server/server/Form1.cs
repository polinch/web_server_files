using System;
using System.IO;
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

namespace server
{
    public partial class Form1 : Form
    {

        static Socket workSocket;
        static bool bCont;
        static int numberOfConnections = 0;

        static string workPath = Directory.GetCurrentDirectory() + "\\";
        static myLogger logger = new myLogger(Directory.GetCurrentDirectory() + "\\log.txt");
        Socket listenSocket;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int bytesReceived, bytesSent;
            byte[] buffer = new byte[2000];
            byte[] message;
            string filename, messageStr = "";

            int port = 5000;
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //устанавливаем соединение
            listenSocket.Bind(endPoint);
            logger.logBind();
            //устанавливаем сокет в режим прослушивания
            listenSocket.Listen(10);
            logger.logListen();
            bCont = true;

            Task testTask = Task.Factory.StartNew(() =>
            {


                while (bCont)
                {

                    //создаем объект сокет для нового соединения
                    workSocket = listenSocket.Accept();
                    numberOfConnections++;
                    logger.logNewConnection();

                    try
                    {
                        //пока сокет на стороне клиента активен и отправляет что-то
                        while (!(workSocket.Poll(-1, SelectMode.SelectRead) && (workSocket.Available == 0)))
                        {

                            //получаем данные из связанного объекта сокет в указанный буфер
                            bytesReceived = workSocket.Receive(buffer);
                            logger.logReceive(bytesReceived);

                            filename = Encoding.UTF8.GetString(buffer).Trim('\0');
                            string tempPath = String.Concat(workPath, filename);
                            //содержательная часть: открытие файла с заданным именем, считывание из него и отправка данных
                            string tempStr;
                            try
                            {
                                using (StreamReader sr = new StreamReader(tempPath))
                                {
                                    while ((tempStr = sr.ReadLine()) != null)
                                    {
                                        messageStr += String.Concat(tempStr, "\r\n");
                                    }

                                }
                            }
                            catch(FileNotFoundException ex)
                            {
                                messageStr = "File not found";
                            }
                            message = Encoding.UTF8.GetBytes(messageStr);
                            bytesSent = workSocket.Send(message);
                            logger.logSend(bytesSent);
                            messageStr = "";

                        }
                    }
                    catch (SocketException ex)
                    {
                        logger.logDisconnect();
                    }


                    workSocket.Shutdown(SocketShutdown.Both);
                    workSocket.Close();
                }
            });

        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bCont = false;
            listenSocket.Close();
            logger.logCloseConnection(numberOfConnections);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
