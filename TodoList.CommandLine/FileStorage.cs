using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TodoList.Core;

namespace TodoList.CommandLine
{
    class FileStorage : IStorage
    {
        public void Save(Todo todo)
        {
            var xml = new XElement("TodoList",
                new XElement("Todo",
                    new XElement("Title", todo.Title),
                    new XElement("Detail", todo.Detail)));
            xml.Save("backup.xml");
        }
    }
}
