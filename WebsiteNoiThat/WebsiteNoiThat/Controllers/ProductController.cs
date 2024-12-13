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
        private readonly DBThoiTrang db = new DBThoiTrang();
        private readonly ProductDao productDao = new ProductDao();
        private ProductView ConvertToProductView(Product product)
        {
            var productView = new ProductView
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Discount = product.Discount,
                StartDate = product.StartDate,
                EndDate = product.EndDate,
                Photo = product.Photo,
                Quantity = product.Quantity,
                DiscountPrice = product.Discount > 0 &&
                                product.StartDate.HasValue && product.EndDate.HasValue &&
                                DateTime.Now >= product.StartDate && DateTime.Now <= product.EndDate
                                ? product.Price - (product.Price * product.Discount / 100)
                                : product.Price
            };

            return productView;
        }
        //cho home page
        //public ActionResult ProductHot()
        //{
        //    var products = productDao.ProductHot()
        //                           .Where(n => n.Quantity > 0)
        //                           .ToList();
        //    return PartialView(products);
        //}
        public ActionResult SaleProduct()
        {
            var products = productDao.SaleProduct()
                                   .Where(n => n.Quantity > 0)
                                   .Select(ConvertToProductView)
                                   .Take(8)
                                   .ToList();
            return PartialView(products);
        }
        public ActionResult CateHavana()
        {
            var products = productDao.CateHavana()
                                   .Where(n => n.Quantity > 0)
                                   .Select(ConvertToProductView)
                                   .Take(4)
                                   .ToList();
            return PartialView(products);
        }
        public ActionResult CateKorea()
        {
            var products = productDao.CateKorea()
                                   .Where(n => n.Quantity > 0)
                                   .Select(ConvertToProductView)
                                   .Take(4)
                                   .ToList();
            return PartialView(products);
        }
        public ActionResult NewProduct()
        {
            var products = productDao.NewProduct()
                                   .Where(n => n.Quantity > 0)
                                   .Select(ConvertToProductView)
                                   .Take(8)
                                   .ToList();
            return PartialView(products);
        }
        public ActionResult DetailProduct(int Id)
        {
            var product = productDao.DetailsProduct(Id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(product);
        }

        public ActionResult CategoryShow(int? cateid, int page = 1, int pagesize = 15)
        {
            if (!cateid.HasValue)
            {
                return RedirectToAction("ShowProduct");
            }

            var categoryDao = new CategoryDao();
            var category = categoryDao.ViewDetail(cateid.Value);
            if (category == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryShow = category.Name;
            int total = 0;
            var model = productDao.ListByCategoryId(cateid.Value, ref total, page, pagesize)
                                  .Select(ConvertToProductView);
            
            ViewBag.Total = total;
            ViewBag.Page = page;
            ViewBag.Totalpage = (int)Math.Ceiling((double)total/pagesize);
            ViewBag.First = 1;
            ViewBag.Last = ViewBag.Totalpage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            ViewBag.CategoryId = cateid.Value;

            return View(model);
        }

        public ActionResult Search(string keyword, int page = 1, int pagesize = 16)
        {
            try
            {
                int total = productDao.ShowProduct().Where(x => x.Name.Contains(keyword) && !x.IsHidden).Count();

                var model = productDao.ShowProduct().Where(n => n.Name.Contains(keyword) && !n.IsHidden)
                                      .OrderByDescending(x => x.Price)
                                      .Skip((page - 1) * pagesize)
                                      .Take(pagesize)
                                      .ToList()
                                      .Select(ConvertToProductView)
                                      .ToList();

                ViewBag.Total = total;
                ViewBag.Page = page;
                int maxpage = 10;
                int totalpage = (int)Math.Ceiling((double)total / pagesize);
                ViewBag.Totalpage = totalpage;
                ViewBag.Maxpage = maxpage;
                ViewBag.First = 1;
                ViewBag.Keyword = keyword;
                ViewBag.Last = maxpage;
                ViewBag.Next = page + 1;
                ViewBag.Prev = page - 1;

                return View(model);
            }
            catch
            {
                return View(new List<ProductView>());
            }
        }

        public ActionResult SearchFocus(int? priceRange, int? page)
        {
            try
            {
                int pageNumber = (page ?? 1);
                int pageSize = 15;
                ViewBag.SelectedPriceRange = priceRange;
                var products = db.Products.AsQueryable();
                products = products.Where(n => !n.IsHidden && n.Quantity > 0);

                if (priceRange.HasValue)
                {
                    switch (priceRange.Value)
                    {
                        case 0: products = products.Where(n => n.Price <= 5000000); break;
                        case 1: products = products.Where(n => n.Price > 5000000 && n.Price <= 10000000); break;
                        case 2: products = products.Where(n => n.Price > 10000000 && n.Price <= 20000000); break;
                        case 3: products = products.Where(n => n.Price > 20000000 && n.Price <= 50000000); break;
                        case 4: products = products.Where(n => n.Price > 50000000); break;
                    }
                }
                //var products = productDao.SearchByPriceRange(priceRange).Select(ConvertToProductView);
                return View(products.Select(ConvertToProductView).ToPagedList(pageNumber, pageSize));
            }
            catch
            {
                return View(new PagedList<ProductView>(new List<ProductView>(), 1, 16));
            }
        }

        public ActionResult ShowProduct(int? page)
        {
            int pagenumber = (page ?? 1);
            int pagesize = 15;
            
            var products = productDao.ShowProduct()
                .OrderBy(n => n.Name)
                .AsEnumerable()
                .Select(ConvertToProductView)
                .ToList();

            return View(products.ToPagedList(pagenumber, pagesize));
        }

        public ActionResult Hots(int? page)
        {
            int pagenumber = (page ?? 1);
            int pagesize = 15;
            var query = productDao.Hots()
                .OrderBy(n => n.Name)
                .AsEnumerable()
                .Select(ConvertToProductView)
                .ToList();

            return View(query.ToPagedList(pagenumber, pagesize));
        }

        public ActionResult Sales(int? page)
        {
            int pagenumber = (page ?? 1);
            int pagesize = 15;
            var products = productDao.SaleProduct().Select(ConvertToProductView);
            return View(products.ToPagedList(pagenumber, pagesize));
        }

        public JsonResult ListName(string q)
        {
            var data = productDao.ListName(q);
            return Json(new { data, status = true }, JsonRequestBehavior.AllowGet);
        }
    }
}