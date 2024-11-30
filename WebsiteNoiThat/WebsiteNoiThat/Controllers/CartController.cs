using Models.DAO;
using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebsiteNoiThat.Common;
using WebsiteNoiThat.Models;

namespace WebsiteNoiThat.Controllers
{
    public class CartController : Controller
    {

        DBThoiTrang db = new DBThoiTrang();
        private const string CartSession = "CartSession";
        private const string HistorySession = "HistorySession";

        public ActionResult Index()
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion];
            if (session != null)
            {
                var cart = Session[CartSession];
                var list = new List<CartItem>();
                if (cart != null)
                {
                    ViewBag.Status = "Đang chờ xác nhận";
                    list = (List<CartItem>)cart;
                }
                return View(list);
            }
            else
            {
                return Redirect("/dang-nhap");
            }
        }

        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Delete(int id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Product.ProductId == id);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public ActionResult DeleteItem(int id)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Tìm OrderDetail cần xóa
                    var orderDetail = db.OrderDetails.Find(id);
                    if (orderDetail != null)
                    {
                        // Lưu OrderId trước khi xóa OrderDetail
                        var orderId = orderDetail.OrderId;

                        // Xóa OrderDetail
                        db.OrderDetails.Remove(orderDetail);
                        db.SaveChanges();

                        // Đếm số OrderDetail còn lại của Order này
                        var remainingCount = db.OrderDetails.Count(x => x.OrderId == orderId);
                        
                        // In ra để debug
                        System.Diagnostics.Debug.WriteLine($"Remaining OrderDetails for Order {orderId}: {remainingCount}");

                        if (remainingCount == 0) // Nếu không còn OrderDetail nào
                        {
                            // Tìm và xóa Order
                            var order = db.Orders.Find(orderId);
                            if (order != null)
                            {
                                System.Diagnostics.Debug.WriteLine($"Deleting Order {orderId}");
                                db.Orders.Remove(order);
                                db.SaveChanges();
                            }
                        }

                        transaction.Commit();
                    }
                    return RedirectToAction("HistoryCart");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                    transaction.Rollback();
                    return RedirectToAction("HistoryCart");
                }
            }
        }
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];
            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ProductId == item.Product.ProductId);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public ActionResult AddCart(int productId, int quantity)
        {
            var product = new ProductDao().ViewDetail(productId);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ProductId == productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ProductId == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tạo mới đối tượng cart item
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                //Gán vào session
                Session[CartSession] = list;
                //Session[HistorySession] = list;
            }
            else
            {
                //tạo mới đối tượng cart item
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //Gán vào session
                Session[CartSession] = list;
                //Session[HistorySession] = list;
            }
            return RedirectToAction("Index");
        }
        

        [HttpGet]
        public ActionResult PayBy()
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion];
            if (session != null)
            {
                var model = db.Users.SingleOrDefault(n => n.UserId == session.UserId);
                var cart = Session[CartSession];
                var list = new List<CartItem>();
                decimal? total = 0;
                if (cart != null)
                {
                    ViewBag.Status = "Đang chờ xác nhận";
                    list = (List<CartItem>)cart;
                    
                    foreach(CartItem item in list)
                    {
                        decimal itemPrice = item.Product.Price ?? 0;
                        decimal? discountAmount = item.Product.Discount > 0 
                            ? (itemPrice * item.Product.Discount * 0.01M) 
                            : 0;
                        decimal? finalPrice = itemPrice - discountAmount;
                        total += finalPrice * item.Quantity;
                    }
                }
                ViewBag.ListItem = list;
                ViewBag.total = total;

                return View(model);
            }
            else
            {
                return Redirect("/dang-nhap");
            }
        }

        [HttpPost]
        public ActionResult PayBy(User n)
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion];
            var model = db.Users.SingleOrDefault(a => a.UserId == session.UserId);
            if (true == true)
            {
                model.UserId = session.UserId;
                model.Name = n.Name;
                model.Phone = n.Phone;
                model.Password = model.Password;
                model.GroupId = model.GroupId;
                model.Address = n.Address;

                model.Status = true;
                model.Email = n.Email;
                model.Username = session.Username;
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                var order = new Order();
                order.UpdateDate = DateTime.Now;
                //order.UpdateDate = DateTime.ToString("yyyy-MM-dd h:mm:ss");
                //DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                order.ShipAddress = n.Address;
                order.ShipPhone = n.Phone;
                order.ShipName = n.Name;
                order.ShipEmail = n.Email;
                order.UserId = session.UserId;
                order.StatusId = 1;


                var id = new OrderDao().Insert(order);
                var cart = (List<CartItem>)Session[CartSession];
                var detailDao = new OrderDetailDao();
                double total = 0;
                double a = 0;
                var htmldata = "<p><b>STT | Tên | Số lượng | Đơn giá | Khuyến mại</b></p>";
                int count = 0;
                foreach (var item in cart)
                {
                        var orderDetail = new OrderDetail();
                        orderDetail.OrderId = id;
                        orderDetail.ProductId = item.Product.ProductId;

                        //a = Convert.ToInt32(item.Product.Price);
                        var discountprice = Convert.ToInt32(item.Product.Price - item.Product.Price * item.Product.Discount * 0.01);
                        orderDetail.Price = discountprice;

                        orderDetail.Quantity = item.Quantity;

                        detailDao.Insert(orderDetail);
                        total += discountprice * item.Quantity;
                        var pro = db.Products.FirstOrDefault(m => m.ProductId == item.Product.ProductId);
                        pro.Quantity = pro.Quantity - item.Quantity;
                        htmldata += "<p>"+count+"  |  "+item.Product.Name+ "  |  "+item.Quantity+"  |  "+ discountprice.ToString("N0") +" | "+item.Product.Discount.ToString()+" %</p>";
                        db.SaveChanges();
                        count += 1;
                }

                string content = System.IO.File.ReadAllText(Server.MapPath("~/Common/neworder.html"));

                content = content.Replace("{{id}}", id.ToString());
                content = content.Replace("{{CustomerName}}", n.Name);
                content = content.Replace("{{Phone}}", n.Phone.ToString());
                content = content.Replace("{{Email}}", n.Email);
                content = content.Replace("{{Address}}", n.Address);
                content = content.Replace("{{Total}}", total.ToString("N0"));
                content = content.Replace("{{data}}", htmldata);

                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMail(n.Email, "Đơn hàng mới từ NOITHATGO.VN", content);
                //new MailHelper().SendMail(toEmail, "Đơn hàng mới từ NoiThatShop", content);

                ViewBag.EMAIL = n.Email;
                return Redirect("/hoan-thanh");
            }
            else
            {
                return Redirect("/Cart/Error");
            }
        }

        public ActionResult HistoryCart(int? page)
        {
            int pagenumber = (page ?? 1);
            int pagesize = 6;
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion];

            var model = (from a in db.OrderDetails
                         join b in db.Orders
                         on a.OrderId equals b.OrderId
                         join c in db.Products
                         on a.ProductId equals c.ProductId
                         join d in db.Status on b.StatusId equals d.StatusId
                         where b.UserId == session.UserId

                         select new HistoryCart
                         {
                             OrderDetaiId = a.OrderDetailId,
                             ProductId = a.ProductId,
                             Name = c.Name,
                             Photo = c.Photo,
                             Price = c.Price,
                             Quantity = a.Quantity,
                             Discount = c.Discount,
                             EndDate = c.EndDate,
                             StatusId = b.StatusId,
                             NameStatus = d.Name,
                             TotalPrice = a.Quantity *(c.Discount != null ?
                                            c.Price - (c.Price * c.Discount * 0.01) : 
                                            c.Price)
                         }).ToList();

            return View(model.ToPagedList(pagenumber, pagesize));

        }

        public ActionResult Success()
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion];
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
                ViewBag.Status = "Đã tiếp nhận";
                ViewBag.ListItem = list;
                Session["CartSession"] = null;
            }
            return View(list);
        }

        public ActionResult Error()
        {
            return View();
        }
        public ActionResult DeleteError()
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion];

            var model = (from a in db.OrderDetails
                         join b in db.Orders
                         on a.OrderId equals b.OrderId
                         join c in db.Products
                         on a.ProductId equals c.ProductId
                         join d in db.Status on b.StatusId equals d.StatusId
                         where b.UserId == session.UserId

                         select new HistoryCart
                         {
                             OrderDetaiId = a.OrderDetailId,
                             ProductId = a.ProductId,
                             Name = c.Name,
                             Photo = c.Photo,
                             Price = a.Price,
                             Quantity = a.Quantity,
                             Discount = c.Discount,
                             StatusId = b.StatusId,
                             NameStatus = d.Name
                         }).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult CheckoutProcess()
        {
            var cart = (List<CartItem>)Session[CartSession];
            if (cart != null && cart.Any())
            {
                var errorMessages = new List<string>();
                
                foreach (var item in cart)
                {
                    var product = db.Products.Find(item.Product.ProductId);
                    if (product == null)
                    {
                        errorMessages.Add($"Sản phẩm có ID {item.Product.ProductId} không tồn tại trong hệ thống");
                        continue;
                    }

                    if (item.Quantity <= 0)
                    {
                        errorMessages.Add($"Số lượng sản phẩm '{product.Name}' phải lớn hơn 0");
                        continue;
                    }
                    
                    if (product.Quantity <= 0)
                    {
                        errorMessages.Add($"Sản phẩm '{product.Name}' đã hết hàng");
                        continue;
                    }
                    
                    if (item.Quantity > product.Quantity)
                    {
                        errorMessages.Add($"Sản phẩm '{product.Name}' trong kho chỉ còn {product.Quantity}");
                    }
                }

                if (errorMessages.Any())
                {
                    TempData["ErrorMessage"] = string.Join("<br/>", errorMessages);
                    return RedirectToAction("Index");
                }

                return RedirectToAction("PayBy");
            }

            TempData["ErrorMessage"] = "Giỏ hàng trống";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult UpdateQuantity(int productId, int quantity)
        {
            var cart = Session[CartSession];
            var cartItems = (List<CartItem>)cart;

            var item = cartItems.FirstOrDefault(x => x.Product.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
                Session[CartSession] = cartItems;
                return Json(new { success = true, message = "Cập nhật thành công" });
            }
            return Json(new { success = false, message = "Cập nhật thất bại" });
        }
    }
}