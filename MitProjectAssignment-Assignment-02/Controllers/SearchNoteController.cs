
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity;

namespace MitProjectAssignment.Controllers
{
    public class SearchNoteController : Controller
    {
        // GET: SearchNote

        MitDatabaseEntities1 dbobj = new MitDatabaseEntities1();

        
        public ActionResult SearchNotes(string search, int? page)
        {

            var emailid = User.Identity.Name.ToString();
            Registration obj = dbobj.Registration.Where(x => x.Email == emailid).FirstOrDefault();

            System.Linq.IQueryable<SellerNotes> Books;     //Empty Variable for Holding Notes

            Books = dbobj.SellerNotes.ToList().AsQueryable();


            ViewBag.Type = new SelectList(dbobj.SellerNotes.Where(x => x.IsActive).Select(x => x.NoteTypes).Distinct().ToList(), "ID", "Name");
            ViewBag.Category = new SelectList(dbobj.SellerNotes.Where(x => x.IsActive).Select(x => x.NoteCategories).Distinct().ToList(), "ID", "Name");
            ViewBag.Univercity = new SelectList(dbobj.SellerNotes.Where(x => x.IsActive).Select(x => x.UniversityName).Distinct().ToList());
            ViewBag.Course = new SelectList(dbobj.SellerNotes.Where(x => x.IsActive).Select(x => x.Course).Distinct().ToList());
            ViewBag.Country = new SelectList(dbobj.SellerNotes.Where(x => x.IsActive).Select(x => x.Countrie).Distinct().ToList(), "ID", "Name");

            return View(dbobj.SellerNotes.ToList().ToPagedList(page ?? 1, 5));
        }
    }
}