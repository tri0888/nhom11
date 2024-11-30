using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.DAO;
using Models.EF;
using System.Web.Script.Serialization;
using PagedList;
using PagedList.Mvc;
using System.IO;
using WebsiteNoiThat.Common;
using System.Data.Entity;
using System.Net;

namespace WebsiteNoiThat.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        private DBThoiTrang db = new DBThoiTrang();
        
        [HasCredential(RoleId = "VIEW_NEWS")]
        public ActionResult Show()
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion_admin];
            if (session == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            ViewBag.username = session.Username;
            var model = db.News.ToList();
            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleId = "ADD_NEWS")]
        public ActionResult Add()
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            return View();
        }

        [HttpPost]
        [HasCredential(RoleId = "ADD_NEWS")]
        public ActionResult Add(News n, HttpPostedFileBase file)
        {
            var model = db.News.SingleOrDefault(a => a.NewsId == n.NewsId);

            if (model != null)
            {
                ModelState.AddModelError("NewError", "Id already in user");

                return View();
            }
            else
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/image"), fileName);
                file.SaveAs(path);
                n.Photo = file.FileName;
                db.News.Add(n);
                db.SaveChanges();
                return RedirectToAction("Show");
            }
        }

        [HttpGet]
        [HasCredential(RoleId = "EDIT_NEWS")]
        public ActionResult Edit(int NewsId)
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            News a = db.News.SingleOrDefault(n => n.NewsId == NewsId);
            if (a == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("Show");
            }
            return View(a);

        }

        [HttpPost]
        public ActionResult Edit(News news, HttpPostedFileBase UploadImage)
        {
            if (ModelState.IsValid)
            {
                if (UploadImage != null && UploadImage.ContentLength > 0)
                {
                    // Xóa file ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(news.Photo))
                    {
                        var oldImagePath = Path.Combine(Server.MapPath("~/image"), news.Photo);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Lưu file ảnh mới
                    var fileName = Path.GetFileName(UploadImage.FileName);
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);
                    UploadImage.SaveAs(path);
                    news.Photo = fileName;
                }

                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Show");
            }
            return View(news);
        }

        [HttpPost]
        [HasCredential(RoleId = "DELETE_NEWS")]
        public ActionResult Delete(int id)
        {
            try
            {
                var news = db.News.Find(id);
                if (news != null)
                {

                    db.News.Remove(news);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

    }
}