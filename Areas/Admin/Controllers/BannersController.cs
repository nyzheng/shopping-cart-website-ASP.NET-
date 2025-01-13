using System;
using System.Collections.Generic;
using System.Linq;
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
    public class BannersController : Controller
    {
        private readonly DBContext _context;

        public BannersController(DBContext context)
        {
            _context = context;
        }

        // GET: Admin/Banners
        public async Task<IActionResult> Index()
        {
            return View(await _context.Banners.ToListAsync());
        }

        // GET: Admin/Banners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // GET: Admin/Banners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Banners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Subject,Image,ImagePath,Summary,CreateDate")] Banner banner)
        {
            // 移除 RegularExpression 的驗證錯誤
            ModelState.Remove("Image");

            if (ModelState.IsValid)
            {
                // 手動驗證圖片格式
                if (banner.Image != null && banner.Image.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    var fileExtension = Path.GetExtension(banner.Image.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("Image", "僅允許上傳圖片格式 (jpg, jpeg, png, gif, bmp)。");
                        return View(banner); // 回到表單顯示錯誤訊息
                    }

                    // 儲存圖片到指定目錄
                    var fileName = Path.GetFileName(banner.Image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UpImages", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await banner.Image.CopyToAsync(stream);
                    }

                    // 設定圖片路徑到 Banner
                    banner.ImagePath =  fileName;

                    // 將資料存入資料庫
                    _context.Add(banner);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Image", "請選擇要上傳的圖片。");
                }
            }

            // 驗證失敗，回到表單並顯示錯誤訊息
            return View(banner);

        }

        // GET: Admin/Banners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }

        // POST: Admin/Banners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Banner banner)
        {
            if (id != banner.Id)
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
                    if (banner.Image != null && banner.Image.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                        var fileExtension = Path.GetExtension(banner.Image.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("Image", "僅允許上傳圖片格式 (jpg, jpeg, png, gif, bmp)。");
                            return View(banner); // 回到表單顯示錯誤訊息
                        }

                        // 儲存圖片到指定目錄
                        var fileName = Path.GetFileName(banner.Image.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UpImages", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await banner.Image.CopyToAsync(stream);
                        }

                        // 設定圖片路徑到 Banner
                        banner.ImagePath = fileName;

                        // 將資料存入資料庫
                        _context.Update(banner);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError("Image", "請選擇要上傳的圖片。");
                    }

                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerExists(banner.Id))
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
            return View(banner);
        }

        // GET: Admin/Banners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // POST: Admin/Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner != null)
            {
                _context.Banners.Remove(banner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BannerExists(int id)
        {
            return _context.Banners.Any(e => e.Id == id);
        }
    }
}
