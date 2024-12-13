namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        public int ProductId { get; set; }

        [StringLength(50)]
        [DisplayName("Tên sản phẩm")]
        public string Name { get; set; }

        [DisplayName("Mô tả")]
        public string Description { get; set; }

        [DisplayName("Giá")]
        public int? Price { get; set; }

        [DisplayName("Số lượng")]
        public int? Quantity { get; set; }

        [DisplayName("Mã nhà cung cấp")]
        public int? ProviderId { get; set; }

        [DisplayName("Mã danh mục sản phẩm")]
        public int? CateId { get; set; }

        [DisplayName("Ảnh sản phẩm")]
        public string Photo { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Ngày bắt đầu KM")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Ngày kết thúc khuyến mại")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Giảm giá (%)")]
        public int? Discount { get; set; }

        public bool IsHidden { get; set; }

        [NotMapped]
        public decimal? DiscountPrice { get; set; }

        // Navigation properties
        [ForeignKey("CateId")]
        public virtual Category Category { get; set; }

        [ForeignKey("ProviderId")]
        public virtual Provider Provider { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
