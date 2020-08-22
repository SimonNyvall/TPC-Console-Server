using System;
using System.Threading;

namespace TCP_Socket_File_Transfer_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            threadClass thClass = new threadClass();
            Thread setPerformaceThread = new Thread(thClass.setPerformace);
            //setPerformaceThread.Start();

            SeFTT SF = new SeFTT();

            SeFTT___Server SF_S = new SeFTT___Server();

            Console.WriteLine("Client Connection System dir. . . " + "\n" +
                "Server 'PexS' --> Privete. Socket. Server. System");

            string frontCommand = "&-Pext Socket: >";
            Console.Write("\n" + frontCommand);

            while (true)
            {
                string command = Console.ReadLine();
                Console.WriteLine();

                switch (command)
                {
                    case "&-SeFTT":
                        Console.WriteLine();
                        SF.SeFTT_Run();

                        break;

                    case "&-Info":
                        Console.WriteLine("\n" + frontCommand + "'&-' is a command used for navigation through the server." + "\n");
                        Console.WriteLine("'&-SeFTT' Socket. File. Transfer. TCP Client" + "\n" + "Used for trnsfer of files.");

                        Console.Write(frontCommand);
                        break;
                    case "&-Start_Server_Instace":
                        Console.WriteLine();
                        SF_S.SeFTT_Server();
                        break;
                }

                Console.Write(frontCommand);
            }
        }
    }
    class vars
    {
        public static string MessageToSend { get; set; }
        public const string salt = "GJkrFkfpEEmp";

        public const string directorySplitter = "&#Directory#&";
        public const string EncryptFileSplitter = "&#EncryptFile#&";
    }
}
