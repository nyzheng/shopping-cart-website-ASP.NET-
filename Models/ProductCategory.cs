using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class ProductCategory
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "產品名稱")]
        public string CategoryName { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public ICollection<Product> Products { get; set; }=new List<Product>();
    }
}
