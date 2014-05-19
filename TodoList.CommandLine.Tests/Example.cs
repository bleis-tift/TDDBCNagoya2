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
        [SetUp]
        public void SetUp()
        {
            // TearDownで消してしまうと、テスト失敗時にbackup.xmlの調査ができなくなって困るので、テスト開始時に消すことにする
            if (File.Exists("backup.xml"))
                File.Delete("backup.xml");
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

        [TestCase("買い物", "牛乳と卵を買う", "<TodoList><Todo><Title>買い物</Title><Detail>牛乳と卵を買う</Detail></Todo></TodoList>")]
        [TestCase("買い物メモ", "牛乳と卵", "<TodoList><Todo><Title>買い物メモ</Title><Detail>牛乳と卵</Detail></Todo></TodoList>")]
        public void TODOを追加するとファイルにTODOが出力される(string title, string detail, string expected)
        {
            Execute("add", title, detail);
            var backup = XDocument.Load("backup.xml");
            Assert.That(backup.ToString(SaveOptions.DisableFormatting), Is.EqualTo(expected));
        }

        [Test]
        public void 初期状態で最後に追加されたTODOを見ようとするとエラーが表示される()
        {
            var output = ExecuteAndReadStream(StreamKind.StandardError, "show", "last");
            Assert.That(output, Is.EqualTo("エラー: TODOがありません。\r\n"));
        }

        [TestCase("買い物メモ", "牛乳と卵", "タイトル: 買い物メモ\r\n詳細: 牛乳と卵\r\n")]
        [TestCase("買い物", "牛乳と卵を買う", "タイトル: 買い物\r\n詳細: 牛乳と卵を買う\r\n")]
        public void Todoを1つ追加した状態で最後に追加されたTODOの詳細が見れる(string title, string detail, string expected)
        {
            Execute("add", title, detail);
            var output = ExecuteAndReadStream(StreamKind.StandardOutput, "show", "last");
            Assert.That(output, Is.EqualTo(expected));
        }

        [Test]
        public void Todoを2つ追加した状態で最後に追加されたTODOの詳細が見れる()
        {
            Execute("add", "買い物", "牛乳と卵");
            Execute("add", "買い物2", "筆記用具");
            var output = ExecuteAndReadStream(StreamKind.StandardOutput, "show", "last");
            Assert.That(output, Is.EqualTo("タイトル: 買い物2\r\n詳細: 筆記用具\r\n"));
        }
    }
}
