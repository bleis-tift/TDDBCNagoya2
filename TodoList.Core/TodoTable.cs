using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoList.Core
{
    public class TodoTable
    {
        readonly IStorage storage;

        public TodoTable(IStorage storage)
        {
            this.storage = storage;
        }

        public void Add(Todo todo)
        {
            this.storage.Append(todo);
        }

        public Todo GetLastTodo()
        {
            return this.storage.GetLastTodo();
        }

        public Todo GetFirstTodo()
        {
            return this.storage.GetFirstTodo();
        }
    }
}
