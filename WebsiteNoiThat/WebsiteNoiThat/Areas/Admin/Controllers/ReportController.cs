using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteNoiThat.Models;
using Rotativa;
using WebsiteNoiThat.Common;
using PagedList;
using System.Net;
using WebsiteNoiThat.Areas.Admin.Models;

namespace WebsiteNoiThat.Areas.Admin.Controllers
{
    public class ReportController : HomeController
    {

        DBThoiTrang db = new DBThoiTrang();

        [HttpGet]
        [HasCredential(RoleId = "SHOW_ORDER")]
        public ActionResult Viewmodel()
        {
            var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
            ViewBag.username = session.Username;

            var model = (from a in db.Orders
                        join b in db.OrderDetails on a.OrderId equals b.OrderId
                        join c in db.Products on b.ProductId equals c.ProductId
                        join d in db.Status on a.StatusId equals d.StatusId
                        select new OrderViewModel
                        {
                            OrderId = a.OrderId,
                            ShipName = a.ShipName,
                            ShipAddress = a.ShipAddress,
                            ShipPhone = a.ShipPhone,
                            Price = b.Price,
                            Quantity = b.Quantity,
                            Discount = c.Discount,
                            UpdateDate = a.UpdateDate,
                            StatusId = a.StatusId,
                            StatusName = d.Name
                        }).ToList();

            decimal total = 0;
            foreach(var item in model)
            {
                if (item.Price.HasValue && item.Quantity.HasValue && item.Discount.HasValue)
                {
                    total += item.Price.Value * item.Quantity.Value * (1 - item.Discount.Value/100);
                }
            }
            ViewBag.total = total;

            return View(model);
        }

        [HttpPost]
        [HasCredential(RoleId = "SHOW_ORDER")]
        public ActionResult Viewmodel(DateTime? dfr, DateTime? dto)
        {
            var model = (from a in db.Orders
                        join b in db.OrderDetails on a.OrderId equals b.OrderId
                        join c in db.Products on b.ProductId equals c.ProductId
                        join d in db.Status on a.StatusId equals d.StatusId
                        where a.UpdateDate >= dfr && a.UpdateDate <= dto
                        select new OrderViewModel
                        {
                            OrderId = a.OrderId,
                            ShipName = a.ShipName,
                            ShipAddress = a.ShipAddress,
                            ShipPhone = a.ShipPhone,
                            Price = b.Price ?? 0,
                            Quantity = b.Quantity ?? 0,
                            Discount = c.Discount ?? 0,
                            UpdateDate = a.UpdateDate,
                            StatusId = a.StatusId,
                            StatusName = d.Name,
                            Total = (b.Price ?? 0) * (b.Quantity ?? 0) * (1 - (c.Discount ?? 0) / 100)
                        }).ToList();

            ViewBag.total = model.Sum(x => x.Total);
            return View(model);
        }

        [HasCredential(RoleId = "SHOW_ORDER")]
        public ActionResult Details(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null) return RedirectToAction("Viewmodel");

            var orderProducts = (from a in db.OrderDetails
                               join b in db.Products on a.ProductId equals b.ProductId
                               where a.OrderId == id
                               select new OrderProduct
                               {
                                   ProductId = b.ProductId,
                                   ProductName = b.Name,
                                   Quantity = a.Quantity,
                                   Price = a.Price
                               }).ToList();

            ViewBag.orderproducts = orderProducts;
            ViewBag.total = orderProducts.Sum(x => x.Price * x.Quantity);
            ViewBag.status = db.Status.Find(order.StatusId).Name;

            return View(order);
        }

        

        [HasCredential(RoleId = "PRINT_ORDER")]
        public ActionResult PrintSalarySlip(int id)
        {
            return new ActionAsPdf("PrintPreview", new { id = id })
            {
                FileName = "DonHang_" + id + ".pdf"
            };
        }

        [HttpGet]
        [HasCredential(RoleId = "PRINT_ORDER")]
        public ActionResult PrintPreview(int id)
        {
            var session = (UserLogin)Session[Commoncontent.user_sesion_admin];
            if (session == null)
            {
                return RedirectToAction("Login", "User", new { area = "Admin" });
            }

            Order order = db.Orders.Find(id);
            if (order == null) return RedirectToAction("Viewmodel");

            var orderProducts = (from a in db.OrderDetails
                        join b in db.Products on a.ProductId equals b.ProductId
                        where a.OrderId == id
                        select new OrderProduct
                        {
                            ProductId = b.ProductId,
                            ProductName = b.Name,
                            Quantity = a.Quantity,
                            Price = a.Price
                        }).ToList();

            ViewBag.orderproducts = orderProducts;
            ViewBag.total = orderProducts.Sum(x => x.Price * x.Quantity);
            ViewBag.status = db.Status.Find(order.StatusId).Name;
            ViewBag.username = session.Username;

            return View(order);
        }

    }
}