using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class Member
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "帳號")]
        public string Account { get; set; }


        [Required(ErrorMessage = "{0}必填")]
        [StringLength(100, ErrorMessage = "{0} 長度至少必須為 {2} 個字元。", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; } = string.Empty;

		[NotMapped]
		[Required(ErrorMessage = "{0}必填")]
		[StringLength(100, ErrorMessage = "{0} 長度至少必須為 {2} 個字元。", MinimumLength = 4)]
		[DataType(DataType.Password)]
		[Display(Name = "確認密碼")]
		[Compare("Password", ErrorMessage = "密碼與確認密碼不相符")]
		public string ConfirmPassword { get; set; }


		[Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [EmailAddress(ErrorMessage = "{0} 格式錯誤")]
        [MaxLength(200)]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [Display(Name = "建立日期")]
		public DateTime CreateDate { get; set; }= DateTime.Now;


	}
}
