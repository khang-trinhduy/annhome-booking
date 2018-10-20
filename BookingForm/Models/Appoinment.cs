using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookingForm.Models
{
    public class Appoinment
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Họ & tên*", Prompt = "Nguyễn Văn Minh")]
        public string Customer { get; set; }
        [Required]
        [Display(Name = "Giới tính*")]
        public string Gender { get; set; }
        [Required]
        [Display(Name = "Nơi ở hiện tại*")]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Số điện thoại*", Prompt = "095444360")]
        [RegularExpression(@"^((\(?([0-9]{3})\)?)|(\(?([0-9]{4})\)?)|(\(?([0-9]{2})\)?))[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Sai số điện thoại")]
        public string Phone { get; set; }
        [RegularExpression(@"^([a-z0-9\.\-]+@[a-z\-]+\.[\w]+)$", ErrorMessage = "Email không tồn tại")]
        [Display(Name = "Email", Prompt = "example@mail.com")]
        public string Email { get; set; }
        [Display(Name = "Nghề nghiệp")]
        public string Job { get; set; }
        [Display(Name = "Nơi làm việc")]
        public string WorkPlace { get; set; }
        
        //[Required(ErrorMessage = "Số CMND không thể bỏ trống")]
        [Display(Name = "Số CMND/Căn cước/Hộ chiếu*")]
        [RegularExpression(@"^(([0-9]{9})|([0-9]{12}))$", ErrorMessage = "Sai số CMND")]
        public string Cmnd { get; set; }
        [Display(Name = "Ngày cấp*")]
        [DataType(DataType.Date)]
        public DateTime Day { get; set; }
        [Display(Name = "Nơi cấp*")]
        public string Place { get; set; }
        [Display(Name = "Số tiền giữ chỗ đặt mua*")]
        public double Money { get; set; }
        [Required]
        [Display(Name = "Mục đích*")]
        public string Purpose { get; set; }
        [Display(Name = "Yêu cầu cụ thể")]
        public string Requires { get; set; }
        [Display(Name = "Số tiền cần vay (nếu có)")]
        public double Price { get; set; }
        [Display(Name = "Đặc điểm KH")]
        public string Details { get; set; }
        [Required]
        [Display(Name = "Hình thức thanh toán*")]
        public string DType { get; set; }
        [Required]
        [Display(Name = "Tiền mặt*")]
        public double Cash { get; set; }
        [Required]
        [Display(Name = "SL Căn 1pn")]
        public int NCH1 { get; set; }
        [Required]
        [Display(Name = "SL Căn 2pn")]
        public int NCH2 { get; set; }
        [Required]
        [Display(Name = "SL Căn 3pn")]
        public int NCH3 { get; set; }
        [Required]
        [Display(Name = "SL biệt thự")]
        public int NMS { get; set; }
        [Required]
        [Display(Name = "SL nhà phố")]
        public int NSH { get; set; }
        [Required]
        [Display(Name = "SL shophouse*")]
        public int NSHH { get; set; }
        [Required]
        [Display(Name = "Hộ khẩu thường trú*")]
        public string HKTT { get; set; }
        [Required]
        [RegularExpression(@"^([a-z0-9\.\-]+@[a-z\-]+\.[\w]+)$", ErrorMessage = "Email không tồn tại")]
        [Display(Name = "Email sale*")]
        public string sale { get; set; }
        [Required]
        [Display(Name = "Password*")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public Appoinment() { }
    }
}
