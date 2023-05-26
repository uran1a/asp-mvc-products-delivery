using Microsoft.EntityFrameworkCore;

namespace ProductsDelivery.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Person> People { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Collector> Collectors { get; set; } = null!;
        public DbSet<Delivery> Deliveries { get; set; } = null!;
        public DbSet<Manager> Managers { get; set; } = null!;
        public DbSet<Provider> Providers { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Application> Applications { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Provider)
                .WithMany(pr => pr.Products)
                .HasForeignKey(p => p.ProviderId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Order>()
               .HasOne(o => o.Collector)
               .WithMany(u => u.Orders)
               .HasForeignKey(o => o.CollectorId);

            modelBuilder.Entity<Order>()
               .HasOne(o => o.Delivery)
               .WithMany(u => u.Orders)
               .HasForeignKey(o => o.DeliveryId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(p => p.OrderId);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Product)
                .WithMany(p => p.Applications)
                .HasForeignKey(a => a.ProductId);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Product)
                .WithMany(p => p.Applications)
                .HasForeignKey(a => a.ProviderId);
        }
    }
}
