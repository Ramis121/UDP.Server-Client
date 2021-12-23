using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UDP.Server_Client
{
    class Program
    {
        private static IPAddress remoteIPAddress;
        private static int remotePort;
        private static int localPort;

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Укажите локальный порт");
                localPort = Convert.ToInt16(Console.ReadLine());

                Console.WriteLine("Укажите удаленный порт");
                remotePort = Convert.ToInt16(Console.ReadLine());

                Console.WriteLine("Укажите удаленный IP-адрес");
                remoteIPAddress = IPAddress.Parse(Console.ReadLine());

                // Создаем поток для прослушивания
                Thread tRec = new Thread(new ThreadStart(Client));
                tRec.Start();

                while (true)
                {
                    Server(Console.ReadLine());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
        }

        public static void Server(string message)
        {
            UdpClient udpClient = new UdpClient();

            IPEndPoint iPEndPoint = new IPEndPoint(remoteIPAddress, remotePort);
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);

                udpClient.Send(buffer, buffer.Length, iPEndPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникла ошибка: {0}", ex.Message);
            }
            finally
            {
                udpClient.Close();
            }
        }

        public static void Client()
        {
            UdpClient udpClient = new UdpClient(localPort);
            IPEndPoint iPEnd = null;
            try
            {
                Console.WriteLine(
                  "\n-----------*******Общий чат*******-----------");
                while (true)
                {
                    byte[] rec = udpClient.Receive(ref iPEnd);

                    string returnData = Encoding.UTF8.GetString(rec);
                    Console.WriteLine(" -->{0}", returnData.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
