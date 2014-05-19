using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TodoList.Core;

namespace TodoList.Core.Tests
{
    [TestFixture]
    public class TodoListTest
    {
        [TestCase("買い物メモ", "牛乳と卵")]
        [TestCase("買い物", "牛乳と卵を買う")]
        public void 空のTodoTableにTodoを追加してその詳細が取得できる(string title, string detail)
        {
            var todoTable = new TodoTable(Storage.Null);
            todoTable.Add(new Todo(title, detail));
            Assert.That(todoTable.GetLastTodo(), Is.EqualTo(new Todo(title, detail)));
        }

        [TestCase("買い物メモ", "牛乳と卵")]
        [TestCase("買い物", "牛乳と卵を買う")]
        public void 空のTodoTableにTodoを追加したものはストレージに保存される(string title, string detail)
        {
            var storage = new Mock<IStorage>();
            storage.Setup(_ => _.LoadAllTodo()).Returns(new List<Todo>());
            storage.Setup(_ => _.Save(new Todo(title, detail)));

            var todoTable = new TodoTable(storage.Object);
            todoTable.Add(new Todo(title, detail));

            storage.VerifyAll();
        }
    }
}
