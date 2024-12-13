using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using Models.EF;
using WebsiteNoiThat.Areas.Admin.Models;
using WebsiteNoiThat.Common;

namespace WebsiteNoiThat.Areas.Admin.Controllers
{
    [HasCredential(RoleId = "VIEW_CREDENTIAL")]
    public class CredentialsController : Controller
    {
        private readonly DBThoiTrang db = new DBThoiTrang();

        [HasCredential(RoleId = "VIEW_CREDENTIAL")]
        public ActionResult Index()
        {
            var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            var credentials = db.Credentials.ToList();
            return View(credentials);
        }

        [HasCredential(RoleId = "ADD_CREDENTIAL")]
        public ActionResult Create()
        {
            var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            if (session.GroupId != "ADMIN")
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            var userGroups = db.UserGroups.ToList();
            ViewBag.ListGroups = new SelectList(userGroups, "GroupId", "Name");
            ViewBag.ListRoles = new SelectList(db.Roles.ToList(), "RoleId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasCredential(RoleId = "ADD_CREDENTIAL")]
        public ActionResult Create([Bind(Include = "UserGroupId,RoleId")] Credential credential)
        {
            try
            {
                var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
                if (session.GroupId != "ADMIN")
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (ModelState.IsValid)
                {
                    // Kiểm tra credential đã tồn tại chưa
                    var exists = db.Credentials.Any(c => 
                        c.UserGroupId == credential.UserGroupId && 
                        c.RoleId == credential.RoleId);

                    if (exists)
                    {
                        ModelState.AddModelError("", "Quyền này đã được cấp cho nhóm người dùng");
                        ViewBag.ListGroups = new SelectList(db.UserGroups.ToList(), "GroupId", "Name");
                        ViewBag.ListRoles = new SelectList(db.Roles.ToList(), "RoleId", "Name");
                        return View(credential);
                    }

                    db.Credentials.Add(credential);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
            }

            ViewBag.ListGroups = new SelectList(db.UserGroups.ToList(), "GroupId", "Name");
            ViewBag.ListRoles = new SelectList(db.Roles.ToList(), "RoleId", "Name");
            return View(credential);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasCredential(RoleId = "DELETE_CREDENTIAL")]
        public JsonResult DeleteCredential(int id)
        {
            try
            {
                var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
                if (session.GroupId != "ADMIN")
                {
                    return Json(new { success = false, message = "Bạn không có quyền xóa quyền này!" });
                }

                var credential = db.Credentials.Find(id);
                if (credential == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy quyền này!" });
                }

                // Không cho phép xóa credential của ADMIN
                if (credential.UserGroupId == "ADMIN")
                {
                    return Json(new { success = false, message = "Không thể xóa quyền của ADMIN!" });
                }

                db.Credentials.Remove(credential);
                db.SaveChanges();
                return Json(new { success = true, message = "Xóa quyền thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HasCredential(RoleId = "EDIT_CREDENTIAL")]
        public ActionResult Edit(int id)
        {
            var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
            if (session.GroupId != "ADMIN")
            {
                return RedirectToAction("Index", "Home");
            }

            var credential = db.Credentials.Find(id);
            if (credential == null)
            {
                return HttpNotFound();
            }

            // Chuẩn bị dữ liệu cho dropdowns
            ViewBag.ListGroups = new SelectList(db.UserGroups.ToList(), "GroupId", "Name", credential.UserGroupId);
            ViewBag.ListRoles = new SelectList(db.Roles.ToList(), "RoleId", "Name", credential.RoleId);

            return View(credential);
        }

        [HttpPost]
        [HasCredential(RoleId = "EDIT_CREDENTIAL")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Credential credential)
        {
            var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
            if (session.GroupId != "ADMIN")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    // Kiểm tra xem credential đang sửa có phải của ADMIN không
                    var existingCredential = db.Credentials.Find(credential.CredenId);
                    if (existingCredential == null)
                    {
                        ModelState.AddModelError("", "Không tìm thấy quyền cần sửa!");
                        return View(credential);
                    }

                    if (existingCredential.UserGroupId == "ADMIN")
                    {
                        ModelState.AddModelError("", "Không thể sửa quyền của ADMIN!");
                        PrepareDropDownLists(credential);
                        return View(credential);
                    }

                    // Kiểm tra xem có đang cố gắng sửa thành quyền của ADMIN không
                    if (credential.UserGroupId == "ADMIN")
                    {
                        ModelState.AddModelError("", "Không thể chuyển thành quyền của ADMIN!");
                        PrepareDropDownLists(credential);
                        return View(credential);
                    }

                    // Kiểm tra xem quyền này đã tồn tại chưa
                    var isDuplicate = db.Credentials.Any(c => 
                        c.UserGroupId == credential.UserGroupId && 
                        c.RoleId == credential.RoleId && 
                        c.CredenId != credential.CredenId);

                    if (isDuplicate)
                    {
                        ModelState.AddModelError("", "Quyền này đã được cấp cho nhóm người dùng!");
                        PrepareDropDownLists(credential);
                        return View(credential);
                    }

                    existingCredential.UserGroupId = credential.UserGroupId;
                    existingCredential.RoleId = credential.RoleId;
                    db.SaveChanges();

                    TempData["Success"] = "Cập nhật quyền thành công!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
            }

            PrepareDropDownLists(credential);
            return View(credential);
        }

        private void PrepareDropDownLists(Credential credential)
        {
            ViewBag.ListGroups = new SelectList(db.UserGroups.ToList(), "GroupId", "Name", credential.UserGroupId);
            ViewBag.ListRoles = new SelectList(db.Roles.ToList(), "RoleId", "Name", credential.RoleId);
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
