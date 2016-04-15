using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ChatBot
{
    class IRCClient
    {
        private TcpClient tcpClient;
        private static BotAccount BotAccount = new BotAccount();
        private StreamReader inputStream;
        private StreamWriter outputStream;
        private string userName = BotAccount.accountUserName;
        private string channel = BotAccount.accountUserName;
        private string channelName;

        public IRCClient(string ip, int port, string userName, string password)
        {
            this.userName = userName;
            tcpClient = new TcpClient(ip, port);
            inputStream = new StreamReader(tcpClient.GetStream());
            outputStream = new StreamWriter(tcpClient.GetStream());

            outputStream.WriteLine("PASS " + password);
            outputStream.WriteLine("NICK " + userName);
            outputStream.Flush();
        }
        public void joinRoom(string channel)
        {
            this.channel = channel;
            string choice;
            StreamReader inFile;
            FileInfo infoFile = new FileInfo("accountInfo.txt");
            try
            {
                inFile = infoFile.OpenText();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File containing account information was not found!");
                Console.WriteLine(@"Make sure it is spelled ""accountInfo.txt"" and is in the same folder as this executable!");
                Console.WriteLine("Creating the file for you just incase!");
                File.WriteAllText("accountInfo.txt", @"Replace this line with username here
Replace this line with oath code here");
                throw;
            }
            if(File.Exists("accountInfo.txt"))
            {
                Console.Write("Join the account that was placed in the accountInfo.txt? (Y or N): ");
                choice = Console.ReadLine();
                if (choice == "Y" || choice == "y")
                {
                    channel = BotAccount.accountUserName;
                    this.channelName = channel;
                    outputStream.WriteLine("JOIN #" + channel);
                    outputStream.Flush();
                }
                else if (choice == "N" || choice == "n")
                {
                    Console.Write("Please enter achannel to join: ");
                    channel = Console.ReadLine();
                    this.channelName = channel;
                    outputStream.WriteLine("JOIN #" + channel);
                    outputStream.Flush();
                }
            }

        }


        public void sendIRCMessage(string message)
        {
            outputStream.WriteLine(message);
            outputStream.Flush();
        }
        public void sendChatMessage (string message)
        {
            sendIRCMessage("PRIVMSG #" + channelName + " :" + message);
        }
        public string readMessage()
        {
            string message = inputStream.ReadLine();
            return message;
        } 
    }
}
