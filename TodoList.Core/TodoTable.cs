using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoList.Core
{
    public class TodoTable
    {
        readonly IStorage storage;
        readonly IList<Todo> values;

        public TodoTable(IStorage storage)
        {
            this.storage = storage;
            this.values = this.storage.LoadAllTodo();
        }

        public void Add(Todo todo)
        {
            this.values.Add(todo);
            this.storage.Save(todo);
        }

        public Todo GetLastTodo()
        {
            return this.values.LastOrDefault();
        }
    }
}
