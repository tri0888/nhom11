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
       
        

        public List<Product> CateKorea()
        {
            return db.Products
                .Where(n => !n.IsHidden && n.CateId == 9)
                .AsEnumerable()
                .ToList();
        }

        public List<Product> CateHavana()
        {
            return db.Products
                .Where(n => !n.IsHidden && n.CateId == 8)
                .AsEnumerable()
                .ToList();
        }

        //public List<Product> ProductHot()
        //{
        //    var products = db.Products
        //        .Where(x => !x.IsHidden)
        //        .OrderByDescending(x => x.OrderDetails.Sum(od => od.Quantity))
        //        .Take(8)
        //        .ToList();

        //    return products.Select(p => {
        //        var productView = ConvertToProductView(p);
        //        productView.Quantity = p.OrderDetails.Sum(od => od.Quantity ?? 0);
        //        return productView;
        //    }).ToList();
        //}

        public List<Product> SaleProduct()
        {
            return db.Products
                .Where(x => !x.IsHidden && x.Discount > 0)
                .OrderByDescending(x => x.Discount)
                .AsEnumerable()
                .ToList();
        }

        public List<Product> NewProduct()
        {
            return db.Products
                .Where(n => !n.IsHidden)
                .OrderByDescending(n => n.StartDate)
                .AsEnumerable()
                .ToList();
        }

        public List<Product> ShowProduct()
        {
            return db.Products
                .Where(n => !n.IsHidden)
                .AsEnumerable()
                .ToList();
        }

        public List<Product> Hots()
        {
            var hotProducts = (from a in db.Products
                             join b in db.OrderDetails on a.ProductId equals b.ProductId
                             where !a.IsHidden
                             group b by a into g
                             select new
                             {
                                 Product = g.Key,
                                 Quantity = g.Sum(s => s.Quantity ?? 0)
                             })
                             .OrderByDescending(x => x.Quantity)
                             .Select(x => x.Product)
                             .ToList();

            return hotProducts;
        }

        public Product DetailsProduct(int Id)
        {
            return db.Products.SingleOrDefault(n => n.ProductId == Id);
        }

        public List<Product> ListByCategoryId(int cateId, ref int total, int pageindex = 1, int pagesize = 12)
        {
            try
            {
                // Lấy tất cả danh mục con
                var allCateIds = new List<int> { cateId };
                var childCates = db.Categories
                    .Where(x => x.ParId == cateId)
                    .Select(x => x.CategoryId)
                    .ToList();
                allCateIds.AddRange(childCates);

                // Query sản phẩm từ cả danh mục cha và con
                var query = db.Products
                    .Where(x => allCateIds.Contains((int)x.CateId) && !x.IsHidden);

                total = query.Count();

                return query.OrderByDescending(x => x.Price)
                           .Skip((pageindex - 1) * pagesize)
                           .Take(total)
                           .AsEnumerable()
                           .ToList();
            }
            catch (Exception)
            {
                total = 0;
                return new List<Product>();
            }
        }

        public List<string> ListName(string keyword)
        {
            return db.Products
                .Where(n => n.Name.Contains(keyword))
                .Select(n => n.Name)
                .Distinct()
                .ToList();
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
                return autoSave && db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
    
}
