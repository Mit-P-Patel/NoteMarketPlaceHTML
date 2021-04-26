using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MitProjectAssignment.Models;
using System.Net;
using System.Net.Mail;
using System.Web.Security;
using System.IO;
using System.Data.Entity.Validation;

namespace MitProjectAssignment.Controllers
{
    public class RegisterController : Controller
    {

        MitDatabaseEntities1 objCon = new MitDatabaseEntities1();
        // GET: Register
        [HttpGet]
        [Route("SignUp")]
        public ActionResult SignUpMethod()
        {
            return View();
        }

        [HttpPost]
        [Route("SignUp")]
        public ActionResult SignUpMethod(Registration objUsr)
        {
           
            objUsr.EmailVerification = false;
            var IsExists = IsEmailExists(objUsr.Email);
            if(IsExists)
            {
                ModelState.AddModelError("EmailExists", "Email Already Exists");
                return View();
            }

            objUsr.ActivetionCode = Guid.NewGuid();
              
            objUsr.Password = MitProjectAssignment.Models.encryptPassword.textToEncrypt(objUsr.Password);
            objCon.Registration.Add(objUsr);
            objCon.SaveChanges();
            SendEmailToUser(objUsr.Email, objUsr.ActivetionCode.ToString());
            var Message = "Registration Completed. Please Check Your Email :" + objUsr.Email;
            ViewBag.Message = Message;
            return View("Email_Verification");
        }

        public bool IsEmailExists(string eMail)
        {
            var IsCheck = objCon.Registration.Where(email => email.Email == eMail).FirstOrDefault();
            return IsCheck != null;
        }

        public void SendEmailToUser(string emailId, string activationCode)
        {
            var GenarateUserVerificationLink = "/Register/UserVerification/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, GenarateUserVerificationLink);

            var fromMail = new MailAddress("mitp73661@gmail.com", "Mit");    
            var fromEmailpassword = "Mit@6465";     
            var toEmail = new MailAddress(emailId);

            var smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);

            var Message = new MailMessage(fromMail, toEmail);
            Message.Subject = "Registration Completed-Demo";
            Message.Body = "<br/> Your registration completed succesfully." +
                           "<br/> please click on the below link for account verification" +
                           "<br/><br/><a href=" + link + ">" + link + "</a>";
            Message.IsBodyHtml = true;
            smtp.Send(Message);
        }

        public ActionResult UserVerification(string id)
        {
            bool Status = false;

            objCon.Configuration.ValidateOnSaveEnabled = false; // Ignor to password confirmation     
            var IsVerify = objCon.Registration.Where(u => u.ActivetionCode == new Guid(id)).FirstOrDefault();

            if (IsVerify != null)
            {
                IsVerify.EmailVerification = true;
                objCon.SaveChanges();
                ViewBag.Message = "Email Verification completed";
                Status = true;
            }
            else
            {
                ViewBag.Message = "Invalid Request...Email not verify";
                ViewBag.Status = false;
            }

            return View();
        }

        [HttpGet]
        [Route("Register/Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Register/Login")]
        public ActionResult Login(UserLogin LgnUsr)
        {
            var _passWord = MitProjectAssignment.Models.encryptPassword.textToEncrypt(LgnUsr.Password);
            bool Isvalid = objCon.Registration.Any(x => x.Email == LgnUsr.Email && 
            x.Password == _passWord);
            if (Isvalid)
            {
                int timeout = LgnUsr.Rememberme ? 60 : 5; // Timeout in minutes, 60 = 1 hour.    
                var ticket = new FormsAuthenticationTicket(LgnUsr.Email, false, timeout);
                string encrypted = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                cookie.Expires = System.DateTime.Now.AddMinutes(timeout);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
                return RedirectToAction("Home", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid Information... Please try again!");
            }
            return View();
        }

        [Authorize]
        [Route("Logout")]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Register");
        }


        [HttpGet]
        [Route("ForgotPassword")]
        
        public ActionResult ForgotPassWord()
        {
            ForgetPassword obj = new ForgetPassword();
            return View();
        }

        [HttpPost]
        [Route("ForgotPassword")]
       

        public ActionResult ForgotPassWord(ForgetPassword xyz)
        {
            if (objCon.Registration.Any(model => model.Email == xyz.Email))
            {
                ForgotPassSentEmail(xyz);
                return View();

            }
            else
            {
                ModelState.AddModelError("Error", "Email Id does not exists");
                return View();
            }
           
            
        }

        private void ForgotPassSentEmail(ForgetPassword xyz)
        {
            var check = objCon.Registration.Where(x => x.Email == xyz.Email).FirstOrDefault();
            using (MailMessage mm = new MailMessage("mitp73661@gmail.com", xyz.Email))
            {
                mm.Subject = "NoteMarketPlace - Temporary Password";

                var body = "<p>Hello,</p> <p>Your newly generated password is:<p> <p>{0}</p> <p>Thanks,</p><p>Team Notes MarketPlace</p>";
                string NewPassword = GeneratePassword().ToString();
              
                body = string.Format(body, NewPassword);
                mm.Body = body;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                NetworkCredential Network = new NetworkCredential("mitp73661@gmail.com", "Mit@6465");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = Network;
                smtp.Port = 587;
                smtp.Send(mm);

                if (NewPassword != null)
                {
                    var Replace = objCon.Registration.Where(x => x.Email == xyz.Email).FirstOrDefault();
                    if (Replace != null)
                    {
                        Replace.Password = NewPassword;
                        Replace.Password = MitProjectAssignment.Models.encryptPassword.textToEncrypt(Replace.Password);

                        objCon.SaveChanges();
                      

                    }
                    

                }
               
            }
        }

        public string GeneratePassword()
        {
            string PassLength = "6";
            string NewPass = "";

            String AllowChar = "";

            AllowChar = "1,2,3,4,5,6,7,8,9,0";

            char[] Seperated = { ',' };

            string[] arr = AllowChar.Split(Seperated);

            string IDString = "";
            string Temp = "";

            Random Rand = new Random();

            for (int i = 0; i < Convert.ToInt32(PassLength); i++)
            {
                Temp = arr[Rand.Next(0, arr.Length)];
                IDString += Temp;
                NewPass = IDString;
            }
            return NewPass;
        }




    }
}