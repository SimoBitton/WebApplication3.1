using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.IO;
using WebApplication3._1.Models;

namespace WebApplication3._1.Controllers
{
    public class FirstController : Controller
    {
        // GET: First
        public ActionResult Index(string ip, int port)
        {
            Command.Instance.Start(ip, port);
            string[] details = Command.Instance.SendCommand();
            ViewBag.lon = float.Parse(details[0]);
            ViewBag.lat = float.Parse(details[1]);
            return View();
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

        public ActionResult Third(string ip, int port, int time, int sec)
        {
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\" + "flight1.txt"))
            {
                System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\" + "flight1.txt");
            }
            Command.Instance.Start(ip, port);
            Session["time"] = time;
            Session["sec"] = sec;
            return View();
        }

        public ActionResult Default()
        {
            return View();
        }

        [HttpPost]
        public string generateXml()
        {
            //Initiate XML stuff
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
            System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + "flight1.txt", string.Join("; ", details));

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