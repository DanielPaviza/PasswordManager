using Microsoft.Data.Sqlite;
using PasswordManager.Models;
using System.Collections.Generic;

namespace PasswordManager.Services;

public class DatabaseService {
    private const string DbFile = "vault.db";

    private SqliteConnection _connection;

    public DatabaseService() {
        _connection = new SqliteConnection($"Data Source={DbFile}");
        _connection.Open();
        InitializeDatabase();
    }

    private void InitializeDatabase() {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Credentials (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ServiceName TEXT NOT NULL,
                Username TEXT NOT NULL,
                Password TEXT NOT NULL,
                Note TEXT
            )";
        cmd.ExecuteNonQuery();
    }

    public List<Credential> GetAll() {
        var list = new List<Credential>();
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT Id, ServiceName, Username, Password, Note FROM Credentials";
        using var reader = cmd.ExecuteReader();

        while (reader.Read()) {
            list.Add(new Credential {
                Id = reader.GetInt32(0),
                ServiceName = reader.GetString(1),
                Username = reader.GetString(2),
                Password = reader.GetString(3),
                Note = reader.IsDBNull(4) ? "" : reader.GetString(4)
            });
        }
        return list;
    }

    public void Add(Credential credential) {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "INSERT INTO Credentials (ServiceName, Username, Password, Note) VALUES (@serviceName, @user, @password, @note)";
        cmd.Parameters.AddWithValue("@serviceName", credential.ServiceName);
        cmd.Parameters.AddWithValue("@user", credential.Username);
        cmd.Parameters.AddWithValue("@password", credential.Password);
        cmd.Parameters.AddWithValue("@note", credential.Note);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id) {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "DELETE FROM Credentials WHERE Id = @id";
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }

    public void Update(Credential credential) {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            UPDATE Credentials SET
                ServiceName = @serviceName,
                Username = @user,
                Password = @password,
                Note = @note
            WHERE Id = @id";
        cmd.Parameters.AddWithValue("@serviceName", credential.ServiceName);
        cmd.Parameters.AddWithValue("@user", credential.Username);
        cmd.Parameters.AddWithValue("@password", credential.Password);
        cmd.Parameters.AddWithValue("@note", credential.Note);
        cmd.Parameters.AddWithValue("@id", credential.Id);
        cmd.ExecuteNonQuery();
    }
}
