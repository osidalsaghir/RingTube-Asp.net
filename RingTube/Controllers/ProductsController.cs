using RingTube.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RingTube.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        RingTubeModel db = new RingTubeModel();
        public ActionResult Index()
        {
            
            ViewBag.cat = db.categories.ToList();
            ViewBag.products = db.ringtones.ToList();
            return View();
        }
        
        public ActionResult SingleProduct(int?id)
        {
            ringtone request = db.ringtones.Where(d => d.id == id).FirstOrDefault();
            int urlCutID =request.urlCutID;
            ViewBag.SingleProduct = request;
            List<tag> tag = new List<tag>();
            var tags = db.ringtoneTags.Where(d => d.ringtoneID == request.id).ToList();

            foreach(var items in tags)
            {
                tag.AddRange(db.tags.Where(b => b.id == items.tagID).ToList());
            }

            ViewBag.tags = tag; 
            ViewBag.urlCutten = db.cutRingtones.Where(b => b.id == urlCutID).FirstOrDefault().cutUrl;
            ViewBag.getThree = db.ringtones.Where(d=>d.catID == request.catID && d.name != request.name).ToList().Take(3);

            return View();
        }

        public ActionResult Filter(int[] cats)
        {
            ViewBag.cat = db.categories.ToList();
            List<ringtone> ringall = new List<ringtone>();
            foreach (var items in cats)
            {

                ringall.AddRange(db.ringtones.Where(d => d.catID == items).ToList());

            }
            
            ViewBag.catsSelected = cats;
            ViewBag.products = ringall;
            return View("~/Views/Products/Index.cshtml");
        }

        [Authorize(Roles = "u,a")]
        public ActionResult AddToFav(int id)
        {
            string userEmail = HttpContext.User.Identity.Name;
            var userId = db.users.Where(d => d.email == userEmail).FirstOrDefault().id;
            favList fv = new favList();

            fv.ringtoneID = id;
            fv.userID = userId;
            db.favLists.Add(fv);
            db.SaveChanges();
            return RedirectToAction("Fav");
        }

        [Authorize(Roles = "u,a")]
        [HttpGet]
        public ActionResult Fav()
        {
            string userEmail = HttpContext.User.Identity.Name;
            List<ringtone> rings = new List<ringtone>();
            var userId = db.users.Where(d => d.email == userEmail).FirstOrDefault().id;
            List<favList> favListIDs = db.favLists.Where(d => d.userID == userId).ToList();
            
            foreach(var items in favListIDs)
            {

                rings.AddRange(db.ringtones.Where(d => d.id == items.ringtoneID).ToList());
            }
            ViewBag.FavList = rings; 
            return View();
        }

        [Authorize(Roles = "u,a")]
        public ActionResult DeleteFromFav(int id)
        {
            var del = db.favLists.Where(d => d.ringtoneID == id).FirstOrDefault();
            db.favLists.Remove(del);
            db.SaveChanges();
            return RedirectToAction("Fav");
        }

        
        [Authorize(Roles = "u,a")]
        public ActionResult AddToCart(int id)
        {
            string userEmail = HttpContext.User.Identity.Name;
            var userID = db.users.Where(d => d.email == userEmail).FirstOrDefault().id;
            shoppingCart sc = new shoppingCart();
            sc.userID = userID;
            sc.ringtoneID = id;
           

            db.shoppingCarts.Add(sc);
            db.SaveChanges();


            return RedirectToAction("Cart");
        }



        [Authorize(Roles = "u,a")]
        [HttpGet]
        public ActionResult Cart()
        {
            string userEmail = HttpContext.User.Identity.Name;
            List<ringtone> rings = new List<ringtone>();
            var userId = db.users.Where(d => d.email == userEmail).FirstOrDefault().id;
            List<shoppingCart> shoppingCartIDs = db.shoppingCarts.Where(d => d.userID == userId).ToList();
          
            foreach (var items in shoppingCartIDs)
            {

                rings.AddRange(db.ringtones.Where(d => d.id == items.ringtoneID).ToList());
            }
            ViewBag.Cart = rings;

            double price = 0; 
            foreach (var item in rings)
            {

                price = price + item.Price;
            }
            ViewBag.Total = price;
            return View();
        }

        [Authorize(Roles = "u,a")]
        public ActionResult DeleteFromCart(int id)
        {
            var del = db.shoppingCarts.Where(d => d.ringtoneID == id).FirstOrDefault();
            db.shoppingCarts.Remove(del);
            db.SaveChanges();
            return RedirectToAction("Cart");
        }

        [Authorize(Roles = "u,a")]
        public ActionResult Purchase()
        {
            string userEmail = HttpContext.User.Identity.Name;
            List<ringtone> rings = new List<ringtone>();
            var userId = db.users.Where(d => d.email == userEmail).FirstOrDefault().id;
            List<shoppingCart> shoppingCartIDs = db.shoppingCarts.Where(d => d.userID == userId).ToList();

            foreach (var items in shoppingCartIDs)
            {

                rings.AddRange(db.ringtones.Where(d => d.id == items.ringtoneID).ToList());
            }
            ViewBag.Cart = rings;

            double price = 0;
            foreach (var item in rings)
            {

                price = price + item.Price;
            }
            ViewBag.Total = price;
            return View();
            
        }

        [Authorize(Roles = "u,a")]
        [HttpGet]
        public ActionResult GetPurchase()
        {
            string userEmail = HttpContext.User.Identity.Name;
            List<ringtone> rings = new List<ringtone>();
            var userId = db.users.Where(d => d.email == userEmail).FirstOrDefault().id;
            List<shoppingCart> shoppingCartIDs = db.shoppingCarts.Where(d => d.userID == userId).ToList();

            purchased p = new purchased();
            
            foreach (var items in shoppingCartIDs)
            {
                double price = db.ringtones.Where(d => d.id == items.ringtoneID).First().Price;
                p.userID = userId;
                p.ringtoneID = items.ringtoneID;
                p.purchasedAt = DateTime.Now;
                p.PurchasedWithPrice = price;
                db.purchaseds.Add(p);
                db.SaveChanges();
                var del = db.shoppingCarts.Where(d => d.id == items.id).FirstOrDefault();
                db.shoppingCarts.Remove(del);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }




    }
}