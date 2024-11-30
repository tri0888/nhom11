using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteNoiThat.Common;
using WebsiteNoiThat.Models;
using Models.DAO;

namespace WebsiteNoiThat.Areas.Admin.Controllers
{
    public class UserController : HomeController
    {
        private readonly DBThoiTrang db = new DBThoiTrang();

        [HasCredential(RoleId = "VIEW_USER")]
        [HasCredential(RoleId = "ADMIN")]
        public ActionResult Show()
        {
            try 
            {
                var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
                if (session == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                // Kiểm tra xem người dùng có phải là ADMIN không
                var userGroup = db.UserGroups.Find(session.GroupId);
                if (userGroup == null || userGroup.Name != "ADMIN")
                {
                    // Nếu không phải ADMIN, chuyển đến trang 401
                    return View("~/Areas/Admin/Views/Shared/401.cshtml");
                }

                var model = db.Users.ToList();
                ViewBag.username = session.Username;
                return View(model);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [HasCredential(RoleId = "ADD_ADMIN")]
        public ActionResult Add()
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            ViewBag.ListGroups = new SelectList(db.UserGroups.Where(a => a.GroupId != "USER").ToList(), "GroupId", "Name");
            return View();
        }

        [HttpPost]
        [HasCredential(RoleId = "ADD_ADMIN")]
        public ActionResult Add(User n)
        {
            ViewBag.ListGroups = new SelectList(db.UserGroups.Where(a => a.GroupId != "USER").ToList(), "GroupId", "Name");
            var model = new User();
            model.Name = n.Name;
            model.Address = n.Address;
            model.Phone = n.Phone;
            model.Username = n.Username;
            model.Password = Encryptor.MD5Hash(n.Password);
            model.Email = n.Email;
            model.GroupId = n.GroupId;
            model.Status = n.Status;
            db.Users.Add(model);
            db.SaveChanges();
            return RedirectToAction("Show");
        }

        [HttpGet]
        [HasCredential(RoleId = "EDIT_ADMIN")]
        public ActionResult Edit(int userId)
        {
            var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            ViewBag.ListGroups = new SelectList(db.UserGroups.Where(a => a.GroupId != "USER").ToList(), "GroupId", "Name");
            var model = db.Users.Find(userId);
            if (model == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("Show");
            }
            
            // Xóa mật khẩu để không hiển thị
            model.Password = string.Empty;
            return View(model);
        }

        [HttpPost]
        [HasCredential(RoleId = "EDIT_ADMIN")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
                    var user = db.Users.Find(model.UserId);
                    if (user == null)
                    {
                        return HttpNotFound();
                    }

                    // Kiểm tra không cho phép tự khóa tài khoản của mình
                    if (user.UserId == session.UserId && !model.Status)
                    {
                        ModelState.AddModelError("", "Không thể khóa tài khoản của chính mình");
                        ViewBag.ListGroups = new SelectList(db.UserGroups.Where(a => a.GroupId != "USER").ToList(), "GroupId", "Name");
                        return View(model);
                    }

                    // Cập nhật các thông tin khác
                    user.Name = model.Name;
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.Username = model.Username;
                    user.GroupId = model.GroupId;
                    user.Status = model.Status;

                    // Chỉ cập nhật mật khẩu nếu có nhập mới
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        user.Password = Encryptor.MD5Hash(model.Password);
                    }

                    db.SaveChanges();
                    return RedirectToAction("Show");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                }
            }

            ViewBag.ListGroups = new SelectList(db.UserGroups.Where(a => a.GroupId != "USER").ToList(), "GroupId", "Name");
            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult UserInfo()
        {
            var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
            var model = db.Users.Find(session.UserId);
            return PartialView(model);
        }

