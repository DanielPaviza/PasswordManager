using Microsoft.EntityFrameworkCore;
using PasswordManager.Data;
using PasswordManager.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace PasswordManager.Services;

public class CredentialService {
    private readonly AppDbContext _db;
    private readonly EncryptionService _encryptionService;

    public CredentialService(AppDbContext db, EncryptionService encryptionService) {
        _db = db;
        _encryptionService = encryptionService;
        _db.Database.EnsureCreated(); // Automaticky vytvoří databázi pokud neexistuje
    }

    public async Task<List<CredentialModel>> GetAllAsync() {
        return await _db.Credentials.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(CredentialModel credential) {

        var validationResult = await ValidateCredentialAsync(credential);
        if (validationResult != ValidationResult.Success) {
            throw new ValidationException("Invalid credential data.");
        }

        credential.Password = _encryptionService.EncryptCredentials(credential.Password);

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

    public async Task UpdateAsync(CredentialModel credential) {
        _db.Credentials.Update(credential);
        await _db.SaveChangesAsync();
    }

    public async Task<ValidationResult> ValidateCredentialAsync(CredentialModel credential) {

        if (string.IsNullOrWhiteSpace(credential.ServiceName))
            return new ValidationResult("Service name cannot be empty.", ["ServiceName"]);

        if (string.IsNullOrWhiteSpace(credential.Password))
            return new ValidationResult("Password cannot be empty.", ["Password"]);

        bool exists = await _db.Credentials.AnyAsync(c => c.Id == credential.Id);

        if (exists)
            return new ValidationResult("Credential with same ServiceName and Username already exists.", ["Credential"]);

        return ValidationResult.Success!;
    }
}
