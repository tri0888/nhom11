using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteNoiThat.Areas.Services;

namespace WebsiteNoiThat.Areas.Admin.Controllers
{
    public class ContactController : Controller
    {
        private readonly DBThoiTrang db = new DBThoiTrang();

        // GET: Admin/Contact
        public ActionResult Index()
        {
            var model = db.Contacts.ToList();
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var contact = db.Contacts.Find(id);
                if (contact != null)
                {
                    db.Contacts.Remove(contact);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Không tìm thấy liên hệ" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateStatus(int id)
        {
            try
            {
                var contact = db.Contacts.Find(id);
                if (contact != null)
                {
                    // Kiểm tra nếu đã xử lý rồi thì không cho xử lý nữa
                    if (contact.Status.GetValueOrDefault())
                    {
                        return Json(new { success = false, message = "Liên hệ này đã được xử lý" });
                    }

                    // Gửi email trước khi cập nhật trạng thái
                    bool emailSent = EmailService.SendAutoResponse(contact.EmailCC, contact.Content);
                    
                    // Cập nhật trạng thái
                    contact.Status = true;
                    db.SaveChanges();

                    return Json(new { 
                        success = true,
                        emailSent = emailSent,
                        message = emailSent ? "Đã xử lý và gửi email phản hồi" : "Đã xử lý nhưng không gửi được email"
                    });
                }
                return Json(new { success = false, message = "Không tìm thấy liên hệ" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}