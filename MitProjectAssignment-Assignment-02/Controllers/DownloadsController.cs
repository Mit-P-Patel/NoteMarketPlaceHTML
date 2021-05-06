using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MitProjectAssignment.Controllers
{
    public class DownloadsController : Controller
    {
        // GET: Downloads
        public ActionResult Downloads()
        {
            MitDatabaseEntities1 entities = new MitDatabaseEntities1();

            var prod = from prodt in entities.Downloads select prodt;

            return View(prod.ToList());
        }
    }
}