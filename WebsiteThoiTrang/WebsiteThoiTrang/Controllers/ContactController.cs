using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebsiteNoiThat.Models;

namespace WebsiteNoiThat.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        DBThoiTrang db= new DBThoiTrang();
        [HttpGet]
        public ActionResult Contacts()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Contacts(EmailModel obj)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.IsError = true;
                ViewBag.Status = "Vui lòng kiểm tra lại thông tin nhập vào.";
                return View(obj);
            }

            try
            {
                // Thông tin đăng nhập Gmail
                string gmailUsername = "trandoanminhtri70@gmail.com";
                string gmailAppPassword = "jamp dcyy aowb rrzy"; // Thay bằng App Password của bạn

                // Cấu hình SMTP client
                using (var smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(gmailUsername, gmailAppPassword);

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(gmailUsername, "Thời Trang Cao Cấp");
                        message.To.Add(new MailAddress(obj.ToEmail));
                        message.Subject = obj.EmailSubject;
                        message.Body = obj.EMailBody;
                        message.IsBodyHtml = true;

                        if (!string.IsNullOrEmpty(obj.EmailCC))
                        {
                            message.CC.Add(obj.EmailCC);
                        }

                        if (!string.IsNullOrEmpty(obj.EmailBCC))
                        {
                            message.Bcc.Add(obj.EmailBCC);
                        }

                        smtp.Send(message);
                    }
                }
                // Lưu vào database
                var model = new Contact
                {
                    Content = obj.EMailBody,
                    Status = false, 
                    EmailCC = obj.ToEmail
                };

                db.Contacts.Add(model);
                db.SaveChanges();
                ViewBag.IsError = false;
                ViewBag.Status = "Email đã được gửi thành công!";
                
                ModelState.Clear();
                return View(new EmailModel());
            }
            catch (Exception ex)
            {
                ViewBag.IsError = true;
                ViewBag.Status = "Có lỗi xảy ra khi gửi email: " + ex.Message;
                return View(obj);
            }
        }
    }
}