using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class Product
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "產品名稱")]
        public string ProductName { get; set; }


        [Display(Name = "圖片")]
        [DataType(DataType.ImageUrl)]
        [RegularExpression(@".*\.(jpg|jpeg|png|gif|bmp)$", ErrorMessage = "僅允許上傳圖片格式 (jpg, jpeg, png, gif, bmp)。")]
        [NotMapped]
        public IFormFile? Image { get; set; }

    
        [MaxLength(50)]
        [Display(Name = "圖片路徑")]
        public string? ImagePath { get; set; } // 這會儲存在資料庫中

        [Display(Name = "描述")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "原價")]
        public int? OriginalPrice { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "價格")]
        public int Price { get; set; }

        [Display(Name = "是否上架")]
        public ProductStatus Status { get; set; } = ProductStatus.上架;

        [Display(Name = "是否新品")]
        public bool IsNew { get; set; } = true;

        [Display(Name = "是否熱銷")]
        public bool IsBestSeller { get; set; } = false;


        [Required(ErrorMessage = "{0}必填")]
        public int ProductCategoryId { get; set; }

        [Display(Name = "產品類別")]
        [ForeignKey("ProductCategoryId")]
        public ProductCategory? ProductCategory { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

		public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        // 判斷是否為可用狀態
        public bool IsAvailable => Status == ProductStatus.Available;
    }

    public enum ProductStatus
    {
        上架 = 1,
        下架 = 0,
        Available = 2
    }
}
