using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MitProjectAssignment.Controllers
{
    public class SearchNoteController : Controller
    {
        // GET: SearchNote

        [Route("SearchNote")]
        public ActionResult SearchNote()
        {
            return View();
        }
    }
}