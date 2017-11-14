using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Xml.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            XDocument changesetDB = new XDocument(
                    new XElement("changes",
                            new XElement("change",
                                new XAttribute("path", "asd"),
                                new XAttribute("changesetID", "ssd"),
                                new XAttribute("JIRAID", "sssss"))));
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            changesetDB.Save(Path.Combine(path, "worm.config"));
            var test = HostingEnvironment.MapPath("~");
        }
    }
}
