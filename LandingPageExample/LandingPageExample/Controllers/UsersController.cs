using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LandingPageExample.Models;

namespace LandingPageExample.Controllers
{
    public class UsersController : Controller
    {
        private UserAccountDBEntities db = new UserAccountDBEntities();

        // GET: Users
        public ActionResult Index()
        {
            if(Session["Login"].ToString() == "none")
            {
                return RedirectToAction("Login");
            }
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Login");
            }

            if (id == (int)Session["userId"])
            {
                User user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            else
            {
                return RedirectToAction("Login");
            }
            
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Users/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Id,UserName,Password")] User loginUser)
        {
            List<User> user = db.Users.Where(x => x.UserName == loginUser.UserName).ToList();

            if (user.Count == 0)
            {
                Session["login"] = "none";
                ViewBag.LoginSuccess = 0;
                ViewBag.ErrorMessage = "Unregistered user!";
                return View();
                // return RedirectToAction("Login", "Users");
            }
            else if (user[0].Password != loginUser.Password)
            {
                Session["login"] = "none";
                ViewBag.LoginSuccess = 0;
                ViewBag.ErrorMessage = "Incorrect password!";
                return View();
                // return RedirectToAction("Login", "Users");
            }
            else if (user[0].UserName == "admin")
            {
                Session["login"] = "admin";
                Session["userId"] = user[0].Id;
                ViewBag.LoginSuccess = 1;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["login"] = "guest";
                Session["userId"] = user[0].Id;
                ViewBag.LoginSuccess = 1;
                return RedirectToAction("Index", "Home");
            }
        }

        // Users/Logout
        public ActionResult Logout()
        {
            Session["userId"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        // Users/CheckName -- for Create User
        [HttpPost]
        public JsonResult CheckName(string name)
        {
            bool duplicateName;
            List<User> users = db.Users.Where(u => (u.UserName == name)).ToList();
            if (users.Count != 0)
            {
                duplicateName = true;
            }
            else
            {
                duplicateName = false;
            }
            return Json(duplicateName);
        }


    }
}
