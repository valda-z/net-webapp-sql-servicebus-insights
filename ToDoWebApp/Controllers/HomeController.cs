using RestSharp;
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

        private string getIP(HttpRequestBase request)
        {
            if (request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
            {
                return "not-found";
            }
            else
            {
                return request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(':')[0];
            }
        }
        private string getGeoIPData(string ip)
        {
            try
            {
                if (ip == "::1" || ip == "not-found")
                {
                    ip = "46.228.25.202";
                }
                var client = new RestClient(string.Format("http://{0}:5000", Properties.Settings.Default.GEOIPSERVER));
                var request = new RestRequest("/api/ip?addr=" + ip, Method.GET);
                var queryResult = client.Execute<GeoIPHelper.RootObject>(request).Data;
                if (queryResult == null)
                {
                    return string.Format(" Your IP [{0}] - VNET not connected", ip);
                }
                else if (queryResult.country == null)
                {
                    return string.Format(" Your IP [{0}] - invalid search argument", ip);
                }
                else
                {
                    return string.Format(" Your IP [{0}] - {1}, {2}, {3}/{4}",
                        ip,
                        queryResult.country.names.en,
                        (queryResult.city.names == null ? "Unknown-City" : queryResult.city.names.en),
                        queryResult.location.latitude,
                        queryResult.location.longitude);
                }
            }catch(Exception ex)
            {
                return ex.ToString();
            }
        }


        public ActionResult Index()
        {
            ViewBag.Geoip = getGeoIPData(getIP(this.Request));
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

                //Send to sservice bus
                ServiceBusHelper.SendToAll(model);

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