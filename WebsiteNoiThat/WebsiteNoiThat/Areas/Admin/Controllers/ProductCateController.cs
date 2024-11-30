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

namespace WebsiteNoiThat.Areas.Admin.Controllers
{
    public class ProductCateController : HomeController
    {
        DBThoiTrang db = new DBThoiTrang();

        public ActionResult Index()
        {
            return View();
        }

        [HasCredential(RoleId = "VIEW_CATE")]
        public ActionResult Show()
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            return View(db.Categories.ToList());
        }

        [HttpGet]
        [HasCredential(RoleId = "ADD_CATE")]
        public ActionResult Add()
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;
            return View();
        }

        [HttpPost]
        [HasCredential(RoleId = "ADD_CATE")]
        public ActionResult Add(Category n)
        {
            var model = db.Categories.SingleOrDefault(a => a.CategoryId == n.CategoryId);
            if (model != null)
            {
                ModelState.AddModelError("CateError", "CategoryId already in use");
                return View();
            }
            else
            {
                db.Categories.Add(n);
                db.SaveChanges();
                return RedirectToAction("Show");
            }

        }

        [HttpGet]
        [HasCredential(RoleId = "EDIT_CATE")]
        public ActionResult Edit(int CategoryId)
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            Category a = db.Categories.SingleOrDefault(n => n.CategoryId == CategoryId);
            return View(a);

        }

        [HttpPost]
        [HasCredential(RoleId = "EDIT_CATE")]
        public ActionResult Edit(Category n)
        {
            if (ModelState.IsValid)
            {
                db.Entry(n).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Show");
            }
            else
            {
                return JavaScript("alert('Error');");
            }
        }


        [HttpPost]
        [HasCredential(RoleId = "DELETE_CATE")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var category = db.Categories.Find(id);
                if (category == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy danh mục" });
                }

                // Kiểm tra xem có sản phẩm nào thuộc danh mục này không
                var productCount = db.Products.Count(p => p.CateId == id);
                if (productCount > 0)
                {
                    return Json(new { 
                        success = false, 
                        message = $"Không thể xóa danh mục này vì đang có {productCount} sản phẩm. Vui lòng chuyển hoặc xóa các sản phẩm trước khi xóa danh mục." 
                    });
                }

                // Nếu không có sản phẩm nào thì tiến hành xóa
                db.Categories.Remove(category);
                db.SaveChanges();

                return Json(new { 
                    success = true, 
                    message = "Xóa danh mục thành công" 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = "Có lỗi xảy ra: " + ex.Message,
                    details = ex.InnerException?.Message 
                });
            }
        }

    }
}