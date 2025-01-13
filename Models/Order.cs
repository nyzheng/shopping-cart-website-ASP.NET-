using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class Order
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [EmailAddress(ErrorMessage = "{0} 格式錯誤")]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "手機號碼")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(200)]
        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "訂單日期")]
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Display(Name = "狀態")]
        public OrderStatus Status { get; set; } = OrderStatus.新訂單;

        [Display(Name = "總金額")]
        public int TotalAmount => OrderDetails.Sum(x => x.Subtotal); 

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
    public enum OrderStatus
    {
        新訂單,
        處理中,
        出貨中,
        已送達,
        已取消,
        已退貨
    }
}
