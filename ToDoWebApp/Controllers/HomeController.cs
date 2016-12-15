using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoWebApp.Models;

namespace ToDoWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new ToDoDBContext().ToDoes);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            ToDo model;
            if (id == 0)
            {
                model = new ToDo();
            }
            else
            {
                model = new ToDoDBContext().ToDoes.Single(e => e.Id == id);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Detail(int id, ToDo model, FormCollection formcol)
        {
            if (formcol["Save"] == "Save")
            {
                var cx = new ToDoDBContext();
                if (id == 0)
                {
                    model.GId = Guid.NewGuid();
                    model.Created = DateTime.Now;
                    cx.ToDoes.Add(model);
                }
                else
                {
                    var p = cx.ToDoes.Single(e => e.Id == id);
                    p.Note = model.Note;
                    p.Category = model.Category;
                }
                cx.SaveChanges();
            }
            else if (formcol["Comment"] == "Comment")
            {
                if (formcol["CommentText"].Length > 0)
                {
                    var td = new ToDoComment();
                    td.Id = Guid.NewGuid();
                    td.ToDoGId = model.GId;
                    td.Created = DateTime.Now;
                    td.Comment = formcol["CommentText"];
                    new DocDBContext().InsertComment(td);
                    return RedirectToAction("Detail", new { id = id });
                }
            }


            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}