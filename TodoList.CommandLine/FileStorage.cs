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
        readonly List<Todo> values;

        internal FileStorage()
        {
            var xml = File.Exists("backup.xml") ? XElement.Load("backup.xml") : new XElement("TodoList");
            this.values = xml.XPathSelectElements("//Todo").Select(e => new Todo(e.Element("Title").Value, e.Element("Detail").Value)).ToList();
        }

        public void Append(Todo todo)
        {
            this.values.Add(todo);
        }

        public Todo GetFirstTodo()
        {
            return this.values.FirstOrDefault();
        }

        public Todo GetLastTodo()
        {
            return this.values.LastOrDefault();
        }

        public Todo DeleteFirstTodo()
        {
            var first = this.GetFirstTodo();
            if (first == null)
                return null;
            this.values.RemoveAt(0);
            return first;
        }

        public Todo DeleteLastTodo()
        {
            var last = this.GetLastTodo();
            if (last == null)
                return null;
            this.values.RemoveAt(this.values.Count - 1);
            return last;
        }

        public IEnumerable<string> GetAllTitles()
        {
            return this.values.Select(todo => todo.Title);
        }

        public void Save()
        {
            var xml = new XElement("TodoList", this.values.Select(todo => todo.ToXml()));
            xml.Save("backup.xml");
        }
    }
}
