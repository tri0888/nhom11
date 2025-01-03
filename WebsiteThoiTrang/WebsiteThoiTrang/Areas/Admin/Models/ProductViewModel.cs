using Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteNoiThat.Models
{
    public class ProductViewModel
    {
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Ảnh")]
        public string Photo { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Display(Name = "Danh mục")]
        public int? CateId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn nhà cung cấp")]
        [Display(Name = "Nhà cung cấp")]
        public int? ProviderId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        [Display(Name = "Giá")]
        [Range(-1, int.MaxValue, ErrorMessage = "Giá không hợp lệ")]
        public int? Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Display(Name = "Số lượng")]
        [Range(-1, int.MaxValue, ErrorMessage = "Số lượng không hợp lệ")]
        public int? Quantity { get; set; }

        [Display(Name = "Giảm giá")]
        [Range(-1, int.MaxValue, ErrorMessage = "Giảm giá không hợp lệ")]
        public int? Discount { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public DateTime? EndDate { get; set; }

        public bool IsHidden { get; set; }

        [Display(Name = "Tên danh mục")]
        public string CateName { get; set; }

        [Display(Name = "Tên nhà cung cấp")]
        public string ProviderName { get; set; }

        public int? ParentCateId { get; set; }
    }
}