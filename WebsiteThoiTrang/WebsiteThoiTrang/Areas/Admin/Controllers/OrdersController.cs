using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models.EF;
using WebsiteNoiThat.Areas.Admin.Models;
using WebsiteNoiThat.Common;

namespace WebsiteNoiThat.Areas.Admin.Controllers
{
    [HasCredential(RoleId = "SHOW_ORDER")]
    public class OrdersController : Controller
    {
        private readonly DBThoiTrang db = new DBThoiTrang();

        [HasCredential(RoleId = "SHOW_ORDER")]
        public ActionResult Show()
        {
            var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
            if (session == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            ViewBag.username = session.Username;

            // MOD có thể xem tất cả đơn hàng nhưng chỉ sửa được đơn chưa xử lý
            var query = from a in db.Orders
                        join b in db.Status on a.StatusId equals b.StatusId
                        select new OrderView
                        {
                            OrderId = a.OrderId,
                            ShipAddress = a.ShipAddress,
                            ShipEmail = a.ShipEmail,
                            ShipName = a.ShipName,
                            ShipPhone = a.ShipPhone,
                            StatusName = b.Name,
                            UpdateDate = a.UpdateDate,
                            UserId = a.UserId
                        };

            return View(query.ToList());
        }

        [HasCredential(RoleId = "SHOW_ORDER")]
        public ActionResult Details(int? id)
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            ViewBag.aaaa = db.Status.SingleOrDefault(x => x.StatusId == order.StatusId).Name;
            if (order == null)
            {
                return HttpNotFound();
            }
            else
            {
                var orderproducts = (
                                         from a in db.OrderDetails
                                         join b in db.Orders
                                         on a.OrderId equals b.OrderId
                                         join c in db.Products
                                         on a.ProductId equals c.ProductId
                                         select new OrderProduct
                                         {
                                             OrderId = a.OrderId,
                                             ProductName = c.Name,
                                             Quantity = a.Quantity,
                                             Price = c.Discount != null ?
                                                     a.Quantity * (c.Price - (c.Price * (c.Discount * 0.01))) :
                                                     a.Quantity * c.Price,
                                             ProductId = c.ProductId
                                         }
                                 ).Where(o=>o.OrderId==order.OrderId).ToList();
                ViewBag.orderproducts = orderproducts;

                double? total = 0;
                foreach(OrderProduct item in orderproducts)
                {
                    total += item.Price;
                }
                ViewBag.total = total;

                return View(order);
            }
        }

        [HttpGet]
        [HasCredential(RoleId = "MANAGER_ORDER")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            var currentStatus = db.Status.Find(order.StatusId);
            ViewBag.CurrentStatus = currentStatus?.Name;
            ViewBag.ListStatus = new SelectList(db.Status.ToList(), "StatusId", "Name", order.StatusId);

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            try
            {
                var existingOrder = db.Orders.Find(order.OrderId);
                if (existingOrder == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy đơn hàng");
                    ViewBag.ListStatus = new SelectList(db.Status.ToList(), "StatusId", "Name", order.StatusId);
                    return View(order);
                }

                var currentStatus = db.Status.Find(existingOrder.StatusId);
                if (currentStatus != null && currentStatus.Name == "Đã hoàn thành")
                {
                    ModelState.AddModelError("", "Không thể sửa đơn hàng đã hoàn thành");
                    ViewBag.CurrentStatus = currentStatus.Name;
                    ViewBag.ListStatus = new SelectList(db.Status.ToList(), "StatusId", "Name", order.StatusId);
                    return View(order);
                }

                existingOrder.StatusId = order.StatusId;
                existingOrder.UpdateDate = DateTime.Now;
                db.SaveChanges();

                return RedirectToAction("Show");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                ViewBag.ListStatus = new SelectList(db.Status.ToList(), "StatusId", "Name", order.StatusId);
                return View(order);
            }
        }

        [HttpPost]
        [HasCredential(RoleId = "DELETE_ORDER")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
                    if (session == null)
                    {
                        return Json(new { success = false, message = "Phiên đăng nhập đã hết hạn" });
                    }

                    var order = db.Orders.Find(id);
                    if (order == null)
                    {
                        return Json(new { success = false, message = "Không tìm thấy đơn hàng" });
                    }

                    // Kiểm tra trạng thái đơn hàng
                    var status = db.Status.Find(order.StatusId);
                    if (status != null && status.Name == "Đã hoàn thành")
                    {
                        return Json(new { success = false, message = "Không thể xóa đơn hàng đã hoàn thành" });
                    }

                    // Xóa chi tiết đơn hàng trước
                    var orderDetails = db.OrderDetails.Where(od => od.OrderId == id).ToList();
                    if (orderDetails.Any())
                    {
                        foreach (var detail in orderDetails)
                        {
                            db.OrderDetails.Remove(detail);
                        }
                        db.SaveChanges();
                    }

                    // Xóa đơn hàng
                    db.Orders.Remove(order);
                    db.SaveChanges();
                    
                    transaction.Commit();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // Lấy inner exception nếu có
                    var innerException = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return Json(new { success = false, message = "Có lỗi xảy ra: " + innerException });
                }
            }
        }
    }
}
