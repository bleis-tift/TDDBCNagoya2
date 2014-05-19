using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TodoList.Core
{
    public class Todo : IEquatable<Todo>
    {
        readonly string title;
        readonly string detail;

        public Todo(string title, string detail)
        {
            this.title = title;
            this.detail = detail;
        }

        public string Title { get { return this.title; } }
        public string Detail { get { return this.detail; } }

        public XElement ToXml()
        {
            return new XElement("Todo",
                new XElement("Title", this.title),
                new XElement("Detail", this.detail));
        }

        public bool Equals(Todo other)
        {
            return this.title == other.title && this.detail == other.detail;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Todo;
            if (other == null)
                return false;
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(this.title, this.detail).GetHashCode();
        }
    }
}
