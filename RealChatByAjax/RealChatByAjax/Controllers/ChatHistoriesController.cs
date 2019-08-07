using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RealChatByAjax.Models;

namespace RealChatByAjax.Controllers
{
    public class ChatHistoriesController : Controller
    {
        private ChatMessageDBEntities db = new ChatMessageDBEntities();

        // GET: ChatHistories/Select
        public ActionResult Select()
        {
            return View();
        }

        // POST: ChatHistories/Select
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Select(string sender)
        {
            Session["senderIdentity"] = sender;
            return RedirectToAction("Create", "ChatHistories");
        }

        // -- AJAX to get chat messages --
        public ActionResult GetMessage(string receiver)
        {
            string senderIdentity = Session["senderIdentity"] as string;

            List<ChatHistory> messages = new List<ChatHistory>();

            messages = db.ChatHistories.Where(x => (x.Sender == senderIdentity && x.Receiver == receiver) || (x.Receiver == senderIdentity && x.Sender == receiver)).ToList();

            return Json(messages.OrderByDescending(x => x.DateTime), JsonRequestBehavior.AllowGet);
        }

        // -- AJAX to create chat message --
        [HttpPost]
        public ActionResult CreateMessage(ChatHistory chatHistory)
        {
            string message;
            if (chatHistory.Content == null)
            {
                message = "ERROR";
                return Json(new { Message = message, JsonRequestBehavior.AllowGet });
            }
            else
            {
                chatHistory.DateTime = DateTime.Now;

                db.ChatHistories.Add(chatHistory);
                db.SaveChanges();
                message = "SUCCESS";
                return Json(new { Message = message, JsonRequestBehavior.AllowGet });
            }
        }

        // GET: ChatHistories/Create
        public ActionResult Create()
        {
            string senderIdentity = Session["senderIdentity"] as string;

            // -- alternative --
            //var messages = from m in db.ChatHistories where (m.FromName == fromName) || (m.ToName == fromName) select m;
            //return View(messages.ToList());

            var messages = db.ChatHistories.Where(x => x.Sender == senderIdentity || x.Receiver == senderIdentity).ToList();
            return View(messages);
        }

        // GET: ChatHistories
        public ActionResult Search(string searchString, string receiver)
        {
            string senderIdentity = Session["senderIdentity"] as string;

            List<ChatHistory> messages = new List<ChatHistory>();

            //messages = db.ChatHistories.Where(x => (x.Sender == senderIdentity || x.Receiver == senderIdentity) && x.Content.Contains(searchString)).ToList();

            if (receiver == "")
            {
                messages = db.ChatHistories.Where(x => (x.Sender == senderIdentity || x.Receiver == senderIdentity) && x.Content.Contains(searchString)).ToList();
            }
            else
            {
                messages = db.ChatHistories.Where(x => ((x.Sender == senderIdentity && x.Receiver == receiver) || (x.Receiver == senderIdentity && x.Sender == receiver)) && x.Content.Contains(searchString)).ToList();
            }

            return View("Search", messages);
        }

        // GET: ChatHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatHistory chatHistory = db.ChatHistories.Find(id);
            if (chatHistory == null)
            {
                return HttpNotFound();
            }
            return View(chatHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
