using RingTube.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RingTube.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        RingTubeModel db = new RingTubeModel();

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userEmail = HttpContext.User.Identity.Name;
                string userRole = Roles.GetRolesForUser(userEmail).FirstOrDefault();
                if (userRole == "u")
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (userRole == "a")
                {
                    return RedirectToAction("index", "AdminPanel");
                }
            }
            return View();
            
        }

        [HttpPost]
        public ActionResult SignIn(string email , string password )
        {
            try
            {
                var u = db.users.FirstOrDefault(x => x.email == email);
                if (u != null)
                {
                    if (u.password == password)
                    {
                        FormsAuthentication.SetAuthCookie(u.email, false);
                        string userRole = Roles.GetRolesForUser(u.email).FirstOrDefault();
                        if (userRole == "u")
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else if (userRole == "a")
                        {
                            return RedirectToAction("index", "AdminPanel");
                        }

                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            catch
            {
                return RedirectToAction("SignIn");
            }

            return RedirectToAction("Index");
        }

        
        public ActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SignUp(string email , string password, string name, string surname)
        {
            user u = new user();
            u.email = email;
            u.password = password;
            u.name = name;
            u.surname = surname;
            u.role = "u";
            db.users.Add(u);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "u,a")]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "u,a")]
        public ActionResult UserProfile()
        {
            string userEmail = HttpContext.User.Identity.Name;
            List<ringtone> rings = new List<ringtone>();
            var userId = db.users.Where(d => d.email == userEmail).FirstOrDefault().id;
            List<purchased> purchase = db.purchaseds.Where(d => d.userID == userId).ToList();

            foreach (var items in purchase)
            {

                rings.AddRange(db.ringtones.Where(d => d.id == items.ringtoneID).ToList());
            }
            ViewBag.ringtones = rings;

            return View();
        }


    }
}