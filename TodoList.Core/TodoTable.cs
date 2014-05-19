using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoList.Core
{
    public class TodoTable
    {
        public void Add(Todo todo)
        {
        }

        public Todo GetLastTodo()
        {
            return new Todo("買い物メモ", "牛乳と卵");
        }
    }
}
