namespace Models.EF
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Role")]
    public partial class Role
    {
        public Role()
        {
            Credentials = new HashSet<Credential>();
        }

        [Key]
        [StringLength(50)]
        [Display(Name = "Mã chức năng")]
        public string RoleId { get; set; }

        [StringLength(50)]
        [Display(Name = "Mô tả")]
        public string Name { get; set; }

        // Navigation property
        public virtual ICollection<Credential> Credentials { get; set; }
    }
}