using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using RingTube.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace RingTube.Controllers
{
   
    public class HomeController : Controller
    {
        RingTubeModel db = new RingTubeModel();
        public ActionResult Index()
        {



            ViewBag.ringtone = db.ringtones.ToList().Take(3);
            ViewBag.ringtoneCutten = db.cutRingtones.ToList();
            return View();
        }

       


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}