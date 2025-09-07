using PasswordManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasswordManager.Interfaces;

public interface ICredentialService {
    Task<List<CredentialModel>> GetAllAsync();
    Task AddAsync(CredentialModel credential);
    Task UpdateAsync(CredentialModel credential);
    Task DeleteAsync(int id);
}
