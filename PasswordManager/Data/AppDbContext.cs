using Microsoft.EntityFrameworkCore;
using PasswordManager.Entities;

namespace PasswordManager.Data;

public class AppDbContext : DbContext {

    public DbSet<CredentialEntity> Credentials => Set<CredentialEntity>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        modelBuilder.Entity<CredentialEntity>(entity => {
            entity.HasKey(e => new { e.ServiceName, e.Username });
            entity.Ignore(e => e.Id);
            entity.Property(e => e.ServiceName).IsRequired();
            entity.Property(e => e.Password).IsRequired();
        });
    }
}
