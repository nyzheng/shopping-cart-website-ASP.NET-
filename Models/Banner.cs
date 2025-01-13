using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class Banner
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "標題")]
        public string Subject { get; set; }


        
        [Display(Name = "圖片")]
        [DataType(DataType.ImageUrl)]
        [RegularExpression(@".*\.(jpg|jpeg|png|gif|bmp)$", ErrorMessage = "僅允許上傳圖片格式 (jpg, jpeg, png, gif, bmp)。")]
        [NotMapped]
        public IFormFile? Image { get; set; }

        [MaxLength(50)]
        [Display(Name = "圖片路徑")]
        public string? ImagePath { get; set; } // 這會儲存在資料庫中

        [Display(Name = "摘要")]
        [MaxLength(1000)]
        [DataType(DataType.MultilineText)]
        public string? Summary { get; set; }


        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
