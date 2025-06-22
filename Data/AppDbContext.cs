using Microsoft.EntityFrameworkCore;
using PasswordManager.Models;

namespace PasswordManager.Data;

public class AppDbContext : DbContext {

    public DbSet<Credential> Credentials => Set<Credential>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Credential>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ServiceName).IsRequired();
            entity.Property(e => e.Username).IsRequired();
            entity.Property(e => e.Password).IsRequired();
        });
    }
}
