using Microsoft.EntityFrameworkCore;
using PasswordManager.Models;

namespace PasswordManager.Data;

public class AppDbContext : DbContext {

    public DbSet<CredentialModel> Credentials => Set<CredentialModel>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        modelBuilder.Entity<CredentialModel>(entity => {
            entity.HasKey(e => new { e.ServiceName, e.Username });
            entity.Property(e => e.ServiceName).IsRequired();
            entity.Property(e => e.Password).IsRequired();
        });
    }
}
