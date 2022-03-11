using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static int size;
        static Socket socket;
        static IPEndPoint endPoint;
        
        const int port = 52000;
        const string ip = "127.0.0.1";
        static byte[] data = new byte[1024];
        static StringBuilder name = new StringBuilder();

        static void Main(string[] args)
        {
            Initialise();
            Listening();
        }

        static void Initialise()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(endPoint);
            socket.Listen(5);
        }

        static void Listening()
        {
            while (true)
            {
                var listener = socket.Accept();

                listener.Receive(data);

                name.Append(Encoding.UTF8.GetString(data, 1, data[0]));
                Console.WriteLine(name);

                using (FileStream fstream = new FileStream(name.ToString(), FileMode.OpenOrCreate))
                {
                    do
                    {
                        size = listener.Receive(data);
                        fstream.Write(data, 0, size);

                    } while (listener.Available > 0);
                }

                name.Clear();

                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
        }
    }
}
