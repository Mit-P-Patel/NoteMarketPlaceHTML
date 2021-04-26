
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using System.IO.Compression;
using System.Linq;

using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Net;
using System.Net.Mail;


using System.Web.UI.WebControls;
using MitProjectAssignment.Models;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;





namespace MitProjectAssignment.Controllers
{
    public class AddNotesController : Controller
    {


        MitDatabaseEntities1 Context = new MitDatabaseEntities1();

        [HttpGet]
        [Route("AddNotes")]
        [Authorize]

        public ActionResult AddNotes()
        {
            // create add note viewmodel and set values in dropdown list

            AddNotesViewModels viewModel = new AddNotesViewModels
            {
                NoteCategoryList = Context.NoteCategories.ToList(),
                NoteTypeList = Context.NoteTypes.ToList(),
                CountryList = Context.Countrie.ToList()
            };

            return View(viewModel);

        }

        [HttpPost]
        [Route("AddNotes")]
        [Authorize]
        public ActionResult AddNotes(AddNotesViewModels xyz)
        {

            if (ModelState.IsValid)
            {
                // create seller note object
                SellerNotes abc = new SellerNotes();


                var user = Context.Registration.FirstOrDefault(x => x.Email == User.Identity.Name);
                abc.Status = Context.ReferenceData.Where(x => x.Value.ToLower() == "draft").Select(x => x.ID).FirstOrDefault();
                abc.SellerID = user.UserId;
                abc.Title = xyz.Title.Trim();

                abc.Category = xyz.Category;
                abc.NoteType = xyz.NoteType;
                abc.NumberofPages = xyz.NumberofPages;
                abc.Description = xyz.Description.Trim();
                abc.UniversityName = xyz.UniversityName.Trim();
                abc.Country = xyz.Country;
                abc.Course = xyz.Course.Trim();
                abc.CourseCode = xyz.CourseCode.Trim();
                abc.Professor = xyz.Professor.Trim();
                abc.IsPaid = xyz.IsPaid;
                if (abc.IsPaid)
                {
                    abc.SellingPrice = xyz.SellingPrice;
                }
                else
                {
                    abc.SellingPrice = 0;
                }

                abc.IsActive = true;
                Context.SellerNotes.Add(abc);

                try
                {
                    Context.SaveChanges();
                }
                catch (Exception ex)
                {

                }
                //


                // add note in database and save




                // get seller note
                abc = Context.SellerNotes.Find(abc.ID);

                // if display picture is not null then save picture into directory and directory path into database
                if (xyz.DisplayPicture != null)
                {
                    string displaypicturefilename = System.IO.Path.GetFileName(xyz.DisplayPicture.FileName);
                    string displaypicturepath = "~/Members/" + user.UserId + "/" + abc.ID + "/";
                    CreateDirectoryIfMissing(displaypicturepath);
                    string displaypicturefilepath = Path.Combine(Server.MapPath(displaypicturepath), displaypicturefilename);
                    abc.DisplayPicture = displaypicturepath + displaypicturefilename;
                    xyz.DisplayPicture.SaveAs(displaypicturefilepath);
                }

                // if note preview is not null then save picture into directory and directory path into database
                if (xyz.NotesPreview != null)
                {
                    string notespreviewfilename = System.IO.Path.GetFileName(xyz.NotesPreview.FileName);
                    string notespreviewpath = "~/Members/" + user.UserId + "/" + abc.ID + "/";
                    CreateDirectoryIfMissing(notespreviewpath);
                    string notespreviewfilepath = Path.Combine(Server.MapPath(notespreviewpath), notespreviewfilename);
                    abc.NotesPreview = notespreviewpath + notespreviewfilename;
                    xyz.NotesPreview.SaveAs(notespreviewfilepath);
                }

                // update note preview path and display picture path and save changes
             //   Context.SellerNotes.Add(abc);
                //Context.Entry(abc).Property(x => x.DisplayPicture).IsModified = true;
                //Context.Entry(abc).Property(x => x.NotesPreview).IsModified = true;
                Context.SaveChanges();

                // attachement files
                foreach (HttpPostedFileBase file in xyz.UploadNotes)
                {
                    // check if file is null or not
                    if (file != null)
                    {
                        // save file in directory
                        string notesattachementfilename = System.IO.Path.GetFileName(file.FileName);
                        string notesattachementpath = "~/Members/" + user.UserId + "/" + abc.ID + "/Attachements/";
                        CreateDirectoryIfMissing(notesattachementpath);
                        string notesattachementfilepath = Path.Combine(Server.MapPath(notesattachementpath), notesattachementfilename);
                        file.SaveAs(notesattachementfilepath);

                        // create object of sellernotesattachement 
                        SellerNotesAttachments notesattachements = new SellerNotesAttachments
                        {
                            NoteID = abc.ID,
                            FileName = notesattachementfilename,
                            FilePath = notesattachementpath,

                            IsActive = true
                        };

                        // save seller notes attachement
                        Context.SellerNotesAttachments.Add(notesattachements);
                        Context.SaveChanges();
                    }
                }
                return RedirectToAction("SignUp", "SignUpMethod");
            }
            // if model state is not valid
            else
            {
                // create object of xyz
                AddNotesViewModels viewModel = new AddNotesViewModels
                {
                    NoteCategoryList = Context.NoteCategories.ToList(),
                    NoteTypeList = Context.NoteTypes.ToList(),
                    CountryList = Context.Countrie.ToList()
                };

                return View(viewModel);
            }
        }

