using System;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace TCP_Socket_File_Transfer_Client
{
    class SeFTT___Server
    {
        public void SeFTT_Server()
        {
            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 5656);
            listener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                TcpClient client = listener.AcceptTcpClient();
                Console.Write("Client accepted.");

                NetworkStream stream = client.GetStream();
                StreamReader sr = new StreamReader(client.GetStream());
                StreamWriter sw = new StreamWriter(client.GetStream());
                try
                {
                    byte[] buffer = new byte[1024];
                    stream.Read(buffer, 0, buffer.Length);
                    int recv = 0;

                    foreach (var bit in buffer)
                    {
                        if (bit != 0)
                            recv++;
                    }
                    string request = Encoding.UTF8.GetString(buffer, 0, recv);
                    //Console.WriteLine("request received: " + request);

                    if (request.Contains("Get_Files"))
                    {
                        string[] info = request.Split(",#,");

                        string containCrypt = File.ReadAllText(info[1]);

                        sw.WriteLine(containCrypt);
                    }

                    if (request == "See_Files")
                    {
                        List<string> files = new List<string>();

                        Console.WriteLine();
                        string massage = "";
                        foreach (var fileName in Directory.GetFiles(@"C:\Users\Simon\Desktop\test222"))
                        {
                            massage += fileName;

                        }
                        sw.WriteLine(massage);
                        sw.Flush();
                    }
                    else {
                        //File.WriteAllText(@"C:\Users\Simon\Desktop\encrypt.txt", request);
                        File.WriteAllText("encrypt.txt", request);
                    }

                    sw.WriteLine("...");
                    sw.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n" + "\n" + ex.ToString());
                }
            }
        }
    }
}
