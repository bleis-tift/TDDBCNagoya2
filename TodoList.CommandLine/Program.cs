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
            switch (command)
            {
                case "add":
                    {
                        var title = args[1];
                        var detail = args[2];

                        var todo = new Todo(title, detail);
                        todoTable.Add(todo);
                        storage.Save();
                        break;
                    }
                case "show":
                    {
                        var type = args[1];
                        switch (type)
                        {
                            case "first":
                                {
                                    var todo = todoTable.GetFirstTodo();
                                    if (todo == null)
                                        Console.Error.WriteLine("エラー: TODOがありません。");
                                    else
                                        Console.WriteLine(todo);
                                    break;
                                }
                            case "last":
                                {
                                    var todo = todoTable.GetLastTodo();
                                    if (todo == null)
                                        Console.Error.WriteLine("エラー: TODOがありません。");
                                    else
                                        Console.WriteLine(todo);
                                    break;
                                }
                        }
                        break;
                    }
            }
        }
    }
}
