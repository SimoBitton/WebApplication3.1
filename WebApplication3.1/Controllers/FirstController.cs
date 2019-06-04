using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
    }
}