using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models.EF;
using WebsiteNoiThat.Common;

namespace WebsiteNoiThat.Areas.Admin.Controllers
{
    [HasCredential(RoleId = "ROLE_ADMIN")]
    public class RolesController : Controller
    {
        private DBThoiTrang db = new DBThoiTrang();

        // GET: Admin/Roles
        [HasCredential(RoleId = "VIEW_ROLE")]
        public ActionResult Index()
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            return View(db.Roles.ToList());
        }       

        // GET: Admin/Roles/Create
        [HasCredential(RoleId = "ADD_ROLE")]
        public ActionResult Create()
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            return View();
        }

        // POST: Admin/Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasCredential(RoleId = "ADD_ROLE")]
        public ActionResult Create([Bind(Include = "RoleId,Name")] Role role)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(role);
        }


        // GET: Admin/Roles/Edit/5
        [HasCredential(RoleId = "EDIT_ROLE")]
        public ActionResult Edit(string id)
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasCredential(RoleId = "EDIT_ROLE")]
        public ActionResult Edit(Role role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingRole = db.Roles.Find(role.RoleId);
                    if (existingRole == null)
                    {
                        return HttpNotFound();
                    }

                    // Chỉ cập nhật Name
                    existingRole.Name = role.Name;
                    // Không cập nhật RoleId

                    db.SaveChanges();
                    TempData["Success"] = "Cập nhật quyền thành công!";
                    return RedirectToAction("Index");
                }
                return View(role);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                return View(role);
            }
        }


        [HttpPost]
        [HasCredential(RoleId = "DELETE_ROLE")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteRole(string id)
        {
            try
            {
                var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
                if (session.GroupId != "ADMIN")
                {
                    return Json(new { success = false, message = "Bạn không có quyền xóa chức năng này!" });
                }

                var hasCredentials = db.Credentials.Any(c => c.RoleId == id);
                if (hasCredentials)
                {
                    return Json(new { 
                        success = false, 
                        message = "Không thể xóa chức năng này vì đang được sử dụng trong phân quyền! Vui lòng xóa phân quyền trước." 
                    });
                }

                var role = db.Roles.Find(id);
                if (role == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy chức năng này!" });
                }

                db.Roles.Remove(role);
                db.SaveChanges();
                return Json(new { success = true, message = "Xóa chức năng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
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
