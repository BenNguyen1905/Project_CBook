using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBook.Models;

namespace CBook.Controllers
{
    public class HomeController : Controller
    {
        Cbook_DatabaseEntities db = new Cbook_DatabaseEntities();
        Hang Hang = new Hang();
        public ActionResult Default()
        {
            return View();
        }
        //public ActionResult top()
        //{
        //    return View();
        //}

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

        public PartialViewResult SachGiaoDucPartialView()
        {
           
            
                var lstSachGiaoDuc = db.Hangs.Take(100).Where(x => x.MaLoai ==1).ToList();
                return PartialView(lstSachGiaoDuc);
            

        }
    }
}