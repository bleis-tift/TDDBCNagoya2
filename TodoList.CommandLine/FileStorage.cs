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
            var xml = new XElement("TodoList", todo.ToXml());
            xml.Save("backup.xml");
        }
    }
}
