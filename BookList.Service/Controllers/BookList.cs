using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookList.Service.Controllers
{
    public class BookList : Controller
    {
        public ActionResult Index()
        {
            return View ();
        }
    }
}
