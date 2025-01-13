using Microsoft.AspNetCore.Mvc;
using Shopping.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using X.PagedList;

namespace Shopping.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBContext _dbContext;

        public HomeController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Index 方法
        public IActionResult Index(int? page, int? CategoryId)
        {
            // 設置分頁參數
            int pageNumber = page ?? 1;
            int pageSize = 8;

            // 設置 ViewData 用於 "active" 樣式
            ViewData["CurrentController"] = "Home";
            ViewData["CurrentAction"] = "Index";

            // 讀取數據
            List<Banner> banners = _dbContext.Banners.ToList();
            List<ProductCategory> productCategories = _dbContext.ProductCategory.ToList();

            IQueryable<Product> products = _dbContext.Product.Where(x => x.Status == ProductStatus.上架);

            if (CategoryId.HasValue)
            {
                products = products.Where(x => x.ProductCategoryId == CategoryId);
            }

            // 設置 ViewBag 和 ViewData
            ViewBag.Banners = banners;
            ViewBag.ProductCategories = productCategories;
            ViewData["CategoryId"] = CategoryId;

            // 返回分頁結果
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        // Detail 方法
        public IActionResult Detail(int? Id)
        {
            if (!Id.HasValue)
            {
                return NotFound();
            }

            // 查詢產品，包含產品種類
            Product product = _dbContext.Product
                .Include(x => x.ProductCategory)
                .FirstOrDefault(x => x.Id == Id);

            if (product == null)
            {
                return NotFound();
            }

            // 設置 ViewData 用於 "active" 樣式
            ViewData["CurrentController"] = "Home";
            ViewData["CurrentAction"] = "Detail";

            return View(product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
