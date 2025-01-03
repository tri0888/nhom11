﻿namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        public int UserId { get; set; }

        [StringLength(50)]
        [DisplayName("Tên")]
        public string Name { get; set; }

        [StringLength(50)]
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        [StringLength(50)]
        [DisplayName("Số điện thoại")]
        public string Phone { get; set; }

        [StringLength(50)]
        [DisplayName("Tên đăng nhập")]
        public string Username { get; set; }

        [StringLength(32)]
        [DisplayName("Mật khẩu")]
        public string Password { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [StringLength(50)]
        [DisplayName("Nhóm người dùng")]
        public string GroupId { get; set; }

        [DisplayName("Trạng thái tài khoản")]
        public bool Status { get; set; }

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; }
        [ForeignKey("GroupId")]
        public virtual UserGroup UserGroup { get; set; }
    }
}
