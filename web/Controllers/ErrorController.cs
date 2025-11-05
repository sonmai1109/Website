using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nhom3.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult PageNotFound()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }
    }
}