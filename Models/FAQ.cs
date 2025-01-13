using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class FAQ
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "標題")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "內容")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
