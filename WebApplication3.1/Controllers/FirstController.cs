using System;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using WebApplication3._1.Models;
using System.Net;

namespace WebApplication3._1.Controllers
{

    /***
     * The controller Class -  responsible for controlling the way that the application
     * will react to Users requests.
       contains the flow control logic for the application
     * */

    

    public class FirstController : Controller
    {
        //static members - used at mission4
        static string[][] data;
        static int count = 0;
        // First Mission - connecting with the server and showing the Index view.
        public ActionResult Index(string ip, int port)
        {
            Command.Instance.Start(ip, port);
            string[] details = Command.Instance.SendCommand();
            ViewBag.lon = float.Parse(details[0]);
            ViewBag.lat = float.Parse(details[1]);
            return View("Index");
        }

        /***
         * Second Mission - connecting with the server 
         * Updates the csHtml with time argument
         * returns the View of "Second"
         ***/

        public ActionResult Second(string ip, int port, int time)
        {
            Command.Instance.Start(ip, port);
            string[] details = Command.Instance.SendCommand();
            ViewBag.lon = float.Parse(details[0]);
            ViewBag.lat = float.Parse(details[1]);
            Session["time"] = time;
            return View();
        }

        /***
      * Third Mission - Creating a txt file, named according to the given argument. 
      * connecting to the Server and recieves intial lat and lon from it.
      * Updates the csHtml with @time,@filename and @sec arguments.
      * returns the View of "Third".
      ***/

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
        /**
         * The function to differntiate between the similarity of mission1 and mission4 request parameters.
         * ***/
        public ActionResult FirstOrFourth(string ip, int port)
        {
            IPAddress number;
            if (IPAddress.TryParse(ip, out number))
                return Index(ip, port);
            return Fourth(ip, port);

        }


        /***
        * Fourt Mission - reading from txt file, named according to the given argument. 
        * Updates the csHtml with @time and @filename arguments.
        * returns the View of "Fourth".
        ***/

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

        /// <summary>
        /// with our static members that help us to read consecutivly from a txt file
        /// we now generate an XML file in order to present our txt file's content in 
        /// our appliction.
        /// </summary>
        /// <returns></returns>

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
        /// <summary>
        ///Control of Deafault View
        /// </summary>
        /// <returns></returns>
        public ActionResult Default()
        {
            return View();
        }


        /// <summary>
        /// this function is responsible of generating data from server 
        /// into an xml to be presented in our application
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// This function does maily two things :
        /// 1. extracts data from server and creates a txt file with the data.
        /// 2. generates XML with that data - to be presented in our Appliction.
        /// </summary>
        /// <returns></returns>
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