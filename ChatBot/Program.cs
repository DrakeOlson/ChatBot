using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatBot
{
    class Program
    {
        static void Main(string[] args)
        {
            string message;
            string effect = null;
            string[] splitMessage;
            string command;
            BotAccount BotAccount = new BotAccount();
            Commands Commands = new Commands();
            IRCClient irc = new IRCClient("irc.twitch.tv", 6667, BotAccount.accountUserName,BotAccount.accountPassword);

            Console.WriteLine("Setting up ChatBot!");
            irc.joinRoom(BotAccount.accountUserName);
            Thread.Sleep(1750);
            Console.WriteLine("ChatBot is set up!");
            Thread.Sleep(1750);
            Console.Clear();

            while (true)
            {

                message = irc.readMessage();
                splitMessage = message.Split(new char[] { ' ' });
                if (splitMessage.Length < 4 || !splitMessage[1].Equals("PRIVMSG"))
                {
                    continue;
                }
                else
                {
                    command = splitMessage[3].Substring(1);

                    foreach (string c in Commands.CommandEffects.Keys)
                    {
                        if (command.Equals(c))
                        {
                            
                            if (Commands.CommandEffects.TryGetValue(c, out effect))
                            {
                                irc.sendChatMessage(effect);
                                effect = null;
                            }
                        }
                    }
                }
            }
        }
    }
}
