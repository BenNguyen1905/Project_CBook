using CBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBook.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        Cbook_DatabaseEntities db = new Cbook_DatabaseEntities();
        public ActionResult KetQuaTimKiem(FormCollection f)
        {
            String sTuKhoa = f["txtSearch"].ToString();
            
            return View();
        }
    }
}