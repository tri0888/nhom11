namespace Models.EF
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Provider")]
    public partial class Provider
    {
        public Provider()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int ProviderId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        // Navigation property
        public virtual ICollection<Product> Products { get; set; }
    }
}
