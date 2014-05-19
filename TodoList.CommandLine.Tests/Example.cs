using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;
using System.Xml.Linq;
using System.IO;

namespace TodoList.CommandLine.Tests
{
    [TestFixture]
    public class Example
    {
        static void DeleteBackupIfExists()
        {
            if (File.Exists("backup.xml"))
                File.Delete("backup.xml");
        }

        [SetUp]
        public void SetUp()
        {
            DeleteBackupIfExists();
        }

        static void Execute(string subcommand, params string[] args)
        {
            var startInfo = new ProcessStartInfo("TodoList.exe", subcommand + " " + string.Join(" ", args))
            {
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (var proc = Process.Start(startInfo))
            {
                proc.WaitForExit();
            }
        }

        enum StreamKind { StandardOutput, StandardError }

        static string ExecuteAndReadStream(StreamKind kind, string subcommand, params string[] args)
        {
            var procInfo = new ProcessStartInfo("TodoList.exe", subcommand + " " + string.Join(" ", args))
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = kind == StreamKind.StandardOutput,
                RedirectStandardError = kind == StreamKind.StandardError
            };
            using (var proc = new Process { StartInfo = procInfo })
            {
                proc.Start();
                switch (kind)
                {
                    case StreamKind.StandardOutput:
                        return proc.StandardOutput.ReadToEnd();
                    case StreamKind.StandardError:
                        return proc.StandardError.ReadToEnd();
                    default:
                        throw new Exception("oops!");
                }
            }
        }

        [TestFixture]
        public class 初期状態
        {
            [SetUp]
            public void SetUp()
            {
                DeleteBackupIfExists();
            }

            [Test]
            public void 追加されたTODO一覧が見れる()
            {
                var output = ExecuteAndReadStream(StreamKind.StandardOutput, "list");
                Assert.That(output, Is.EqualTo("TODOはありません。\r\n"));
            }

            [Test]
            public void 最初に追加されたTodoを見ようとするとエラーが表示される()
            {
                var output = ExecuteAndReadStream(StreamKind.StandardError, "show", "first");
                Assert.That(output, Is.EqualTo("エラー: TODOがありません。\r\n"));
            }

            [Test]
            public void 最後に追加されたTodoを見ようとするとエラーが表示される()
            {
                var output = ExecuteAndReadStream(StreamKind.StandardError, "show", "last");
                Assert.That(output, Is.EqualTo("エラー: TODOがありません。\r\n"));
            }

            [Test]
            public void 最初に追加されたTodoを削除しようとするとエラーが表示される()
            {
                var output = ExecuteAndReadStream(StreamKind.StandardError, "delete", "first");
                Assert.That(output, Is.EqualTo("エラー: TODOがありません。\r\n"));
            }
        }

        [TestCase("買い物", "牛乳と卵を買う", "<TodoList><Todo><Title>買い物</Title><Detail>牛乳と卵を買う</Detail></Todo></TodoList>")]
        [TestCase("買い物メモ", "牛乳と卵", "<TodoList><Todo><Title>買い物メモ</Title><Detail>牛乳と卵</Detail></Todo></TodoList>")]
        public void TODOを追加するとファイルにTODOが出力される(string title, string detail, string expected)
        {
            Execute("add", title, detail);
            var backup = XDocument.Load("backup.xml");
            Assert.That(backup.ToString(SaveOptions.DisableFormatting), Is.EqualTo(expected));
        }

        [TestCase("買い物メモ", "牛乳と卵", "タイトル: 買い物メモ\r\n詳細: 牛乳と卵\r\n")]
        [TestCase("買い物", "牛乳と卵を買う", "タイトル: 買い物\r\n詳細: 牛乳と卵を買う\r\n")]
        public void Todoを1つ追加した状態で最後に追加されたTODOの詳細が見れる(string title, string detail, string expected)
        {
            Execute("add", title, detail);
            var output = ExecuteAndReadStream(StreamKind.StandardOutput, "show", "last");
            Assert.That(output, Is.EqualTo(expected));
        }

        [TestCase("買い物メモ", "牛乳と卵", "タイトル: 買い物メモ\r\n詳細: 牛乳と卵\r\n")]
        [TestCase("買い物", "牛乳と卵を買う", "タイトル: 買い物\r\n詳細: 牛乳と卵を買う\r\n")]
        public void Todoを1つ追加した状態で最初に追加されたTODOの詳細が見れる(string title, string detail, string expected)
        {
            Execute("add", title, detail);
            var output = ExecuteAndReadStream(StreamKind.StandardOutput, "show", "first");
            Assert.That(output, Is.EqualTo(expected));
        }

        [TestFixture]
        public class Todoを2つ追加した状態
        {
            [SetUp]
            public void SetUp()
            {
                DeleteBackupIfExists();
                Execute("add", "買い物", "牛乳と卵");
                Execute("add", "買い物2", "筆記用具");
            }

            [Test]
            public void 追加されたTODO一覧が見れる()
            {
                var output = ExecuteAndReadStream(StreamKind.StandardOutput, "list");
                Assert.That(output, Is.EqualTo("1: 買い物\r\n2: 買い物2\r\n"));
            }

            [Test]
            public void 最初に追加したTODOの詳細が見れる()
            {
                var output = ExecuteAndReadStream(StreamKind.StandardOutput, "show", "first");
                Assert.That(output, Is.EqualTo("タイトル: 買い物\r\n詳細: 牛乳と卵\r\n"));
            }

            [Test]
            public void 最後に追加したTODOの詳細が見れる()
            {
                var output = ExecuteAndReadStream(StreamKind.StandardOutput, "show", "last");
                Assert.That(output, Is.EqualTo("タイトル: 買い物2\r\n詳細: 筆記用具\r\n"));
            }

            [Test]
            public void 最初に追加されたTODOを削除するとバックアップから消える()
            {
                var output = ExecuteAndReadStream(StreamKind.StandardOutput, "delete", "first");
                var backup = XDocument.Load("backup.xml");
                var expected = "<TodoList><Todo><Title>買い物2</Title><Detail>筆記用具</Detail></Todo></TodoList>";
                Assert.That(backup.ToString(SaveOptions.DisableFormatting), Is.EqualTo(expected));
            }

            [Test]
            public void 最後に追加されたTODOを削除するとバックアップから消える()
            {
                var output = ExecuteAndReadStream(StreamKind.StandardOutput, "delete", "last");
                var backup = XDocument.Load("backup.xml");
                var expected = "<TodoList><Todo><Title>買い物</Title><Detail>牛乳と卵</Detail></Todo></TodoList>";
                Assert.That(backup.ToString(SaveOptions.DisableFormatting), Is.EqualTo(expected));
            }

            [Test]
            public void 追加したすべてのTODOを削除するとバックアップが空になる()
            {
                var output = ExecuteAndReadStream(StreamKind.StandardOutput, "clear");
                var backup = XDocument.Load("backup.xml");
                var expected = "<TodoList />";
                Assert.That(backup.ToString(SaveOptions.DisableFormatting), Is.EqualTo(expected));
            }
        }
    }
}
