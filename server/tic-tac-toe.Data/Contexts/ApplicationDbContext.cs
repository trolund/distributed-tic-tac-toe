using Manufacturing.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Manufacturing.Data.Contexts;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<Order> Orders { get; set; }

    public DbSet<StateChangeHistory> StateChangeHistories { get; set; }
    // public DbSet<Worker> Workers { get; set; }
    // public DbSet<Supervisor> Supervisors { get; set; }
    // public DbSet<EquipmentOrderHistory> EquipmentOrderHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Equipment>()
            .HasMany(e => e.Orders)
            .WithOne(o => o.Equipment)
            .HasForeignKey(o => o.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete if Equipment is deleted

        // Configuring Equipment and StateChangeHistory relationships
        modelBuilder.Entity<Equipment>()
            .HasMany(e => e.StateChangeHistories)
            .WithOne(s => s.Equipment)
            .HasForeignKey(s => s.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete if Equipment is deleted

        // Configuring EquipmentOrderHistory relationships
        modelBuilder.Entity<EquipmentOrderHistory>()
            .HasOne(e => e.Equipment)
            .WithMany() // No navigation property on Equipment for this relationship
            .HasForeignKey(eh => eh.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete if Equipment is deleted

        modelBuilder.Entity<EquipmentOrderHistory>()
            .HasOne(o => o.Order)
            .WithMany() // No navigation property on Order for this relationship
            .HasForeignKey(eh => eh.OrderId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete if Order is deleted
    }

    public override int SaveChanges()
    {
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}