using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EF;
namespace Models.DAO
{
   
    public class ProductDao
    {
        
         DBThoiTrang db  = new DBThoiTrang();
       
        public List<Product> ListSanPham()
        {
            return db.Products.Where(p => !p.IsHidden).ToList();
        }
        public List<Product> CateKorea()
        {
            return db.Products.Where(n => !n.IsHidden && 
                                        (n.Discount == 0 || n.EndDate < DateTime.Now || n.StartDate > DateTime.Now))
                             .Where(n => n.CateId == 9)
                             .Take(4)
                             .ToList();
        }
        public List<Product> CateHavana()
        {
            return db.Products.Where(n => !n.IsHidden && 
                                        (n.Discount == 0 || n.EndDate < DateTime.Now || n.StartDate > DateTime.Now))
                             .Where(n => n.CateId == 8)
                             .Take(4)
                             .ToList();
        }
        public List<ProductView> ProductHot()
        {
            var model = (from a in db.Products
                         join b in db.OrderDetails on a.ProductId equals b.ProductId
                         where !a.IsHidden
                         group b by new { a.Description,a.ProductId,a.Photo,a.Price,a.Discount,a.EndDate,a.StartDate } 
                         into g
                         select new ProductView
                         {
                            Description= g.Key.Description,
                            ProductId=g.Key.ProductId,
                            Price=g.Key.Price,
                            Discount=g.Key.Discount,
                            StartDate=g.Key.StartDate,
                            EndDate=g.Key.EndDate,
                            Photo=g.Key.Photo,
                          
                            Quantity = g.Sum(s => s.Quantity),

                         }).OrderByDescending(n => n.Quantity).Take(6).ToList();
           return model;
        }

        public List<Product> SaleProduct() { 
            return db.Products.Where(n => !n.IsHidden && n.Discount > 0)
                             .OrderByDescending(n => n.Discount)
                             .Take(8)
                             .ToList();
        }
        
        public List<Product> NewProduct()
        {
            return db.Products.Where(n => !n.IsHidden && 
                                        (n.Discount == 0 || n.EndDate < DateTime.Now || n.StartDate > DateTime.Now))
                             .OrderByDescending(n => n.StartDate)
                             .Take(9)
                             .ToList();
        }
        public Product DetailsProduct(int Id)
        {
           return db.Products.SingleOrDefault(n => n.ProductId == Id);
        }
        
        //in ra loai san pham
       
        public List<Product> ListByCategoryId(int cateId, ref int total, int pageindex = 1, int pagesize = 12)
        {
            try
            {
                // Lấy tất cả danh mục con
                var allCateIds = new List<int> { cateId };
                var childCates = db.Categories
                    .Where(x => x.ParId.HasValue && x.ParId.Value == cateId)
                    .Select(x => x.CategoryId)
                    .ToList();
                allCateIds.AddRange(childCates);

                // Query sản phẩm từ cả danh mục cha và con
                var query = db.Products.Where(x => allCateIds.Contains((int)x.CateId));
                
                // Lấy tổng số sản phẩm
                total = query.Count();

                // Lấy sản phẩm theo trang
                var model = query
                    .OrderByDescending(x => x.Price)
                    .Skip((pageindex - 1) * pagesize)
                    .Take(pagesize)
                    .ToList();

                return model;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ListByCategoryId: {ex.Message}");
                total = 0;
                return new List<Product>();
            }
        }
        public List<string> ListName(string keyword)
        {
        
            return db.Products.Where(n => n.Name.Contains(keyword)).Select(n => n.Name).Distinct().ToList();
        }
        public List<Product> Search(string keyword, ref int total, int pageindex = 1, int pagesize = 12)
        {
            total = db.Products.Where(x => x.Name.Contains(keyword) && !x.IsHidden).Count();
            var model = db.Products.Where(n => n.Name.Contains(keyword) && !n.IsHidden)
                                  .OrderByDescending(x => x.Price)
                                  .Skip((pageindex - 1) * pagesize)
                                  .Take(pagesize)
                                  .ToList();
            return model;
        }
        public Product ViewDetail(int id)
        {
            return db.Products.Find(id);
        }
        public bool Update(Product entity, bool autoSave = true)
        {
            try
            {

                db.Products.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                if (autoSave)
                    return Convert.ToBoolean(db.SaveChanges());
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }


    }
    
}
