using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoList.Core
{
    public interface IStorage
    {
        void Save(IEnumerable<Todo> list);
    }
}
