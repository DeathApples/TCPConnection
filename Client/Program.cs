using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            const int port = 52000;
            var data = new byte[1024];
            const string ip = "127.0.0.1";

            Console.Write("Введите имя файла, который хотите сохранить на сервере: ");
            string name = Console.ReadLine();

            var endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);

            data[0] = (byte)name.Length;
            Encoding.UTF8.GetBytes(name, 0, name.Length, data, 1);

            socket.Send(data);

            using (var fstream = new FileStream(name, FileMode.Open))
            {
                while (fstream.Read(data, 0, data.Length) > 0)
                {
                    socket.Send(data);
                }
            }

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
