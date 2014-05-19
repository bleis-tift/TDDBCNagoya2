using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;
using System.Xml.Linq;

namespace TodoList.CommandLine.Tests
{
    [TestFixture]
    public class Example
    {
        [Test]
        public void TODOを追加するとファイルにTODOが出力される()
        {
            var procInfo = new ProcessStartInfo("TodoList.exe", "add 買い物 牛乳と卵を買う");
            using (var proc = Process.Start(procInfo))
            {
                proc.WaitForExit();
                var backup = XDocument.Load("backup.xml");
                Assert.That(
                    backup.ToString(SaveOptions.DisableFormatting),
                    Is.EqualTo("<TodoList><Todo><Title>買い物</Title><Detail>牛乳と卵を買う</Detail></Todo></TodoList>"));
            }
        }
    }
}
