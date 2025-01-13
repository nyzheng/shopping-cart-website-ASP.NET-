using Microsoft.AspNetCore.Mvc;
using Shopping.Data;
using Shopping.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Shopping.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly DBContext _context;
        public ShoppingCartController(DBContext dbContext)
        {
            _context = dbContext;

        }
        [HttpPost]
        public IActionResult Index(int? Id, int Quantity)
        {
            // 檢查產品ID是否為空
            if (Id == null)
            {
                return NotFound();
            }

            List<OrderDetail> cart;
            // 從Session中獲取購物車數據
            var cartJson = HttpContext.Session.GetString("cart");

            // 如果購物車為空，創建新的購物車
            if (string.IsNullOrEmpty(cartJson))
            {
                cart = new List<OrderDetail>();
            }
            else
            {
                // 反序列化現有的購物車數據
                cart = JsonSerializer.Deserialize<List<OrderDetail>>(cartJson);
            }

            // 檢查購物車中是否已存在該產品
            OrderDetail existingItem = cart.FirstOrDefault(x => x.ProductId == Id);
            if (existingItem != null)
            {
                // 如果產品已存在，增加數量
                existingItem.Quantity += Quantity;
                // 更新Session中的購物車數據
                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));
                return RedirectToAction("ShoppingCart");
            }



            // 從數據庫獲取產品信息
            Product product = _context.Product.FirstOrDefault(x => x.Id == Id);
            if (product == null)
            {
                return NotFound();
            }

            // 創建新的訂單詳情

            //OrderDetail orderDetail = new OrderDetail
            //{
            //    ProductId = product.Id,
            //    ProductName = product.ProductName,
            //    ImagePath = product.ImagePath,
            //    Price = product.Price,
            //    Quantity = Quantity
            //};  以上為簡寫

            // 創建新的訂單詳情
            OrderDetail newOrderDetail = new OrderDetail();
            newOrderDetail.ProductId = product.Id;
            newOrderDetail.ProductName = product.ProductName;
            newOrderDetail.ImagePath = product.ImagePath;
            newOrderDetail.Price = product.Price;
            newOrderDetail.Quantity = Quantity;
            
            // 將新產品添加到購物車
            cart.Add(newOrderDetail);
            // 更新Session中的購物車數據
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));

            // 重定向到購物車頁面
            return RedirectToAction("ShoppingCart");
        }
        public IActionResult ShoppingCart()
        {
            List<OrderDetail> cart;
            // 從Session中獲取購物車數據
            var cartJson = HttpContext.Session.GetString("cart");

            // 如果購物車為空，創建新的購物車
            if (string.IsNullOrEmpty(cartJson))
            {
                cart = new List<OrderDetail>();
            }
            else
            {
                // 反序列化現有的購物車數據
                cart = JsonSerializer.Deserialize<List<OrderDetail>>(cartJson);
            }

            return View(cart);
        }
        [HttpPost]
        public IActionResult ShoppingCart(List<OrderDetail> orderDetails)
        {
            return RedirectToAction("checkout");
        }


        [HttpPost]
        public IActionResult Update(int Id,int Quantity)
        {
            List<OrderDetail> cart;
            // 從Session中獲取購物車數據
            var cartJson = HttpContext.Session.GetString("cart");

            // 如果購物車為空，創建新的購物車
            if (string.IsNullOrEmpty(cartJson))
            {
                cart = new List<OrderDetail>();
            }
            else
            {
                // 反序列化現有的購物車數據
                cart = JsonSerializer.Deserialize<List<OrderDetail>>(cartJson);
            }
            OrderDetail existingItem = cart.FirstOrDefault(x => x.ProductId == Id);
            if (existingItem != null)
            {
                // 如果產品已存在，增加數量
                existingItem.Quantity = Quantity;
                // 更新Session中的購物車數據
                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));
            }
            return RedirectToAction("ShoppingCart");
        }
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            List<OrderDetail> cart;
            // 從Session中獲取購物車數據
            var cartJson = HttpContext.Session.GetString("cart");

            // 如果購物車為空，創建新的購物車
            if (string.IsNullOrEmpty(cartJson))
            {
                cart = new List<OrderDetail>();
            }
            else
            {
                // 反序列化現有的購物車數據
                cart = JsonSerializer.Deserialize<List<OrderDetail>>(cartJson);
            }
            OrderDetail existingItem = cart.FirstOrDefault(x => x.ProductId == Id);
            if (existingItem != null)
            {
                // 如果產品已存在，移除
                cart.Remove(existingItem);
                // 更新Session中的購物車數據
                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));
            }
            return RedirectToAction("ShoppingCart");
        }

        public IActionResult checkout()
        {
            return View();
        }
        [HttpPost]
        public IActionResult checkout(Order order)
        {
            if (ModelState.IsValid) //後端驗證
            {
                // 從Session中獲取購物車數據
                var cartJson = HttpContext.Session.GetString("cart");
                if (string.IsNullOrEmpty(cartJson))
                {
                    return RedirectToAction("ShoppingCart");
                }

                List<OrderDetail> cart = JsonSerializer.Deserialize<List<OrderDetail>>(cartJson);

                // 創建新訂單
                Order newOrder = new Order
                {
                    Name = order.Name,
                    Email = order.Email,
                    Mobile = order.Mobile,
                    Address = order.Address
                };

                // 將購物車中的產品添加到訂單詳情
                foreach (var item in cart)
                {
                    newOrder.OrderDetails.Add(item);
                }

                // 將訂單保存到數據庫
                _context.Order.Add(newOrder);
                _context.SaveChanges();

                // 清空購物車
                HttpContext.Session.Remove("cart");

                //導到完成頁面 並顯示訂單編號
                return RedirectToAction("complete",new {id=newOrder.Id});
            }

            return View(order);
        }

        public IActionResult complete(int? Id)
        {
            if (!Id.HasValue)
            {
                return NotFound();
            }

            Order order = _context.Order.Include(x => x.OrderDetails).FirstOrDefault(x => x.Id == Id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
