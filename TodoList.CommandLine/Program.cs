using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.CommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            File.WriteAllText("backup.xml", "<TodoList><Todo><Title>買い物</Title><Detail>牛乳と卵を買う</Detail></Todo></TodoList>");
        }
    }
}
