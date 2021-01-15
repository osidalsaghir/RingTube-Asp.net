using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NAudio.Wave;
using RingTube.Models;

namespace RingTube.Controllers
{
    [Authorize(Roles = "a")]
    public class AdminPanelController : Controller
    {
        // GET: AdminPanel
        RingTubeModel db = new RingTubeModel();
        public ActionResult Index()
        {
            ViewBag.cat = db.categories.ToList();
            ViewBag.tag = db.tags.ToList();
            return View();
        }

        public ActionResult Table(string miss)
        {
            ViewBag.Mes = miss;
            ViewBag.ringtones = db.ringtones.ToList();
            ViewBag.cat = db.categories.ToList();
            ViewBag.tag = db.ringtoneTags.ToList();
            ViewBag.CuttenUrl = db.cutRingtones.ToList();
            ViewBag.tags = db.tags.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file ,string auther,string name , string description, int category , int price ,int[] tagnames)
        { //hoon wselet
           
            var getPath = Server.MapPath("~");
           
            var fileNameSaver = getPath + @"Content\mp3\" + DateTime.Now.ToString("HHmmss") + file.FileName;
            var newName = getPath + @"Content\mp3\" + DateTime.Now.ToString("HHmmss") +"Cutted"+ file.FileName;
            file.SaveAs(fileNameSaver);
            var savedPath = newName;
            var trimmed = new AudioFileReader(fileNameSaver)
                    .Skip(TimeSpan.FromSeconds(2))
                    .Take(TimeSpan.FromSeconds(3));
            WaveFileWriter.CreateWaveFile16(savedPath, trimmed);

           

            cutRingtone cut = new cutRingtone();
            var s = @"Content/mp3/" + DateTime.Now.ToString("HHmmss") + "Cutted" + file.FileName;
            cut.cutUrl = @"Content/mp3/" + DateTime.Now.ToString("HHmmss") + "Cutted" + file.FileName;
            db.cutRingtones.Add(cut);
            db.SaveChanges();
            var id = db.cutRingtones.Where(d => d.cutUrl == s).First().id;



            ringtone ring = new ringtone();

            ring.name = name;
            ring.Price = price;
            ring.auther = auther;
            ring.urlCutID = id;
            ring.catID = category;
            ring.dis = description;
            ring.url =  @"Content/mp3/" + DateTime.Now.ToString("HHmmss") + file.FileName;
            db.ringtones.Add(ring);
            db.SaveChanges();

    
         
            var ringtoneID = db.ringtones.Where(b => b.urlCutID == id).First().id;

            ringtoneTag t = new ringtoneTag();

            foreach (var item in tagnames)
            {
                t.tagID = item;
                t.ringtoneID = ringtoneID;
                db.ringtoneTags.Add(t);
                db.SaveChanges();

            }




            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Category(string name)
        {
            category c = new category();
            c.name = name;
            db.categories.Add(c);
            db.SaveChanges();
           
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Tag(string name)
        {
            tag t = new tag();
            t.name = name;
            db.tags.Add(t);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteRingtone(int id)
        {
            var rt = db.ringtoneTags.Where(d => d.ringtoneID == id).ToList();
            var pur = db.purchaseds.Where(d => d.ringtoneID == id).ToList();
            var cart = db.shoppingCarts.Where(d => d.ringtoneID == id).ToList();

          

            db.shoppingCarts.RemoveRange(cart);
            db.purchaseds.RemoveRange(pur);
            db.ringtoneTags.RemoveRange(rt);

            db.SaveChanges();

            var ringo = db.ringtones.Where(d => d.id == id).ToList();
            db.ringtones.RemoveRange(ringo);
            db.SaveChanges();

            return RedirectToAction("Table", new { miss = "" });
        }

        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            var ringtones = db.ringtones.Where(d => d.catID == id).ToList();
            string show = ""; 
            if(ringtones.Count > 0)
            {
                show = "The Category can't be deleted before deleting all the ringtones related to it";
                return RedirectToAction("Table" ,new { miss = show });
            }
            else
            {
                var cat = db.categories.Where(d => d.id == id).FirstOrDefault();



                db.categories.Remove(cat);


                db.SaveChanges();
                return RedirectToAction("Table", new { miss = show });
            }




           
        }
        [HttpGet]
        public ActionResult EditTag(int id)
        {
            ViewBag.name = db.tags.Where(d => d.id == id).FirstOrDefault();
            return View();
        }
        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            ViewBag.name = db.categories.Where(d => d.id == id).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public ActionResult EditTag(string name,int id)
        {
            tag tag = db.tags.Where(d => d.id == id).FirstOrDefault();
            tag.name = name;
            db.tags.AddOrUpdate(tag);
            db.SaveChanges();
            return RedirectToAction("Table", new { miss = "" });
        }
        [HttpPost]
        public ActionResult EditCategory(string name, int id)
        {
            category cat = db.categories.Where(d => d.id == id).FirstOrDefault();
            cat.name = name;
            db.categories.AddOrUpdate(cat);
            db.SaveChanges();
            return RedirectToAction("Table", new { miss = "" });
        }

        [HttpGet]
        public ActionResult EditRingtone(int id)
        {
            ViewBag.ringtone = db.ringtones.Where(d => d.id == id).FirstOrDefault();
            ViewBag.cat = db.categories.ToList();
            ViewBag.tag = db.tags.ToList();
            return View();
        }


        [HttpPost]
        public ActionResult EditRingtone(int id,string auther, string name, string description, int category, int price, int[] tagnames)
        {
            ringtone ring = db.ringtones.Where(d => d.id == id).FirstOrDefault();
            ring.name = name;
            ring.dis = description;
            ring.Price = price;
            ring.catID = category;
            ring.auther = auther;
            db.ringtones.AddOrUpdate(ring);
            db.SaveChanges();

            List<ringtoneTag> s = new List<ringtoneTag>();
            s = db.ringtoneTags.Where(d => d.ringtoneID == id).ToList();


            db.ringtoneTags.RemoveRange(s);
            db.SaveChanges();
            ringtoneTag t = new ringtoneTag();
            foreach (var item in tagnames)
            {
                t.tagID = item;
                t.ringtoneID = id;
                db.ringtoneTags.Add(t);
                db.SaveChanges();

            }


            return RedirectToAction("Table", new { miss = "" });
        }

        [HttpGet]
        public ActionResult DeleteTag(int id)
        {

            var tags = db.ringtoneTags.Where(d => d.tagID == id).ToList();
            string show = "";
            if (tags.Count > 0)
            {
                show = "The Tag can't be deleted before deleting all the ringtones related to it";
               
                return RedirectToAction("Table", new { miss = show });
            }
            else
            {
                var tag = db.tags.Where(d => d.id == id).FirstOrDefault();



                db.tags.Remove(tag);


                db.SaveChanges();
            
                return RedirectToAction("Table", new { miss = show });
            }
        }


        [HttpGet]
        public ActionResult Users()
        {
            var Model = db.users.ToList();
            return View(Model);
        }

        [HttpGet]
        public ActionResult PurchasedRingtones()
        {
            var Model = db.purchaseds.ToList();

            return View(Model);
        }

       
    }
}