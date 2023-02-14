using System;
using System.IO;

namespace АИС_Автосалон
{
    class Logger
    {
        public void Log(string message)
        {
            DateTime now = DateTime.Now;
            File.AppendAllText("log.txt", "[" + now + "] " + message + Environment.NewLine);
            Console.WriteLine("[" + now + "] " + message);
        }
    }
}
