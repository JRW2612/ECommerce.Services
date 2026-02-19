using Microsoft.EntityFrameworkCore;
using Orders.Service.Entities;

namespace Orders.Service.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OutBoxMessage> OutBoxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OutBoxMessage>(builder =>
            {
                builder.HasKey(e => e.Id);
                builder.HasIndex(e => e.CorrelationId);//for faster lookups
                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.OccurredOn).IsRequired();
                builder.Property(e => e.Type).IsRequired().HasMaxLength(250);
                builder.Property(e => e.Content).IsRequired();
            });

            modelBuilder.Entity<Order>().
                Property(e => e.Status).
                HasConversion<string>();
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = "Admin";//change from User from IUserService
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = "Admin";//change from User from IUserService
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
