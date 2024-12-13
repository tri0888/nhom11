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
            
            // Lấy danh sách loại cha để dropdown
            ViewBag.ParentCategories = new SelectList(
                db.Categories.Where(c => c.ParId == 0),
                "CategoryId",
                "Name"
            );
            
            return View();
        }

        [HttpPost]
        [HasCredential(RoleId = "ADD_CATE")]
        public ActionResult Add(Category n)
        {
            if (!ModelState.IsValid)
            {
                // Load lại dropdown nếu cần
                ViewBag.ParentCategories = new SelectList(
                    db.Categories.Where(c => c.ParId == 0),
                    "CategoryId",
                    "Name"
                );
                return View(n);
            }

            try 
            {
                // Đảm bảo ParId = 0 nếu không được chọn
                if (n.ParId == null)
                {
                    n.ParId = 0;
                }

                db.Categories.Add(n);
                db.SaveChanges();
                return RedirectToAction("Show");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm danh mục: " + ex.Message);
                
                // Load lại dropdown
                ViewBag.ParentCategories = new SelectList(
                    db.Categories.Where(c => c.ParId == 0),
                    "CategoryId",
                    "Name"
                );
                return View(n);
            }
        }

        [HttpGet]
        [HasCredential(RoleId = "EDIT_CATE")]
        public ActionResult Edit(int CategoryId)
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            Category category = db.Categories.Find(CategoryId);
            if (category == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách loại cha để dropdown (trừ category hiện tại và các con của nó)
            var parentCategories = db.Categories
                .Where(c => c.ParId == 0 && c.CategoryId != CategoryId)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name,
                    Selected = c.CategoryId == category.ParId
                })
                .ToList();

            ViewBag.ParentCategories = parentCategories;
            return View(category);
        }

        [HttpPost]
        [HasCredential(RoleId = "EDIT_CATE")]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingCategory = db.Categories.Find(model.CategoryId);
                    if (existingCategory == null)
                    {
                        return HttpNotFound();
                    }

                    // Nếu đang là loại cha, không cho phép chuyển thành loại con
                    if (existingCategory.ParId == 0)
                    {
                        model.ParId = 0;
                    }

                    // Cập nhật các thuộc tính
                    existingCategory.Name = model.Name;
                    existingCategory.ParId = model.ParId;
                    existingCategory.MetaTitle = model.MetaTitle;

                    db.SaveChanges();
                    return RedirectToAction("Show");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                }
            }
            
            // Nếu có lỗi, load lại dropdown
            var parentCategories = db.Categories
                .Where(c => c.ParId == 0 && c.CategoryId != model.CategoryId)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name,
                    Selected = c.CategoryId == model.ParId
                })
                .ToList();

            ViewBag.ParentCategories = parentCategories;
            return View(model);
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

                // Kiểm tra xem có phải loại cha không
                if (category.ParId == 0)
                {
                    // Kiểm tra xem có loại con nào không
                    var childCategories = db.Categories.Any(c => c.ParId == category.CategoryId);
                    if (childCategories)
                    {
                        return Json(new { 
                            success = false, 
                            message = "Không thể xóa loại cha khi còn loại con. Vui lòng xóa các loại con trước." 
                        });
                    }
                }

                // Kiểm tra xem có sản phẩm nào thuộc danh mục này không
                var productCount = db.Products.Count(p => p.CateId == id);
                if (productCount > 0)
                {
                    return Json(new { 
                        success = false, 
                        message = $"Không thể xóa danh mục này vì đang có {productCount} sản phẩm. Vui lòng chuyển hoặc xóa các sản phẩm trước." 
                    });
                }

                // Nếu không có sản phẩm và không có loại con thì tiến hành xóa
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
                    message = "Có lỗi xảy ra: " + ex.Message
                });
            }
        }

    }
}