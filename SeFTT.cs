using System;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TCP_Socket_File_Transfer_Client
{
    class SeFTT
    {
        public void SeFTT_Run()
        {

            Console.WriteLine("TCP Socket File Transfer Client. . .   " + "\n" + "Server 'SeFTT' --> Socket. e. File. Transfer. TPC Client" +
           "\n"
           );

            TcpClient client = new TcpClient("192.168.1.199", 5656);

            string frontCommand = "&-SeFTT Socket: >";
            Console.Write(frontCommand);

            while (true)
            {
                string command = Console.ReadLine();

                if (command.Contains("&-Exit"))
                {
                    break;
                }

                if (command.Contains("&-Get_File"))
                {
                    Console.Write(frontCommand + "Enter path to extration file: ");
                    string path_to_file = Console.ReadLine();

                    // Sending get file command to server
                    int Get_File_byteCount = Encoding.ASCII.GetByteCount("Get_Files" + ",#," + path_to_file + 1);
                    byte[] Get_File_sendData = new byte[Get_File_byteCount];
                    Get_File_sendData = Encoding.ASCII.GetBytes("Get_Files" + ",#," + path_to_file);

                    NetworkStream Get_File_stream = client.GetStream();
                    Get_File_stream.Write(Get_File_sendData, 0, Get_File_sendData.Length);

                    StreamReader Get_File_sr = new StreamReader(Get_File_stream);
                    string Get_File_response = Get_File_sr.ReadLine();

                    decrypt(Get_File_response);
                }


                if (command.Contains("&-Info"))
                {
                    Console.WriteLine(frontCommand + "\n" + "'&-' is a command used in all command and indicates that a command is given" +
                        "\n" + "\n" +
                        "for info type: &-Info" +
                        "\n" + "\n" +
                        "To send a file type: &-Send_File");
                }

                else if (command.Contains("&-See_File"))
                {
                    int byteCount = Encoding.ASCII.GetByteCount("See_Files" + 1);
                    byte[] sendData = new byte[byteCount];
                    sendData = Encoding.ASCII.GetBytes("See_Files");

                    NetworkStream stream = client.GetStream();
                    stream.Write(sendData, 0, sendData.Length);

                    StreamReader sr = new StreamReader(stream);
                    string response = sr.ReadLine();

                    List<string> responseSpit = new List<string>();
                    string[] paths = response.Split("C:");
                    foreach (var path in paths)
                    {
                        Console.WriteLine();
                        if (path != "")
                        {
                            responseSpit.Add(path);
                            Console.WriteLine(frontCommand + "C:" + path);
                        }
                    }
                    stream.Close();
                    client.Close();
                }

                else if (command.Contains("&-Send_File"))
                {
                    // Prepare the package
                    Console.Write(frontCommand + "Enter path to package: ");
                    string package = Console.ReadLine();

                    bool directory_check = false;
                    FileAttributes attributes = File.GetAttributes(package);
                    if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        directory_check = true;
                    }
                            
                    if (directory_check == true)
                    {
                        string directoryFiles = null;
                        string directoryPathBefore = null;
                        string[] diresInPathBefore = null;
                        string[] dirsInPath = null;

                        foreach (var directoryPath in Directory.GetFiles(package, "", SearchOption.AllDirectories))
                        {
                            if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                directoryFiles += (encrypt(directoryPath) + vars.EncryptFileSplitter);
                            }
                            diresInPathBefore = dirsInPath;
                            dirsInPath = directoryPath.Split("\\");
                            foreach (var folder in dirsInPath)
                            {
                                for (int i = 0; i < dirsInPath.Length - 1; i++) {
                                    try
                                    {
                                        if (dirsInPath[i] != diresInPathBefore[i])
                                        {
                                            directoryFiles += vars.directorySplitter;
                                            break;
                                        }
                                    }
                                    catch (Exception) { }
                                }
                            }

                            directoryPathBefore = directoryPath;
                        }
                        directoryFiles += "&#MultiFile#&";

                        int byteCount = Encoding.ASCII.GetByteCount(directoryFiles + 1);
                        byte[] sendData = new byte[byteCount];
                        sendData = Encoding.ASCII.GetBytes(directoryFiles);

                        NetworkStream stream = client.GetStream();
                        stream.Write(sendData, 0, sendData.Length);

                        StreamReader sr = new StreamReader(stream);
                        string response = sr.ReadLine();
                    }

                    // Encrupt the package.
                    else if (encrypt(package) != null && directory_check == false)
                    {
                        vars.MessageToSend = encrypt(package);
                    }

                    connection:
                    try
                    {
                        // Prepareing the message, turning it to byte array.
                        int byteCount = Encoding.ASCII.GetByteCount(vars.MessageToSend + 1);
                        byte[] sendData = new byte[byteCount];
                        sendData = Encoding.ASCII.GetBytes(vars.MessageToSend);

                        // Sending the data to the other computer.
                        Console.WriteLine(frontCommand + "Sending data. . .");
                        NetworkStream stream = client.GetStream();
                        stream.Write(sendData, 0, sendData.Length);
                        Console.Write(frontCommand + "Data has been send.");

                        StreamReader sr = new StreamReader(stream);
                        string response = sr.ReadLine();
                        Console.WriteLine(response);

                        stream.Close();
                        client.Close();
                        Console.ReadKey();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\n" + "Fail to connect to serever. . .\n" + ex.ToString());
                        goto connection;
                    }
                }
            }
        }
        private static string encrypt(string path)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                var onConvertion = Convert.ToBase64String(bytes);
                var addOn = onConvertion + vars.salt;
                return addOn;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        private static void decrypt(string crypt)
        {
            if (crypt.Contains(vars.salt))
            {
                StringBuilder sb = new StringBuilder(crypt);
                sb.Replace(vars.salt, "");
                crypt = sb.ToString();
            }
            byte[] deConvertion = Convert.FromBase64String(crypt);

            File.WriteAllBytes(@"C:\Users\Simon\Desktop\name1decon.txt", deConvertion);
            Console.WriteLine("\n" + "Done. . ." + "\n");
        }
    }
}
