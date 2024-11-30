using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Models.DAO;
using Models.EF;
using PagedList;

namespace WebsiteNoiThat.Controllers
{
    public class ProductController : Controller
    {
        DBThoiTrang db  = new DBThoiTrang();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        //cho home page
        public ActionResult ProductHot()
        {
            var model = new ProductDao().ProductHot().Where(n => n.Quantity > 0).ToList();
            return PartialView(model);
        }
        public ActionResult SaleProduct()
        {
            var model = new ProductDao().SaleProduct().Where(n => n.Quantity > 0).ToList();
            return PartialView(model);
        }
        public ActionResult CateHavana()
        {
            var model = new ProductDao().CateHavana().Where(n => n.Quantity > 0).ToList();
            return PartialView(model);
        }
        public ActionResult CateKorea()
        {
            var model = new ProductDao().CateKorea().Where(n => n.Quantity > 0).ToList();
            return PartialView(model);
        }
        public ActionResult NewProduct()
        {
            var model = new ProductDao().NewProduct()
                                       .Where(n => n.Quantity > 0)
                                       .Take(8)  // Lấy 8 sản phẩm (2 dòng x 4 cột)
                                       .ToList();
            return PartialView(model);
        }
       
        public ActionResult DetailProduct(int Id)
        {
          var model = new ProductDao().DetailsProduct(Id);
            if (model == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(model);

        }

        //in ra theo danh mục sản phẩm
        public ActionResult CategoryShow(int cateid, int page = 1, int pagesize = 16)
        {
            var category = new CategoryDao().ViewDetail(cateid);
            ViewBag.CategoryShow = category;
            int total = 0;
            var model = new ProductDao().ListByCategoryId(cateid, ref total, page, pagesize);
            
            // Tính toán phân trang
            int totalPages = (int)Math.Ceiling((double)total/pagesize);
            
            ViewBag.Total = total;
            ViewBag.Page = page;
            ViewBag.Totalpage = totalPages;  // Đảm bảo tên ViewBag giống với view
            ViewBag.First = 1;
            ViewBag.Last = totalPages;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }

        //tìm kiếm
        public ActionResult Search(string keyword, int page = 1, int pagesize = 16)
        {
          
            int total = 0;
            var model = new ProductDao().Search(keyword, ref total, page, pagesize);
            ViewBag.Total = total;
            ViewBag.Page = page;
            int maxpage = 10;
            int totalpage = 0;
            totalpage = (int)Math.Ceiling((double)total / pagesize);
            ViewBag.Totalpage = totalpage;
            ViewBag.Maxpage = maxpage;
            ViewBag.First = 1;
            ViewBag.Keyword = keyword;
            ViewBag.Last = maxpage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }
        
        //tìm theo khoảng giá
        public ActionResult SearchFocus(int? priceRange, int? page)
        {
            try
            {
                int pageNumber = (page ?? 1);
                int pageSize = 16;
                ViewBag.SelectedPriceRange = priceRange;

                var query = db.Products.AsQueryable();
                query = query.Where(n => !n.IsHidden && n.Quantity > 0);

                if (priceRange.HasValue)
                {
                    switch (priceRange.Value)
                    {
                        case 0:
                            query = query.Where(n => n.Price <= 5000000);
                            break;
                        case 1:
                            query = query.Where(n => n.Price > 5000000 && n.Price <= 10000000);
                            break;
                        case 2:
                            query = query.Where(n => n.Price > 10000000 && n.Price <= 20000000);
                            break;
                        case 3:
                            query = query.Where(n => n.Price > 20000000 && n.Price <= 50000000);
                            break;
                        case 4:
                            query = query.Where(n => n.Price > 50000000);
                            break;
                    }
                }

                return View(query.OrderBy(n => n.Price).ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return View(new PagedList<Product>(new List<Product>(), 1, 16));
            }
        }

        public JsonResult ListName(string q)
        {
            var data = new ProductDao().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            },JsonRequestBehavior.AllowGet);
        }

        // Xem tất cả sản phẩm
         public ActionResult ShowProduct(int? page)
        {
            int pagenumber = (page ?? 1);
            int pagesize = 16;
            var model = db.Products.Where(n => !n.IsHidden && n.Quantity > 0)
                                  .OrderBy(n => n.Name)
                                  .ToPagedList(pagenumber, pagesize);
            return View(model);
        }

        // Xem tất cả sản phẩm hot
        public ActionResult Hots(int? page)
        {
            int pagenumber = (page ?? 1);
            int pagesize = 16;
            var model = (from a in db.Products
                        join b in db.OrderDetails on a.ProductId equals b.ProductId
                        where !a.IsHidden
                        group b by new { 
                            a.Name,           // Thêm Name vào đây
                            a.Description, 
                            a.ProductId, 
                            a.Photo, 
                            a.Price, 
                            a.Discount, 
                            a.EndDate, 
                            a.StartDate 
                        }
                        into g
                        select new ProductView
                        {
                            Name = g.Key.Name,     // Thêm dòng này
                            Description = g.Key.Description,
                            ProductId = g.Key.ProductId,
                            Price = g.Key.Price,
                            Discount = g.Key.Discount,
                            StartDate = g.Key.StartDate,
                            EndDate = g.Key.EndDate,
                            Photo = g.Key.Photo,
                            Quantity = g.Sum(s => s.Quantity),
                        }).OrderByDescending(n => n.Quantity)
                          .ToPagedList(pagenumber, pagesize);
            return View(model);
        }

        // Xem tất cả sản phẩm sale
        public ActionResult Sales(int? page)
        {
            int pagenumber = (page ?? 1);
            int pagesize = 16;
            var model = db.Products.Where(n => !n.IsHidden && n.Discount > 0 && n.Quantity > 0)
                                  .OrderBy(n => n.Name)
                                  .ToPagedList(pagenumber, pagesize);
            return View(model);
        }
      
        
    }
}