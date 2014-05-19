using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoList.Core
{
    public class TodoTable
    {
        readonly List<Todo> values = new List<Todo>();

        public void Add(Todo todo)
        {
            this.values.Add(todo);
        }

        public Todo GetLastTodo()
        {
            return this.values.LastOrDefault();
        }
    }
}
