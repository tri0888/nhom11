using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteNoiThat.Common;
using WebsiteNoiThat.Models;
using System.Data.Entity;
using System.Net;

namespace WebsiteNoiThat.Areas.Admin.Controllers
{
    [HasCredential(RoleId = "VIEW_PRODUCT")]
    public class ProductController : HomeController
    {
        private readonly DBThoiTrang db = new DBThoiTrang();

        [HasCredential(RoleId = "VIEW_PRODUCT")]
        public ActionResult Show()
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            var productViewModels = (from a in db.Products
                         join b in db.Providers on a.ProviderId equals b.ProviderId
                         join c in db.Categories on a.CateId equals c.CategoryId
                         select new ProductViewModel
                         {
                             ProductId = a.ProductId,
                             Name = a.Name,
                             Description = a.Description,
                             Discount = a.Discount,
                             ProviderName = b.Name,
                             CateName = c.Name,
                             Price = a.Price,
                             Quantity = a.Quantity,
                             StartDate = a.StartDate,
                             EndDate = a.EndDate,
                             Photo = a.Photo,
                             IsHidden = a.IsHidden
                         }).ToList();

            return View(productViewModels);
        }

        [HttpGet]
        [HasCredential(RoleId = "ADD_PRODUCT")]
        public ActionResult Add()
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            // Lấy danh sách loại cha
            var parentCategories = db.Categories
                .Where(c => c.ParId == 0)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                })
                .ToList();
            ViewBag.ParentCategories = parentCategories;

            // Lấy danh sách loại con (mặc định rỗng)
            ViewBag.ChildCategories = new List<SelectListItem>();

            ViewBag.ListProvider = new SelectList(db.Providers.ToList(), "ProviderId", "Name");
            return View();
        }

        [HttpGet]
        public JsonResult GetChildCategories(int parentId)
        {
            var childCategories = db.Categories
                .Where(c => c.ParId == parentId)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                })
                .ToList();

            return Json(childCategories, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [HasCredential(RoleId = "ADD_PRODUCT")]
        public ActionResult Add(ProductViewModel n, HttpPostedFileBase UploadImage)
        {
            // Khởi tạo lại danh sách categories
            var parentCategories = db.Categories
                .Where(c => c.ParId == 0)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                })
                .ToList();
            ViewBag.ParentCategories = parentCategories;

            // Khởi tạo lại danh sách child categories
            var childCategories = db.Categories
                .Where(c => c.ParId == n.ParentCateId)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                })
                .ToList();
            ViewBag.ChildCategories = childCategories;

            ViewBag.ListProvider = new SelectList(db.Providers.ToList(), "ProviderId", "Name");

            if (UploadImage == null)
            {
                ModelState.AddModelError("Photo", "Vui lòng chọn ảnh sản phẩm");
                return View(n);
            }

            if ((n.Discount == null || n.Discount == 0) && (n.StartDate.HasValue || n.EndDate.HasValue))
            {
                ModelState.AddModelError("", "Không thể chọn ngày bắt đầu và kết thúc khi không có giảm giá");
                return View(n);
            }

            if ((n.Discount != null || n.Discount != 0) && (!n.StartDate.HasValue || !n.EndDate.HasValue))
            {
                ModelState.AddModelError("", "Chưa chọn ngày bắt đầu và kết thúc");
                return View(n);
            }

            if (n.StartDate.HasValue && n.EndDate.HasValue && n.StartDate > n.EndDate)
            {
                ModelState.AddModelError("EndDate", "Ngày kết thúc phải sau ngày bắt đầu");
                return View(n);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var model = new Product
                    {
                        Name = n.Name,
                        Description = n.Description,
                        CateId = n.CateId,
                        Price = n.Price,
                        Quantity = n.Quantity,
                        StartDate = n.Discount == 0 ? null : n.StartDate,
                        EndDate = n.Discount == 0 ? null : n.EndDate,
                        Discount = n.Discount ?? 0,
                        ProviderId = n.ProviderId,
                        IsHidden = false
                    };

                    var fileName = Path.GetFileName(UploadImage.FileName);
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);
                    UploadImage.SaveAs(path);
                    model.Photo = fileName;

                    db.Products.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Show");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi thêm sản phẩm: " + ex.Message);
                }
            }

            return View(n);
        }

        [HttpGet]
        [HasCredential(RoleId = "EDIT_PRODUCT")]
        public ActionResult Edit(int ProductId)
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            var product = db.Products.Find(ProductId);
            if (product == null)
                return HttpNotFound();

            var category = db.Categories.Find(product.CateId);
            if (category == null)
                return HttpNotFound();

            // Lấy danh sách loại cha
            var parentCategories = db.Categories
                .Where(c => c.ParId == 0)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name,
                    Selected = c.CategoryId == (category.ParId == 0 ? category.CategoryId : category.ParId)
                })
                .ToList();
            ViewBag.ParentCategories = parentCategories;

            // Lấy danh sách loại con
            var childCategories = db.Categories
                .Where(c => c.ParId == (category.ParId == 0 ? category.CategoryId : category.ParId))
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name,
                    Selected = c.CategoryId == product.CateId
                })
                .ToList();
            ViewBag.ChildCategories = childCategories;

            ViewBag.ListProvider = new SelectList(db.Providers.ToList(), "ProviderId", "Name");

            var model = new ProductViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Discount = product.Discount,
                ProviderName = db.Providers.Find(product.ProviderId).Name,
                CateName = db.Categories.Find(product.CateId).Name,
                Price = product.Price,
                Quantity = product.Quantity,
                StartDate = product.StartDate,
                EndDate = product.EndDate,
                Photo = product.Photo,
                CateId = product.CateId,
                ParentCateId = category.ParId == 0 ? category.CategoryId : category.ParId
            };

            return View(model);
        }

        [HttpPost]
        [HasCredential(RoleId = "EDIT_PRODUCT")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel model, HttpPostedFileBase UploadImage)
        {
            // Khởi tạo lại danh sách categories
            var parentCategories = db.Categories
                .Where(c => c.ParId == 0)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name,
                    Selected = c.CategoryId == model.ParentCateId
                })
                .ToList();
            ViewBag.ParentCategories = parentCategories;

            // Khởi tạo lại danh sách child categories
            var childCategories = db.Categories
                .Where(c => c.ParId == model.ParentCateId)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name,
                    Selected = c.CategoryId == model.CateId
                })
                .ToList();
            ViewBag.ChildCategories = childCategories;

            ViewBag.ListProvider = new SelectList(db.Providers.ToList(), "ProviderId", "Name");
            
            if (!ModelState.IsValid)
                return View(model);

            // Kiểm tra discount, startdate và enddate
            if (model.Discount == null || model.Discount == 0)
            {
                if (model.StartDate != null || model.EndDate != null)
                {
                    ModelState.AddModelError("", "Không thể chọn ngày bắt đầu và kết thúc khi không có giảm giá");
                    return View(model);
                }
            }

            try
            {
                var product = db.Products.Find(model.ProductId);
                if (product == null)
                    return HttpNotFound();

                // Kiểm tra quyền
                var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
                if (session.GroupId != "ADMIN" && product.IsHidden)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                // Cập nhật thông tin sản phẩm
                UpdateProductInfo(product, model, UploadImage);
                db.SaveChanges();

                return RedirectToAction("Show");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        [HasCredential(RoleId = "DELETE_PRODUCT")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var product = db.Products.Find(id);
                if (product != null)
                {
                    var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
                    bool isAdmin = session.GroupId == "ADMIN";
                    bool isProductOrdered = db.OrderDetails.Any(od => od.ProductId == id);

                    if (isProductOrdered || !isAdmin)
                    {
                        // MOD chỉ được ẩn sản phẩm
                        product.IsHidden = true;
                        db.Entry(product).State = EntityState.Modified;
                    }
                    else
                    {
                        // ADMIN mới được xóa hoàn toàn
                        db.Products.Remove(product);
                    }

                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi xóa sản phẩm" });
            }
        }

        private void UpdateProductInfo(Product product, ProductViewModel model, HttpPostedFileBase uploadImage)
        {
            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Quantity = model.Quantity;
            product.CateId = model.CateId;
            product.ProviderId = model.ProviderId;
            product.StartDate = model.StartDate;
            product.EndDate = model.EndDate;
            product.Discount = model.Discount ?? 0;
            product.IsHidden = model.IsHidden;

            if (uploadImage != null)
            {
                string fileName = Path.GetFileName(uploadImage.FileName);
                string path = Path.Combine(Server.MapPath("~/image"), fileName);
                uploadImage.SaveAs(path);
                product.Photo = fileName;
            }
        }
    }
}