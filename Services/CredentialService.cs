using Microsoft.EntityFrameworkCore;
using PasswordManager.Data;
using PasswordManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasswordManager.Services;

public class CredentialService {
    private readonly AppDbContext _db;

    public CredentialService(AppDbContext db) {
        _db = db;
        _db.Database.EnsureCreated(); // Automaticky vytvoří databázi pokud neexistuje
    }

    public async Task<List<Credential>> GetAllAsync() {
        return await _db.Credentials.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(Credential credential) {
        _db.Credentials.Add(credential);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        var credential = await _db.Credentials.FindAsync(id);
        if (credential != null) {
            _db.Credentials.Remove(credential);
            await _db.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(Credential credential) {
        _db.Credentials.Update(credential);
        await _db.SaveChangesAsync();
    }
}
