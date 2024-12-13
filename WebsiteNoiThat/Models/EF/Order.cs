namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Order")]
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [DisplayName("Mã hoá đơn")]
        public int OrderId { get; set; }

        [StringLength(50)]
        [DisplayName("Tên khách hàng")]
        public string ShipName { get; set; }

        [DisplayName("Mã khách hàng")]
        public int? UserId { get; set; }

        [DisplayName("SĐT Người nhận")]
        public string ShipPhone { get; set; }

        [DisplayName("Email người nhận")]
        public string ShipEmail { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Ngày cập nhật")]
        public DateTime? UpdateDate { get; set; }

        [DisplayName("Địa chỉ nhận hàng")]
        public string ShipAddress { get; set; }

        [Required]
        [DisplayName("Trạng thái đơn hàng")]
        public int StatusId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
