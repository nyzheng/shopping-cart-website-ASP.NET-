using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Models;

namespace Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class ProductsController : Controller
    {
        private readonly DBContext _context;

        public ProductsController(DBContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var dBContext = _context.Product.Include(p => p.ProductCategory);
            return View(await dBContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategory, "Id", "CategoryName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            // 移除 RegularExpression 的驗證錯誤
            ModelState.Remove("Image");
            if (ModelState.IsValid)
            {
                // 手動驗證圖片格式
                if (product.Image != null && product.Image.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    var fileExtension = Path.GetExtension(product.Image.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("Image", "僅允許上傳圖片格式 (jpg, jpeg, png, gif, bmp)。");
                        return View(product); // 回到表單顯示錯誤訊息
                    }

                    // 儲存圖片到指定目錄
                    var fileName = Path.GetFileName(product.Image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UpImages", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.Image.CopyToAsync(stream);
                    }

                    // 設定圖片路徑到 Banner
                    product.ImagePath = fileName;

                    // 將資料存入資料庫
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Image", "請選擇要上傳的圖片。");
                }
            }
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategory, "Id", "CategoryName", product.ProductCategoryId);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategory, "Id", "CategoryName", product.ProductCategoryId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            // 移除 RegularExpression 的驗證錯誤
            ModelState.Remove("Image");
            if (ModelState.IsValid)
            {
                try
                {
                    // 手動驗證圖片格式
                    if (product.Image != null && product.Image.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                        var fileExtension = Path.GetExtension(product.Image.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("Image", "僅允許上傳圖片格式 (jpg, jpeg, png, gif, bmp)。");
                            return View(product); // 回到表單顯示錯誤訊息
                        }

                        // 儲存圖片到指定目錄
                        var fileName = Path.GetFileName(product.Image.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UpImages", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await product.Image.CopyToAsync(stream);
                        }

                        // 設定圖片路徑到 Banner
                        product.ImagePath = fileName;

                        
                    }
                    // 將資料存入資料庫
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategory, "Id", "CategoryName", product.ProductCategoryId);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
