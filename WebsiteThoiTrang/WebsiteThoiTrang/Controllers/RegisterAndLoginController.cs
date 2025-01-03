using BotDetect.Web.Mvc;
using Models.DAO;
using Models.EF;
using reCAPTCHA.MVC;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteNoiThat.Areas.Admin.Models;
using WebsiteNoiThat.Common;
using WebsiteNoiThat.Models;

namespace WebsiteNoiThat.Controllers
{
    public class RegisterAndLoginController : Controller
    {
        // GET: RegisterAndLogin
        DBThoiTrang db = new DBThoiTrang();

        public ActionResult Logout()
        {
            Session[Commoncontent.user_sesion] = null;
            Session[Commoncontent.CartSession] = null;
            return Redirect("/");
        }
        
        [HttpGet]
        public ActionResult Login()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Login(Models.LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                //var result = dao.Login(model.UserName, model.Password);
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));

                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.Username = user.Username;
                    userSession.UserId = user.UserId;
                    Session.Add(Commoncontent.user_sesion, userSession);
                    return Redirect("/");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài Khoản đang bị khóa, liên hệ admin");

                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }
            }
            return View(model);
        }
        [HttpGet]
        
        public ActionResult Register()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if(ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                //else if (dao.CheckEmail(model.Email))
                //{
                //    ModelState.AddModelError("", "Email đã tồn tại");
                //}
                else
                {
                    var user = new User();
                    user.Username = model.UserName;
                    user.Password = Encryptor.MD5Hash(model.Password);
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.Name = model.Name;
                    user.GroupId = "USER";

                    user.Status = true;

                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        var models = db.Users.SingleOrDefault(n => n.Username == model.UserName);
                        return RedirectToAction("Card", new { UserId= models.UserId });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công.");
                    }
                }
            }
            model = new RegisterModel();
            return View();
        }

        [HttpGet]
        public ActionResult ViewCurentUser()
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion];
            if (session != null)
            {
                var model = db.Users.SingleOrDefault(n => n.UserId == session.UserId);
                return View(model);
            }
            else
            {
                return Redirect("/RegisterAndLogin/Login");
            }
        }

        [HttpGet]
        public ActionResult EditCurentUser()
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion];
            if (session == null)
                return RedirectToAction("Login");

            var user = db.Users.SingleOrDefault(n => n.UserId == session.UserId);
            var model = new UserEditModel
            {
                UserId = user.UserId,
                Username = user.Username,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                GroupId = user.GroupId,
                Status = user.Status
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCurentUser(UserEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = db.Users.Find(model.UserId);
                    if (currentUser != null)
                    {
                        currentUser.Name = model.Name;
                        currentUser.Address = model.Address;
                        currentUser.Phone = model.Phone;
                        currentUser.Email = model.Email;
                        
                        if (!string.IsNullOrEmpty(model.Password?.Trim()))
                        {
                            currentUser.Password = Encryptor.MD5Hash(model.Password);
                        }

                        db.SaveChanges();
                        TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                        return RedirectToAction("ViewCurentUser");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thông tin: " + ex.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Card(int? userId = null)
        {
            var session = (UserLogin)Session[Commoncontent.user_sesion];
            
            // Nếu đã đăng nhập
            if (session != null)
            {
                // Kiểm tra xem user đã có thẻ chưa
                var existingCard = db.Cards.FirstOrDefault(x => x.UserId == session.UserId);
                if (existingCard != null)
                {
                    ModelState.AddModelError("", "Bạn đã có thẻ tích điểm rồi!");
                    return View();
                }

                // Tạo model mới với UserId từ session
                var model = new Card
                {
                    UserId = session.UserId,
                    NumberCard = 0,
                };
                return View(model);
            }

            // Nếu chưa đăng nhập
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult Card(Card n)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(n);
                }

                // Kiểm tra user có tồn tại không
                var user = db.Users.FirstOrDefault(x => x.UserId == n.UserId);
                if (user == null)
                {
                    ModelState.AddModelError("", "Mã khách hàng không tồn tại!");
                    return View(n);
                }

                // Kiểm tra xem user đã có thẻ chưa
                var existingCard = db.Cards.FirstOrDefault(x => x.UserId == n.UserId);
                if (existingCard != null)
                {
                    ModelState.AddModelError("", "Bạn đã có thẻ tích điểm rồi!");
                    return View(n);
                }

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var card = new Card
                        {
                            UserId = n.UserId,
                            NumberCard = 0,
                        };

                        db.Cards.Add(card);
                        db.SaveChanges();

                        transaction.Commit();
                        TempData["Success"] = "Đăng ký thẻ thành công";
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Log error
                        ModelState.AddModelError("", "Có lỗi xảy ra khi đăng ký thẻ: " + ex.Message);
                        return View(n);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi hệ thống: " + ex.Message);
                return View(n);
            }
        }
        
        public ActionResult ViewLogin()
        {
            var session = (UserLogin)Session[WebsiteNoiThat.Common.Commoncontent.user_sesion];
            
            if(session != null)
            {
                var card = db.Cards.FirstOrDefault(n => n.UserId == session.UserId);
                
                if (card != null)
                {
                    ViewBag.Card = card.NumberCard;
                    var completedOrders = (from a in db.OrderDetails
                                join b in db.Orders on a.OrderId equals b.OrderId
                                join c in db.Products on a.ProductId equals c.ProductId
                                where b.StatusId == 5 && b.UserId == session.UserId
                                select new
                                {
                                    Price = a.Price,
                                    Quantity = a.Quantity,
                                    Discount = c.Discount
                                }).ToList();

                    if (completedOrders.Any())
                    {
                        double? total = completedOrders.Sum(item => 
                            (item.Price.GetValueOrDefault(0) - 
                            (item.Price.GetValueOrDefault(0) * item.Discount.GetValueOrDefault(0) * 0.01)) 
                            * item.Quantity);
                        
                        db.SaveChanges();
                    }
                }
            }
            return PartialView();
        }




    }
}
