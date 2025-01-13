using System.ComponentModel.DataAnnotations;

namespace Shopping.Areas.Admin.Models.ViewMembers
{
    public class ViewLogin
    {
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "帳號")]
        public string Account { get; set; }


        [Required(ErrorMessage = "{0}必填")]
        [StringLength(100, ErrorMessage = "{0} 長度至少必須為 {2} 個字元。", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }
    }
}