        private void CreateDirectoryIfMissing(string folderpath)
        {
            // check if directory exists
            bool folderalreadyexists = Directory.Exists(Server.MapPath(folderpath));
            // if directory does not exists then create
            if (!folderalreadyexists)
                Directory.CreateDirectory(Server.MapPath(folderpath));
        }



        [Authorize]
        [HttpGet]
        [Route("AddNotes/EditNotes/{id}")]
        public ActionResult EditNotes(int id)
        {
            // get logged in user
            var user = Context.Registration.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            // get note
            SellerNotes note = Context.SellerNotes.Where(x => x.ID == id && x.IsActive == true && x.SellerID == user.UserId).FirstOrDefault();
            // get note attachement
            SellerNotesAttachments attachement = Context.SellerNotesAttachments.Where(x => x.NoteID == id).FirstOrDefault();
            if (note != null)
            {
                // create object of edit note viewmodel
                EditNotesViewModel xyz = new EditNotesViewModel
                {
                    ID = note.ID,
                    NoteID = note.ID,
                    Title = note.Title,
                    Category = note.Category,
                    Picture = note.DisplayPicture,
                    Note = attachement.FilePath,
                    NumberofPages = note.NumberofPages,
                    Description = note.Description,
                    NoteType = note.NoteType,
                    UniversityName = note.UniversityName,
                    Course = note.Course,
                    CourseCode = note.CourseCode,
                    Country = note.Country,
                    Professor = note.Professor,
                    IsPaid = note.IsPaid,
                    SellingPrice = note.SellingPrice,
                    Preview = note.NotesPreview,
                    NoteCategoryList = Context.NoteCategories.ToList(),
                    NoteTypeList = Context.NoteTypes.ToList(),
                    CountryList = Context.Countrie.ToList()
                };

                // return viewmodel to edit notes page
                return View(xyz);
            }
            else
            {
                // if note not found
                return HttpNotFound();
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AddNotes/EditNotes/{id}")]
        public ActionResult EditNotes(int id, EditNotesViewModel notes)
        {
            // check if model state is valid or not
            if (ModelState.IsValid)
            {
                // get logged in user
                var user = Context.Registration.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                // get note 
                var sellernotes = Context.SellerNotes.Where(x => x.ID == id && x.IsActive == true && x.SellerID == user.UserId).FirstOrDefault();
                // if sellernote null
                if (sellernotes == null)
                {
                    return HttpNotFound();
                }
                // check if note is paid or preview is not null
                if (notes.IsPaid == true && notes.Preview == null && sellernotes.NotesPreview == null)
                {
                    //ModelState.AddModelError("NotesPreview", "This field is required if selling type is paid");
                    @ViewBag.Notespreview = "This field is required if selling type is paid";
                    return View(notes);
                }
                // get note attachement 
                var notesattachement = Context.SellerNotesAttachments.Where(x => x.NoteID == notes.NoteID && x.IsActive == true).ToList();

                // attache note object and update
                Context.SellerNotes.Attach(sellernotes);
                sellernotes.Title = notes.Title.Trim();
                sellernotes.Category = notes.Category;
                sellernotes.NoteType = notes.NoteType;
                sellernotes.NumberofPages = notes.NumberofPages;
                sellernotes.Description = notes.Description.Trim();
                sellernotes.Country = notes.Country;
                sellernotes.UniversityName = notes.UniversityName.Trim();
                sellernotes.Course = notes.Course.Trim();
                sellernotes.CourseCode = notes.CourseCode.Trim();
                sellernotes.Professor = notes.Professor.Trim();
                if (notes.IsPaid == true)
                {
                    sellernotes.IsPaid = true;
                    sellernotes.SellingPrice = notes.SellingPrice;
                }
                else
                {
                    sellernotes.IsPaid = false;
                    sellernotes.SellingPrice = 0;
                }
               
                Context.SaveChanges();

                // if display picture is not null
                if (notes.DisplayPicture != null)
                {
                    // if note object has already previously uploaded picture then delete it
                    if (sellernotes.DisplayPicture != null)
                    {
                        string path = Server.MapPath(sellernotes.DisplayPicture);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    // save updated profile picture in directory and save directory path in database
                    string displaypicturefilename = System.IO.Path.GetFileName(notes.DisplayPicture.FileName);
                    string displaypicturepath = "~/Members/" + user.UserId + "/" + sellernotes.ID + "/";
                    CreateDirectoryIfMissing(displaypicturepath);
                    string displaypicturefilepath = Path.Combine(Server.MapPath(displaypicturepath), displaypicturefilename);
                    sellernotes.DisplayPicture = displaypicturepath + displaypicturefilename;
                    notes.DisplayPicture.SaveAs(displaypicturefilepath);
                }

                // if note preview is not null
                if (notes.NotesPreview != null)
                {
                    // if note object has already previously uploaded note preview then delete it
                    if (sellernotes.NotesPreview != null)
                    {
                        string path = Server.MapPath(sellernotes.NotesPreview);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    // save updated note preview in directory and save directory path in database
                    string notespreviewfilename = System.IO.Path.GetFileName(notes.NotesPreview.FileName);
                    string notespreviewpath = "~/Members/" + user.UserId + "/" + sellernotes.ID + "/";
                    CreateDirectoryIfMissing(notespreviewpath);
                    string notespreviewfilepath = Path.Combine(Server.MapPath(notespreviewpath), notespreviewfilename);
                    sellernotes.NotesPreview = notespreviewpath + notespreviewfilename;
                    notes.NotesPreview.SaveAs(notespreviewfilepath);
                }

                // check if user upload notes or not
                if (notes.UploadNotes[0] != null)
                {
                    // if user upload notes then delete directory that have previously uploaded notes
                    string path = Server.MapPath(notesattachement[0].FilePath);
                    DirectoryInfo dir = new DirectoryInfo(path);
                    EmptyFolder(dir);

                    // remove previously uploaded attachement from database
                    foreach (var item in notesattachement)
                    {
                        SellerNotesAttachments attachement = Context.SellerNotesAttachments.Where(x => x.ID == item.ID).FirstOrDefault();
                        Context.SellerNotesAttachments.Remove(attachement);
                    }

                    // add newly uploaded attachement in database and save it in database
                    foreach (HttpPostedFileBase file in notes.UploadNotes)
                    {
                        // check if file is null or not
                        if (file != null)
                        {
                            // save file in directory
                            string notesattachementfilename = System.IO.Path.GetFileName(file.FileName);
                            string notesattachementpath = "~/Members/" + user.UserId + "/" + sellernotes.ID + "/Attachements/";
                            CreateDirectoryIfMissing(notesattachementpath);
                            string notesattachementfilepath = Path.Combine(Server.MapPath(notesattachementpath), notesattachementfilename);
                            file.SaveAs(notesattachementfilepath);

                            // create object of sellernotesattachement 
                           SellerNotesAttachments notesattachements = new SellerNotesAttachments
                            {
                                NoteID = sellernotes.ID,
                                FileName = notesattachementfilename,
                                FilePath = notesattachementpath,
                               
                                IsActive = true
                            };

                            // save seller notes attachement
                           Context.SellerNotesAttachments.Add(notesattachements);
                            Context.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Dashboard", "Dashboard");
            }
            else
            {
                return RedirectToAction("EditNotes", new { id = notes.ID });
            }

        }


       

        private void EmptyFolder(DirectoryInfo directory)
        {
            // check if directory have files
            if (directory.GetFiles() != null)
            {
                // delete all files
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
            }

            // check if directory have subdirectory
            if (directory.GetDirectories() != null)
            {
                // call emptyfolder and delete subdirectory
                foreach (DirectoryInfo subdirectory in directory.GetDirectories())
                {
                    EmptyFolder(subdirectory);
                    subdirectory.Delete();
                }
            }  

        }

        


    }


}