using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MitProjectAssignment.Controllers
{
    public class UserDashController : Controller
    {
        // GET: UserDash
        public ActionResult Index()
        {
            return View();
        }
    }
}