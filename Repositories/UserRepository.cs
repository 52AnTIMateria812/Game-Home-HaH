using Npgsql;
using System;

public class UserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Создание пользователя
    public void Create(User user)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new NpgsqlCommand(
                "INSERT INTO Account (username, email) VALUES (@u, @e);" +
                "INSERT INTO \"User\" (account_id, password) VALUES (currval(pg_get_serial_sequence('account', 'id')), @p)", 
                conn);
            cmd.Parameters.AddWithValue("u", user.Username);
            cmd.Parameters.AddWithValue("e", user.Email);
            cmd.Parameters.AddWithValue("p", user.Password);
            cmd.ExecuteNonQuery();
        }
    }

    // Получение пользователя по ID
    public User GetById(int id)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new NpgsqlCommand(
                "SELECT u.id, a.username, a.email, u.password " +
                "FROM \"User\" u " +
                "JOIN Account a ON u.account_id = a.id " +
                "WHERE u.id = @id", 
                conn);
            cmd.Parameters.AddWithValue("id", id);
            using (var reader = cmd.ExecuteReader())
            {
                return reader.Read() ? new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Email = reader.GetString(2),
                    Password = reader.GetString(3)
                } : null;
            }
        }
    }
} 