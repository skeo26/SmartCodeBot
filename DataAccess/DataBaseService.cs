
using System.Data.SqlClient;
using SmartCodeBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCodeBot.DataAccess
{
    public class DataBaseService : IDBService
    {
        private readonly string _connectionString;

        public DataBaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> GetOrCreateUserAsync(long chatId, string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Проверка наличия пользователя по ChatId
                var checkCmd = new SqlCommand("SELECT Id FROM Users WHERE ChatId = @ChatId", connection);
                checkCmd.Parameters.AddWithValue("@ChatId", chatId);

                var userIdObj = await checkCmd.ExecuteScalarAsync();

                // Если пользователь не найден (DBNull.Value), создаём нового пользователя
                if (userIdObj == DBNull.Value || userIdObj == null)
                {
                    // Создание пользователя (без указания Id, чтобы не вмешиваться в автоинкремент)
                    var insertCmd = new SqlCommand("INSERT INTO Users (ChatId, Username) VALUES (@ChatId, @Username)", connection);
                    insertCmd.Parameters.AddWithValue("@ChatId", chatId);
                    insertCmd.Parameters.AddWithValue("@Username", username ?? (object)DBNull.Value);

                    await insertCmd.ExecuteNonQueryAsync();

                    // Получаем Id нового пользователя
                    var getUserIdCmd = new SqlCommand("SELECT SCOPE_IDENTITY()", connection);
                    var newUserId = await getUserIdCmd.ExecuteScalarAsync();

                    return Convert.ToInt32(newUserId); // Возвращаем Id нового пользователя
                }

                // Если пользователь уже есть, возвращаем его Id
                return Convert.ToInt32(userIdObj);
            }
        }


        public async Task<UserSessionData> GetUserSessionAsync(long userId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var cmd = new SqlCommand(@"
            SELECT language, category, topic, is_taking_test, test
            FROM user_sessions
            WHERE user_id = @UserId", connection);
            cmd.Parameters.AddWithValue("@UserId", userId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new UserSessionData
                {
                    Language = reader["language"] as string,
                    Category = reader["category"] as string,
                    Topic = reader["topic"] as string,
                    IsTakingTest = reader["is_taking_test"] != DBNull.Value && (bool)reader["is_taking_test"],
                    Test = reader["test"] as string
                };
            }

            return null;
        }

        public async Task SaveUserSessionAsync(long chatId, MenuState session)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            // 1. Получаем id пользователя по chatId
            var getUserIdCmd = new SqlCommand("SELECT id FROM users WHERE chatId = @ChatId", connection);
            getUserIdCmd.Parameters.AddWithValue("@ChatId", chatId);

            var userIdObj = await getUserIdCmd.ExecuteScalarAsync();
            if (userIdObj == null)
            {
                Console.WriteLine($"Ошибка: пользователь с chatId={chatId} не найден в таблице users.");
                return; 
            }

            int userId = (int)userIdObj;

            var checkCmd = new SqlCommand("SELECT COUNT(*) FROM user_sessions WHERE user_id = @UserId", connection);
            checkCmd.Parameters.AddWithValue("@UserId", userId);
            var count = (int)await checkCmd.ExecuteScalarAsync();

            SqlCommand cmd;
            if (count == 0)
            {
                cmd = new SqlCommand(@"
                            INSERT INTO user_sessions (user_id, language, category, topic, is_taking_test, test)
                            VALUES (@UserId, @Language, @Category, @Topic, @IsTakingTest, @Test)", connection);
            }
            else
            {
                cmd = new SqlCommand(@"
                            UPDATE user_sessions
                            SET language = @Language,
                                category = @Category,
                                topic = @Topic,
                                is_taking_test = @IsTakingTest,
                                test = @Test
                            WHERE user_id = @UserId", connection);
            }

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@Language", (object?)session.Language ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Category", (object?)session.Category ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Topic", (object?)session.CurrentTopic ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsTakingTest", session.IsTakingTest);
            cmd.Parameters.AddWithValue("@Test", (object?)session.Test ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task SaveFeedbackAsync(long chatId, string message)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("UPDATE users SET feedback = @message WHERE chatId = @chatId", connection);
            command.Parameters.AddWithValue("@chatId", chatId);
            command.Parameters.AddWithValue("@message", message);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

    }
}


