using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.DAO;
using WebsiteNoiThat.Models;

namespace WebsiteNoiThat.Controllers
{
    public class CategoryProductController : Controller
    {
        private readonly CategoryDao _categoryDao = new CategoryDao();

        public ActionResult Menu()
        {        
            var categories = _categoryDao.ListCategory();
            
            // Xử lý dữ liệu trước khi gửi đến view
            var viewModel = new List<CategoryViewModel>();
            var parentCategories = categories.Where(c => c.ParId == 0);

            foreach (var parent in parentCategories)
            {
                var childCategories = categories.Where(c => c.ParId == parent.CategoryId)
                                              .Select(c => new CategoryViewModel 
                                              {
                                                  CategoryId = c.CategoryId,
                                                  Name = c.Name,
                                                  ParId = c.ParId
                                              })
                                              .ToList();

                viewModel.Add(new CategoryViewModel
                {
                    CategoryId = parent.CategoryId,
                    Name = parent.Name,
                    ParId = parent.ParId,
                    ChildCategories = childCategories
                });
            }

            return PartialView(viewModel);
        }
    }
}