using Microsoft.EntityFrameworkCore;
using PasswordManager.Data;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManager.Services;

public class CredentialService : ICredentialService {
    private readonly AppDbContext _db;
    private readonly IEncryptionService _encryptionService;

    public CredentialService(AppDbContext db, IEncryptionService encryptionService) {
        _db = db;
        _encryptionService = encryptionService;
        _db.Database.EnsureCreated(); // Automaticky vytvoří databázi pokud neexistuje
    }

    public async Task<List<CredentialModel>> GetAllAsync() {
        return await _db.Credentials.AsNoTracking().Select(c => new CredentialModel(c)).ToListAsync();
    }

    public async Task AddAsync(CredentialModel credential) {

        var validationResult = await ValidateCredentialAsync(credential);
        if (validationResult.Count != 0) {
            // TODO LOG Add credential validation error: {validationResult}
            return;
        }

        credential.Password = _encryptionService.EncryptCredentials(credential.Password);

        _db.Credentials.Add(credential.ToEntity());
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        var credential = await _db.Credentials.FindAsync(id);

        if (credential == null) {
            // TODO LOG Credential delete validation not found with ID: {id}
            return;
        }

        _db.Credentials.Remove(credential);
        await _db.SaveChangesAsync();
        // TODO LOG Credential deleted with ID: {id}
    }

    public async Task UpdateAsync(CredentialModel credential) {

        var validationResult = await ValidateCredentialAsync(credential);
        if (validationResult.Count != 0) {
            // TODO LOG Update credential validation error: {validationResult}
            return;
        }

        _db.Credentials.Update(credential.ToEntity());
        await _db.SaveChangesAsync();
    }

    private async Task<List<ValidationResult>> ValidateCredentialAsync(CredentialModel credential) {

        var validationContext = new ValidationContext(credential);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(credential, validationContext, results, true);
        if (!isValid) return results;

        bool exists = await _db.Credentials.AnyAsync(c => c.Id == credential.Id);

        if (exists) {
            results.Add(new ValidationResult("Credential with same ServiceName and Username already exists.", [nameof(credential.ServiceName), nameof(credential.Username)]));
            return results;
        }

        return results;
    }
}
