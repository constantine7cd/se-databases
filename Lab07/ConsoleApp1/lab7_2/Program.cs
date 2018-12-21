using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace lab7_2 // XML
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //Read();
            //Modify();
            Write();
            Console.ReadLine();
        }

        static void Read()
        {
            XDocument xdoc = XDocument.Load(@"C:\Users\Konstantin\Documents\db\Lab07\ConsoleApp1\lab7_2\cars.xml");
            var query = from cars in xdoc.Descendants("row")
                        where cars.Attribute("CarBody").Value == "suv"
                        select new {Name =  cars.Attribute("CarName").Value, Vin = cars.Attribute("CarVin").Value };
            foreach (var q in query)
            {
                Console.WriteLine("{0}  {1}", q.Name, q.Vin);
            }
            Console.ReadLine();
        }

        static void Modify()
        {
            XDocument xdoc = XDocument.Load(@"C:\Users\Konstantin\Documents\db\Lab07\ConsoleApp1\lab7_2\cars.xml");
            XNode node = xdoc.Root.FirstNode;

            int idx = 0;

            while (node != null)
            {
                if (node.NodeType == System.Xml.XmlNodeType.Element)
                {
                    XElement el = (XElement)node;
                    el.Attribute("CarId").Value = idx.ToString();
                    idx++;
                }
                node = node.NextNode;
            }
            xdoc.Save(@"C:\Users\Konstantin\Documents\db\Lab07\ConsoleApp1\lab7_2\cars1.xml");
        }
        
        static void Write()
        {
            XDocument xdoc = XDocument.Load(@"C:\Users\Konstantin\Documents\db\Lab07\ConsoleApp1\lab7_2\cars.xml");
            int maxId = xdoc.Root.Elements("row").Max(t => Int32.Parse(t.Attribute("CarId").Value));
            XElement newCar = new XElement("row",
                new XAttribute("CarId", ++maxId),
                new XAttribute("CarName", "maybach"),
                new XAttribute("CarVin", "wdd8bb8"),
                new XAttribute("CarDate", "2004 - 08 - 07"),
                new XAttribute("CarBody", "hatch"));
            xdoc.Root.Add(newCar);
            xdoc.Save(@"C:\Users\Konstantin\Documents\db\Lab07\ConsoleApp1\lab7_2\cars2.xml");
        }
    }
}



