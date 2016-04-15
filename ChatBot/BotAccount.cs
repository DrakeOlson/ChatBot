using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace ChatBot
{
    class BotAccount
    {
        public string accountUserName { get; set; }
        public string accountPassword { get; set; }
        public FileInfo infoFile { get; set; }
        public StreamReader inFile; 
        
        public BotAccount()
        {
            getAccountInformation();
        }
        private void getAccountInformation()
        {
            infoFile = new FileInfo("accountInfo.txt");
            try
            {
                inFile = infoFile.OpenText();
                accountUserName = inFile.ReadLine();
                accountPassword = inFile.ReadLine();
                inFile.Close();
                Console.WriteLine("Account information set!");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File Not Found!");
                Console.WriteLine("Make sure the file name is exactly \"accountInfo.txt\".");
                Console.WriteLine("Also make sure it is in the same folder or location as the executable.");
                throw;
            }
        }
    }
}
