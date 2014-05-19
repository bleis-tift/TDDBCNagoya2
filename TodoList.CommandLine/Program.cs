using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TodoList.CommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = args[0];
            var title = args[1];
            var detail = args[2];
            var content = new XElement("TodoList",
                new XElement("Todo",
                    new XElement("Title", title),
                    new XElement("Detail", detail)));
            content.Save("backup.xml");
        }
    }
}
