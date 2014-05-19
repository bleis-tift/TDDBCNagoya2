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

            public IEnumerable<string> GetAllTitles()
            {
                return this.values.Select(todo => todo.Title);
            }
        }
    }
}
