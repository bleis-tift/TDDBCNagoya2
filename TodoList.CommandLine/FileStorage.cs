using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using TodoList.Core;

namespace TodoList.CommandLine
{
    class FileStorage : IStorage
    {
        readonly XElement xml;

        internal FileStorage()
        {
            xml = File.Exists("backup.xml") ? XElement.Load("backup.xml") : new XElement("TodoList");
        }

        public IList<Todo> LoadAllTodo()
        {
            return this.xml.XPathSelectElements("//Todo").Select(e => new Todo(e.Element("Title").Value, e.Element("Detail").Value)).ToList();
        }

        public void Save(Todo todo)
        {
            var xml = new XElement("TodoList", todo.ToXml());
            xml.Save("backup.xml");
        }
    }
}
