using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TodoList.Core;

namespace TodoList.CommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            var storage = new FileStorage();
            var todoTable = new TodoTable(storage);

            var command = args[0];
            var title = args[1];
            var detail = args[2];

            var todo = new Todo(title, detail);
            todoTable.Add(todo);
        }
    }
}
