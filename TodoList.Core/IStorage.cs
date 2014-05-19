using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoList.Core
{
    public interface IStorage
    {
        void Append(Todo todo);

        Todo GetFirstTodo();

        Todo GetLastTodo();

        Todo DeleteFirstTodo();

        Todo DeleteLastTodo();

        IEnumerable<string> GetAllTitles();
    }

    public static class Storage
    {
        public static readonly IStorage Memory = new OnMemoryStorage();

        class OnMemoryStorage : IStorage
        {
            List<Todo> values = new List<Todo>();

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
        }
    }
}
