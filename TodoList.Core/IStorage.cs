using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoList.Core
{
    public interface IStorage
    {
        IList<Todo> LoadAllTodo();

        void Save(Todo todo);
    }

    public static class Storage
    {
        public static readonly IStorage Null = new NullStorage();

        class NullStorage : IStorage
        {
            public IList<Todo> LoadAllTodo()
            {
                return new List<Todo>();
            }

            public void Save(Todo todo)
            {
                // do nothing
            }
        }
    }
}
