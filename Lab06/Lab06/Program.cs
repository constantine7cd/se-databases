using System;
using System.Xml;
using System.IO;

namespace Lab06
{
    class XmlTool
    {
        private XmlDocument doc;

        public XmlTool()
        {
            this.doc = new XmlDocument();
        }

        //first
        public void readXml(String path)
        {
            FileStream file = new FileStream(path, FileMode.Open);
            doc.Load(file);

            file.Close();
        }

        //second
        public void findByName(String tagName)
        {
            XmlNodeList tagList = doc.GetElementsByTagName(tagName);

            if (tagList == null)
                throw (new Exception("Not found any tag!\n"));

            Console.Write(tagList.Count);
           

            for (int i = 0; i < tagList.Count; i++)
            {
                Console.Write(tagList[i].Name + "\n" + "----------------\n\n");
            }

        }

        public void findById(String Id)
        {
            XmlElement id = doc.GetElementById(Id);

            if (id == null)
                throw (new Exception("Not found any Id!\n"));

            Console.Write(id.InnerXml + "\n" + "----------------------\n\n");


        }

        public void selectNodes(String Xpath)
        {
            XmlNodeList res = doc.SelectNodes(Xpath);

            if (res == null)
                throw (new Exception("Can't find nodes by this path!\n"));

            Console.Write(res.Count);

            for (int i = 0; i < res.Count; i++)
                Console.Write(res[i].InnerXml + "\n" + "----------------------\n\n");
        }

        public void selectSingleNode(String Xpath)
        {
            XmlNode res = doc.SelectSingleNode(Xpath);

            if (res == null)
                throw (new Exception("Can't find nodes by this path!\n"));

            Console.Write(res.InnerXml + "\r\n");
        }

        //third
        public XmlElement getXmlElementById(String Id)
        {
            XmlElement res = doc.GetElementById(Id);

            if (res == null)
                throw new Exception("Not found any Id!");

            return res;
        }

        public XmlElement getXmlElementByPath(String Xpath)
        {
            XmlNode res = doc.SelectSingleNode(Xpath);

            if (res == null)
                throw (new Exception("Can't find nodes by this path!\n"));

            return (XmlElement)res;
        }

        public String getElemName(XmlElement elem)
        {
            return  elem.Name;
        }

        public String getComments()
        {
            String res = "";

            foreach (XmlComment comment in doc.SelectNodes("//comment()"))
            {
                res += comment.Value + "\n";
            }

            return res;
        }

        public String getProcessingInstr()
        {
            XmlProcessingInstruction instruction = doc.SelectSingleNode("processing-instruction('xml-stylesheet')") as XmlProcessingInstruction;

            if (instruction == null)
                throw new Exception("Can't find processing instructions\n");

            return instruction.Data;
        }

        public String getElemAttrs(XmlElement elem)
        {
            String res = "";

            foreach (XmlAttribute n in elem.Attributes)
                res += n.Value;

            return res;
        }

        //fourth
        public void deleteLastNode(String output)
        {
            XmlElement root = doc.DocumentElement;
            XmlElement toDelete = (XmlElement)root.ChildNodes[root.ChildNodes.Count - 1];

            root.RemoveChild(toDelete);

            doc.Save(output);
        }

        //XPath
        public void changeCause(String output, String Value)
        {
            XmlElement elem = getXmlElementByPath("/root/Drivers/Fines[@Cause = 'red traffic light']");

            elem.Attributes[1].Value = "red traffic light or waffle";

            doc.Save(output);
        }

        public void addDriver(String output, String Did, String FineId, String Cause, String Cost)
        {
            XmlElement driver = doc.CreateElement("Drivers");
            XmlElement fines = doc.CreateElement("Fines");
            XmlElement fcost = doc.CreateElement("FCost");

            XmlAttribute did = doc.CreateAttribute("Did");
            did.Value = Did;

      
            XmlAttribute fineid = doc.CreateAttribute("FineId");
            fineid.Value = FineId;

            XmlAttribute cause = doc.CreateAttribute("Cause");
            cause.Value = Cause;


            XmlAttribute cost = doc.CreateAttribute("Cost");
            cost.Value = Cost;

            driver.Attributes.Append(did);
            fines.Attributes.Append(fineid);
            fines.Attributes.Append(cause);
            fcost.Attributes.Append(cost);

            fines.AppendChild(fcost);
            driver.AppendChild(fines);

            doc.DocumentElement.AppendChild(driver);

            doc.Save(output);
        }

        public void changeToComment(XmlNode Parent)
        {
            foreach (XmlNode n in Parent.ChildNodes)
            {
                changeToComment(n);      
            }

            if (Parent.GetType().ToString() == "System.Xml.XmlProcessingInstruction")
            {
                Console.Write(Parent.Name + "\n");
                XmlComment elem = doc.CreateComment(Parent.Name);
                doc.ReplaceChild(elem, Parent); 
            }
        }

        public void chComm()
        {
            foreach (XmlNode n in doc.ChildNodes)
            {
                changeToComment(n);

            }

            doc.Save(@"C:\Users\Konstantin\Documents\db\Lab06\Lab06\drivers_comm.xml");


        }
    }




    class consoleApp
    {
        
        static void Main(string[] args)
        {
            try
            {
                XmlTool tool = new XmlTool();

                tool.readXml(@"C:\Users\Konstantin\Documents\db\Lab06\Lab06\drivers2.xml");

                //tool.findByName("?xml-stylesheet");
                //tool.findById("A33");
                //tool.selectNodes("//xsl()");
                //tool.selectSingleNode("/root/Drivers/Fines");

                //Console.Write(tool.getElemName(tool.getXmlElementById("A1")));
                //Console.Write(tool.getComments());
                //Console.Write(tool.getProcessingInstr());
                //Console.Write(tool.getElemAttrs(tool.getXmlElementById("A1")));

                //tool.deleteLastNode(@"C:\Users\Konstantin\Documents\db\Lab06\Lab06\drivers_d.xml");
                //tool.changeCause(@"C:\Users\Konstantin\Documents\db\Lab06\Lab06\drivers_c.xml","red traffic light or waffle");
                //tool.addDriver(@"C:\Users\Konstantin\Documents\db\Lab06\Lab06\drivers_add.xml", "101", "34", "light", "3456");

                tool.chComm();
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            Console.ReadLine();

        }
    }
}