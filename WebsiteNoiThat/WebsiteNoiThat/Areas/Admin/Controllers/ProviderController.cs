﻿using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteNoiThat.Common;

namespace WebsiteNoiThat.Areas.Admin.Controllers
{
    public class ProviderController : HomeController
    {
        // GET: Admin/Provider
        DBThoiTrang db = new DBThoiTrang();

        [HasCredential(RoleId = "VIEW_PROVIDER")]
        public ActionResult Show()
        {
            return View(db.Providers.ToList());
        }

        [HttpGet]
        [HasCredential(RoleId = "ADD_PROVIDER")]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [HasCredential(RoleId = "ADD_PROVIDER")]
        public ActionResult Add(Provider n)
        {
            var model = db.Providers.SingleOrDefault(a => a.ProviderId == n.ProviderId);
            if (model != null)
            {
                ModelState.AddModelError("ProError", "Id already in use");
                return View();
            }
            else
            {
                db.Providers.Add(n);
                db.SaveChanges();
                return RedirectToAction("Show");
            }

        }
        [HttpGet]
        [HasCredential(RoleId = "EDIT_PROVIDER")]
        public ActionResult Edit(int ProviderId)
        {
            Provider a = db.Providers.SingleOrDefault(n => n.ProviderId == ProviderId);
            if (a == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("Show");
            }
            return View(a);

        }

        [HttpPost]
        [HasCredential(RoleId = "EDIT_PROVIDER")]
        public ActionResult Edit(Provider n)
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
        [HasCredential(RoleId = "DELETE_PROVIDER")]
        public ActionResult Delete(int id)
        {
            try
            {
                var provider = db.Providers.Find(id);
                if (provider == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy nhà cung cấp" });
                }

                // Kiểm tra xem nhà cung cấp có sản phẩm nào không
                var hasProducts = db.Products.Any(p => p.ProviderId == id);
                if (hasProducts)
                {
                    var productCount = db.Products.Count(p => p.ProviderId == id);
                    return Json(new { 
                        success = false, 
                        message = $"Không thể xóa nhà cung cấp này vì đang có {productCount} sản phẩm. Vui lòng xóa hoặc chuyển các sản phẩm sang nhà cung cấp khác trước."
                    });
                }

                // Nếu không có sản phẩm nào thì tiến hành xóa
                db.Providers.Remove(provider);
                db.SaveChanges();

                return Json(new { success = true, message = "Xóa nhà cung cấp thành công" });
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