        public ActionResult UserProfile()
        {
            try
            {
                var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
                if (session == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                var model = db.Users.FirstOrDefault(n => n.UserId == session.UserId);
                if (model == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin người dùng";
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.username = session.Username;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult UserProfile([Bind(Include = "UserId,Name,Phone,Address,Email,Username,GroupId,Status")] User user, string NewPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingUser = db.Users.Find(user.UserId);
                    if (existingUser == null)
                    {
                        TempData["Error"] = "Không tìm thấy thông tin người dùng";
                        return RedirectToAction("Index", "Home");
                    }

                    // Cập nhật thông tin cơ bản
                    existingUser.Name = user.Name;
                    existingUser.Phone = user.Phone;
                    existingUser.Address = user.Address;
                    existingUser.Email = user.Email;

                    // Cập nhật mật khẩu nếu có
                    if (!string.IsNullOrEmpty(NewPassword))
                    {
                        existingUser.Password = Encryptor.MD5Hash(NewPassword);
                    }

                    db.SaveChanges();
                    TempData["Success"] = "Cập nhật thông tin thành công";
                    return RedirectToAction("UserProfile");
                }

                return View(user);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                return View(user);
            }
        }

        [HttpPost]
        [HasCredential(RoleId = "DELETE_ADMIN")]
        public ActionResult Delete(int userId)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
                    var user = db.Users.Find(userId);
                    
                    if (user == null)
                    {
                        return Json(new { success = false, message = "Không tìm thấy nhân viên" });
                    }

                    // Kiểm tra quyền và điều kiện xóa
                    if (session.GroupId != "ADMIN" && user.GroupId == "ADMIN")
                    {
                        return Json(new { success = false, message = "Không đủ quyền để xóa ADMIN" });
                    }

                    if (user.UserId == session.UserId)
                    {
                        return Json(new { success = false, message = "Không thể xóa tài khoản đang đăng nhập" });
                    }

                    // Thử xóa từng bảng và ghi log
                    try
                    {
                        // 1. Xóa Card
                        var cards = db.Cards.Where(c => c.UserId == userId).ToList();
                        foreach (var card in cards)
                        {
                            db.Cards.Remove(card);
                        }
                        db.SaveChanges();
                        System.Diagnostics.Debug.WriteLine("Đã xóa Cards");

                        // 2. Xóa OrderDetails và Orders
                        var orders = db.Orders.Where(o => o.UserId == userId).ToList();
                        foreach (var order in orders)
                        {
                            if (order.StatusId != 4)
                            {
                                throw new Exception($"Đơn hàng #{order.OrderId} đang xử lý");
                            }

                            var details = db.OrderDetails.Where(od => od.OrderId == order.OrderId).ToList();
                            db.OrderDetails.RemoveRange(details);
                            db.SaveChanges();
                            System.Diagnostics.Debug.WriteLine($"Đã xóa OrderDetails của đơn hàng {order.OrderId}");

                            db.Orders.Remove(order);
                            db.SaveChanges();
                            System.Diagnostics.Debug.WriteLine($"Đã xóa Order {order.OrderId}");
                        }


                        // 4. Xóa User
                        db.Users.Remove(user);
                        db.SaveChanges();
                        System.Diagnostics.Debug.WriteLine("Đã xóa User");

                        transaction.Commit();
                        return Json(new { success = true, message = "Xóa nhân viên thành công" });
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Chi tiết lỗi: " + ex.ToString());
                        if (ex.InnerException != null)
                        {
                            System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.ToString());
                        }
                        throw; // Ném lại exception để outer catch block xử lý
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    var errorMessage = ex.Message;
                    if (ex.InnerException != null)
                    {
                        errorMessage = ex.InnerException.Message;
                    }
                    return Json(new { 
                        success = false, 
                        message = "Không thể xóa nhân viên: " + errorMessage
                    });
                }
            }
        }

        [HttpPost]
        [HasCredential(RoleId = "UPDATE_ADMIN")]
        [ValidateAntiForgeryToken]
        public JsonResult ToggleStatus(int userId, bool status)
        {
            try
            {
                var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
                var user = db.Users.Find(userId);
                
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy nhân viên" });
                }

                // Kiểm tra quyền: chỉ ADMIN mới được khóa MOD
                if (user.GroupId == "MOD" && session.GroupId != "ADMIN")
                {
                    return Json(new { success = false, message = "Bạn không có quyền khóa tài khoản MOD" });
                }

                // Không cho phép tự khóa chính mình
                if (user.UserId == session.UserId)
                {
                    return Json(new { success = false, message = "Không thể khóa tài khoản của chính mình" });
                }

                user.Status = status;
                db.SaveChanges();
                return Json(new { 
                    success = true, 
                    message = status ? "Mở khóa thành công" : "Khóa thành công",
                    newStatus = status
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}