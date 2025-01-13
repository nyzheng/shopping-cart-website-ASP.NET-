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

        // Index ��k
        public IActionResult Index(int? page, int? CategoryId)
        {
            // �]�m�����Ѽ�
            int pageNumber = page ?? 1;
            int pageSize = 8;

            // �]�m ViewData �Ω� "active" �˦�
            ViewData["CurrentController"] = "Home";
            ViewData["CurrentAction"] = "Index";

            // Ū���ƾ�
            List<Banner> banners = _dbContext.Banners.ToList();
            List<ProductCategory> productCategories = _dbContext.ProductCategory.ToList();

            IQueryable<Product> products = _dbContext.Product.Where(x => x.Status == ProductStatus.�W�[);

            if (CategoryId.HasValue)
            {
                products = products.Where(x => x.ProductCategoryId == CategoryId);
            }

            // �]�m ViewBag �M ViewData
            ViewBag.Banners = banners;
            ViewBag.ProductCategories = productCategories;
            ViewData["CategoryId"] = CategoryId;

            // ��^�������G
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        // Detail ��k
        public IActionResult Detail(int? Id)
        {
            if (!Id.HasValue)
            {
                return NotFound();
            }

            // �d�߲��~�A�]�t���~����
            Product product = _dbContext.Product
                .Include(x => x.ProductCategory)
                .FirstOrDefault(x => x.Id == Id);

            if (product == null)
            {
                return NotFound();
            }

            // �]�m ViewData �Ω� "active" �˦�
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
