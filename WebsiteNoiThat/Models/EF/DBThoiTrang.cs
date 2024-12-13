namespace Models.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DBThoiTrang : DbContext
    {
        public DBThoiTrang() : base("name=DBThoiTrang")
        {
        }

        // Khai báo các DbSet tương ứng với các bảng trong CSDL
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Credential> Credentials { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Quan hệ Card - User (1-1)
            modelBuilder.Entity<Card>()
                .HasRequired(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .WillCascadeOnDelete(false);

            // Quan hệ Order - User (n-1)
            modelBuilder.Entity<Order>()
                .HasOptional(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .WillCascadeOnDelete(false);

            // Quan hệ Order - Status (n-1)
            modelBuilder.Entity<Order>()
                .HasRequired(o => o.Status)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.StatusId)
                .WillCascadeOnDelete(false);

            // Quan hệ Product - Category
            modelBuilder.Entity<Product>()
                .HasOptional(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CateId)
                .WillCascadeOnDelete(false);

            // Quan hệ Product - Provider
            modelBuilder.Entity<Product>()
                .HasOptional(p => p.Provider)
                .WithMany(pr => pr.Products)
                .HasForeignKey(p => p.ProviderId)
                .WillCascadeOnDelete(false);

            // Quan hệ User - UserGroup
            modelBuilder.Entity<User>()
                .HasOptional(u => u.UserGroup)
                .WithMany(g => g.Users)
                .HasForeignKey(u => u.GroupId)
                .WillCascadeOnDelete(false);

            // Quan hệ Credential - UserGroup
            modelBuilder.Entity<Credential>()
                .HasOptional(c => c.UserGroup)
                .WithMany(g => g.Credentials)
                .HasForeignKey(c => c.UserGroupId)
                .WillCascadeOnDelete(false);

            // Quan hệ Credential - Role
            modelBuilder.Entity<Credential>()
                .HasOptional(c => c.Role)
                .WithMany(r => r.Credentials)
                .HasForeignKey(c => c.RoleId)
                .WillCascadeOnDelete(false);

            // Quan hệ OrderDetail - Order
            modelBuilder.Entity<OrderDetail>()
                .HasRequired(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .WillCascadeOnDelete(false);

            // Quan hệ OrderDetail - Product
            modelBuilder.Entity<OrderDetail>()
                .HasRequired(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .WillCascadeOnDelete(false);
        }
    }
}
