using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChatBot
{
    class Commands
    {
        public FileInfo infoFile { get; set; }
        public StreamReader inFile;
        public Dictionary<string, string> CommandEffects { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> AutoCommands { get; private set; } = new Dictionary<string, string>();
        private string[] botChat;
        private string commandText;
        private string effectText = "";
        private char[] commandDelimiter = { '~' };
        public Commands()
        {
            infoFile = new FileInfo("commandInfo.txt");
            try
            {
                inFile = infoFile.OpenText();
                if(File.Exists("commandInfo.txt"))
                {
                    getCommands();
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File Not Found!");
                Console.WriteLine("Make sure the file name is exactly \"commandInfo.txt\".");
                Console.WriteLine("Also make sure it is in the same folder or location as the executable.");
                Console.WriteLine("Creating a file for you to use!");
                Console.WriteLine("Open with text editor to edit!");
                File.WriteAllText("commandInfo.txt", @"# Please structure the following commands in this format EXACTLY or commands will not run!
# This file needs to be this way because the bot will correctly parse your commands so
# that they will run correctly!
# Format: (Without the # to the left! ""#"" are used for commenting!)
# NOTE: Please refrain from using ""~"" in effects and commands! It will break the bot!
# NOTE: Keep effects and commands on one line! It will break the bot!
# NOTE: There should be no blank lines between a group of a commnad and an effect!
# Command~!hello
# Effect~ What's up");

                throw;
            }
            inFile.Close();
        }
        private void getCommands()
        {
            Console.WriteLine("Loading Commands");
            string textLine;
            bool commandsSet = true;

            string currentCommand = "";

            while ((textLine = inFile.ReadLine()) != null)
            {
                botChat = new string[] { };
                if (textLine.StartsWith("# "))
                {
                    continue;
                }
                else if (textLine.StartsWith("Command~"))
                {
                    botChat = textLine.Split(commandDelimiter);
                    commandText = botChat[1];
                    currentCommand = commandText;
                }
                else if (textLine.StartsWith("Effect~"))
                {
                    if (currentCommand.Equals("")) { continue; }

                    botChat = textLine.Split(commandDelimiter);
                    for (int i = 1; i < botChat.Length; i++)
                    {
                        if (botChat[i] == Environment.NewLine)
                        {
                            continue;
                        }
                        else
                        {
                            effectText += botChat[i];
                        }
                    }
                    CommandEffects.Add(currentCommand, effectText);
                    currentCommand = "";
                    effectText = "";
                }
                else if (textLine.StartsWith("Auto Command~"))
                {
                    botChat = textLine.Split(commandDelimiter);
                    commandText = botChat[1];
                    currentCommand = commandText;
                }
                else if (textLine.StartsWith("Auto Effect~"))
                {
                    botChat = textLine.Split(commandDelimiter);
                    for (int i = 1; i < botChat.Length; i++)
                    {
                        if (botChat[i] == Environment.NewLine)
                        {
                            continue;
                        }
                        else
                        {
                            effectText += botChat[i];
                        }
                    }
                    AutoCommands.Add(currentCommand, effectText);
                    currentCommand = "";
                    effectText = "";
                }
                else
                {
                    Console.WriteLine("There is an error in the commands file!");
                    Console.WriteLine("Please check the formatting!");
                    Console.WriteLine("Commands not added! Please restart ChatBot!");
                    commandsSet = false;
                    break;
                }
            }
            if (commandsSet)
            {
                Console.WriteLine("Commands were set!");
            }
        }
    }
}