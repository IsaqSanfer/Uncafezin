using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UncafezinEntities;

namespace UncafezinDAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UncafezinEntities.Category> Category { get; set; }
    public DbSet<UncafezinEntities.Client> Client { get; set; }
    public DbSet<UncafezinEntities.Order> Order { get; set; }
    public DbSet<UncafezinEntities.OrderDetail> OrderDetail { get; set; }
    public DbSet<UncafezinEntities.Product> Product { get; set; }
    public DbSet<UncafezinEntities.User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderDetail>().HasKey(x => new { x.CodeOrder, x.CodeProduct });

        modelBuilder.Entity<OrderDetail>()
            .HasOne(x => x.Order)
            .WithMany(y => y.Products)
            .HasForeignKey(x => x.CodeOrder);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(x => x.Product)
            .WithMany(y => y.Orders)
            .HasForeignKey(x => x.CodeProduct);
    }
}
