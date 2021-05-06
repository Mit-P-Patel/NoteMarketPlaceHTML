using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MitProjectAssignment.Models;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Web.Security;

namespace MitProjectAssignment.Controllers
{
    
    public class HomeController : Controller
    {
        MitDatabaseEntities1 dbobj = new MitDatabaseEntities1();
        [Route("Home")]
        public ActionResult Home()
        {
            return View();
        }
        // GET: Home
        [HttpGet]
        [Route("ContactUs")]
        public ActionResult ContactUs()
        {
            var user = dbobj.Registration.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            ContactUsModel userprofilemodel = new ContactUsModel();

            userprofilemodel.EmailID = user.Email;
            userprofilemodel.FullName = user.FirstName + " " + user.LastName;
            

            return View(userprofilemodel);
        }

        [HttpPost]
        [Route("ContactUs")]
        public async Task<ActionResult> ContactUs(ContactUsModel xyz)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {1}</p><p>Hello,</p><p>{2}</p><br><p>Regards,</p><p>{0}</p>";
                var Message = new MailMessage();
                Message.To.Add(new MailAddress("mitp73661@gmail.com"));

                Message.From = new MailAddress(xyz.EmailID);
                Message.Subject = xyz.Subject;
                Message.Body = string.Format(body, xyz.FullName, xyz.EmailID, xyz.Comments);
                Message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "mitp73661@gmail.com",
                        Password = "Mit@6465"
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(Message);
                }

            }
            return View();

        }
    }
}