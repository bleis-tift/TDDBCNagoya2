using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Core;

namespace TodoList.Core.Tests
{
    [TestFixture]
    public class TodoListTest
    {
        [Test]
        public void 空のTodoTableにTodoを追加してその詳細が取得できる()
        {
            var todoTable = new TodoTable();
            todoTable.Add(new Todo("買い物メモ", "牛乳と卵"));
            Assert.That(todoTable.GetLastTodo(), Is.EqualTo(new Todo("買い物メモ", "牛乳と卵")));
        }
    }
}
