using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class OrderDetail
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "訂單編號")]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        [Display(Name = "訂單")]
        public Order? Order { get; set; }

        [Display(Name = "產品編號")]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [Display(Name = "產品")]
        public Product Product { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "產品名稱")]
        public string ProductName { get; set; }

        [MaxLength(50)]
        [Display(Name = "圖片路徑")]
        public string? ImagePath { get; set; } // 這會儲存在資料庫中

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "價格")]
        public int Price { get; set; }= 0;

        [Display(Name = "數量")]
        public int Quantity { get; set; } = 1;

        [Display(Name = "小計")]
        public int Subtotal => Price * Quantity;
    }
}
