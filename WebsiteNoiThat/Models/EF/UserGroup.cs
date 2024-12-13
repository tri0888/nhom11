namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserGroups")]
    public partial class UserGroup
    {
        public UserGroup()
        {
            Users = new HashSet<User>();
            Credentials = new HashSet<Credential>();
        }

        [Key]
        [StringLength(50)]
        [Display(Name = "Mã nhóm người dùng")]
        public string GroupId { get; set; }

        [StringLength(50)]
        [Display(Name = "Tên mô tả")]
        public string Name { get; set; }

        // Navigation properties
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Credential> Credentials { get; set; }
    }
}
