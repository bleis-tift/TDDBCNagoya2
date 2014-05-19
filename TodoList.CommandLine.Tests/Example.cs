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

        [TestCase("買い物", "牛乳と卵を買う", "<TodoList><Todo><Title>買い物</Title><Detail>牛乳と卵を買う</Detail></Todo></TodoList>")]
        [TestCase("買い物メモ", "牛乳と卵", "<TodoList><Todo><Title>買い物メモ</Title><Detail>牛乳と卵</Detail></Todo></TodoList>")]
        public void TODOを追加するとファイルにTODOが出力される(string title, string detail, string expected)
        {
            var procInfo = new ProcessStartInfo("TodoList.exe", string.Format("add {0} {1}", title, detail));
            using (var proc = Process.Start(procInfo))
            {
                proc.WaitForExit();
                var backup = XDocument.Load("backup.xml");
                Assert.That(backup.ToString(SaveOptions.DisableFormatting), Is.EqualTo(expected));
            }
        }

        [Test]
        public void 初期状態で最後に追加されたTODOを見ようとするとエラーが表示される()
        {
            var procInfo = new ProcessStartInfo("TodoList.exe", "show last")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true
            };
            using (var proc = new Process { StartInfo = procInfo })
            {
                proc.Start();
                var output = proc.StandardError.ReadToEnd();
                Assert.That(output, Is.EqualTo("エラー: TODOがありません。\r\n"));
            }
        }

        [TestCase("買い物メモ", "牛乳と卵", "タイトル: 買い物メモ\r\n詳細: 牛乳と卵\r\n")]
        [TestCase("買い物", "牛乳と卵を買う", "タイトル: 買い物\r\n詳細: 牛乳と卵を買う\r\n")]
        public void Todoを1つ追加した状態で最後に追加されたTODOの詳細が見れる(string title, string detail, string expected)
        {
            var procInfo = new ProcessStartInfo("TodoList.exe", string.Format("add {0} {1}", title, detail));
            using (var proc = Process.Start(procInfo))
            {
                proc.WaitForExit();
            }

            procInfo = new ProcessStartInfo("TodoList.exe", "show last")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            };
            using (var proc = new Process { StartInfo = procInfo })
            {
                proc.Start();
                var output = proc.StandardOutput.ReadToEnd();
                Assert.That(output, Is.EqualTo(expected));
            }
        }
    }
}
