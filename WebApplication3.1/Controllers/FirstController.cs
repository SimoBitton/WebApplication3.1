using System;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using WebApplication3._1.Models;
using System.Net;

namespace WebApplication3._1.Controllers
{
    public class FirstController : Controller
    {

        static string[][] data;
        static int count = 0;
        // GET: First
        public ActionResult Index(string ip, int port)
        {
            Command.Instance.Start(ip, port);
            string[] details = Command.Instance.SendCommand();
            ViewBag.lon = float.Parse(details[0]);
            ViewBag.lat = float.Parse(details[1]);
            return View("Index");
        }

        public ActionResult Second(string ip, int port, int time)
        {
            Command.Instance.Start(ip, port);
            string[] details = Command.Instance.SendCommand();
            ViewBag.lon = float.Parse(details[0]);
            ViewBag.lat = float.Parse(details[1]);
            Session["time"] = time;
            return View();
        }

        public ActionResult Third(string ip, int port, int time, int sec, string fileName)
        {
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName + ".txt"))
            {
                System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName + ".txt");
            }
            Command.Instance.Start(ip, port);
            string[] details = Command.Instance.SendCommand();
            ViewBag.lon = float.Parse(details[0]);
            ViewBag.lat = float.Parse(details[1]);
            Session["time"] = time;
            Session["fileName"] = fileName;
            Session["sec"] = sec;
            return View();
        }

        public ActionResult FirstOrFourth(string ip, int port)
        {
            IPAddress number;
            if (IPAddress.TryParse(ip, out number))
                return Index(ip, port);
            return Fourth(ip, port);

        }

        public ActionResult Fourth(string fileName, int time)
        {
            string[] lines = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName + ".txt");
            data = new string[lines.Length][];
            int i = 0;
            foreach (string line in lines)
            {
                data[i] = line.Replace(" ", "").Split(';');
                i++;

            }
            ViewBag.lon = float.Parse(data[0][0]);
            ViewBag.lat = float.Parse(data[0][1]);
            Session["time"] = time;
            Session["fileName"] = fileName;
            return View("Fourth");
        }

        public string fileToXml()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Points");
            Random rnd = new Random();

            if (count < data.Length)
            {

                string lon = (float.Parse(data[count][0]) + rnd.Next(10, 40)).ToString();
                string lat = (float.Parse(data[count][1]) + rnd.Next(10, 40)).ToString();
                string rudder = data[count][2];
                string throttle = data[count][3];

                writer.WriteElementString("Lon", lon);
                writer.WriteElementString("Lat", lat);
                writer.WriteElementString("Rud", rudder);
                writer.WriteElementString("Throt", throttle);
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();

                count++;
            }
            return sb.ToString();

        }

        public ActionResult Default()
        {
            return View();
        }

        [HttpPost]
        public string generateXml()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Points");
            Random rnd = new Random();

            string[] details = Command.Instance.SendCommand();

            string lon = (float.Parse(details[0]) + rnd.Next(10, 40)).ToString();
            string lat = (float.Parse(details[1]) + rnd.Next(10, 40)).ToString();

            writer.WriteElementString("Lon", lon);
            writer.WriteElementString("Lat", lat);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            return sb.ToString();
        }

        [HttpPost]
        public string createFile()
        {
            Random rnd = new Random();
            string[] details = Command.Instance.SendCommand();
            System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + Session["fileName"].ToString() + ".txt", string.Join(";", details));

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Details");

            writer.WriteElementString("Lon", (float.Parse(details[0]) + rnd.Next(10, 40)).ToString());
            writer.WriteElementString("Lat", (float.Parse(details[1]) + rnd.Next(10, 40)).ToString());
            writer.WriteElementString("Rudder", details[2]);
            writer.WriteElementString("Throttle", details[3]);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            return sb.ToString();
        }
    }
}