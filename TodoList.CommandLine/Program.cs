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
        readonly static FileStorage storage = new FileStorage();

        static void Main(string[] args)
        {
            var todoTable = new TodoTable(storage);

            var command = args[0];
            switch (command)
            {
                case "add":
                    Add(todoTable, args[1], args[2]);
                    break;
                case "show":
                    Show(todoTable, args[1]);
                    break;
            }
        }

        private static void Add(TodoTable todoTable, string title, string detail)
        {
            var todo = new Todo(title, detail);
            todoTable.Add(todo);
            storage.Save();
        }

        static readonly IDictionary<string, Func<TodoTable, Todo>> getters = new Dictionary<string, Func<TodoTable, Todo>>
        {
            { "first", todoTable => todoTable.GetFirstTodo() },
            { "last", todoTable => todoTable.GetLastTodo() }
        };

        private static void Show(TodoTable todoTable, string type)
        {
            Func<TodoTable, Todo> getter = getters[type];
            var todo = getter(todoTable);
            if (todo == null)
                Console.Error.WriteLine("エラー: TODOがありません。");
            else
                Console.WriteLine(todo);
        }
    }
}
