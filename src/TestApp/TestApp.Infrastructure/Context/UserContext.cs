using Microsoft.EntityFrameworkCore;
using TestApp.Core.Entities;

namespace TestApp.Infrastructure.Context;

internal sealed class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {

    }

    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.LastName)
                .IsUnique(false);

            entity.HasIndex(e => e.City)
                .IsUnique(false);

            entity.HasIndex(e => e.Country)
                .IsUnique(false);

            entity.HasIndex(e => e.DataCollectedDate)
                .IsUnique(false);
        });
    }
}