using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteNoiThat.Common;
using WebsiteNoiThat.Models;

namespace WebsiteNoiThat.Areas.Admin.Controllers
{
    public class CustomerController : HomeController
    {
        DBThoiTrang db = new DBThoiTrang();

        [HasCredential(RoleId = "VIEW_USER")]
        public ActionResult Show()
        {
            var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;
            
            var users = (from a in db.Users
                        join b in db.Cards on a.UserId equals b.UserId into g
                        from d in g.DefaultIfEmpty()
                        where a.GroupId == "USER"
                        select new UserModelView
                        {
                            UserId = a.UserId,
                            Name = a.Name,
                            Address = a.Address,
                            Phone = a.Phone,
                            Username = a.Username,
                            Email = a.Email,
                            Status = a.Status,
                            NumberCard = d.NumberCard ?? 0
                        }).ToList();

            return View(users);
        }

        [HttpPost]
        [HasCredential(RoleId = "UPDATE_USER")]
        [ValidateAntiForgeryToken]
        public JsonResult ToggleStatus(int userId, bool status)
        {
            try
            {
                var user = db.Users.Find(userId);
                if (user != null && user.GroupId == "USER")
                {
                    user.Status = status;
                    db.SaveChanges();
                    return Json(new { 
                        success = true, 
                        message = status ? "Mở khóa thành công" : "Khóa thành công",
                        newStatus = status
                    });
                }
                return Json(new { 
                    success = false, 
                    message = "Không tìm thấy người dùng" 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = "Có lỗi xảy ra: " + ex.Message 
                });
            }
        }
    }
}