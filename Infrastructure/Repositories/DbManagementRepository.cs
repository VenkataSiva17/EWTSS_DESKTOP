using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient;
using EWTSS_DESKTOP.Core.Interfaces;

namespace EWTSS_DESKTOP.Infrastructure.Repositories
{
    public class DbManagementRepository : IDbManagementRepository
    {
        private readonly string _connectionString;

        public DbManagementRepository(string server, string database, string user, string password)
        {
            _connectionString = $"Server={server};Database={database};Uid={user};Pwd={password};";
        }

        // ================= BACKUP =================
        public bool BackupDatabase(string outputFilePath, out string message)
        {
            message = string.Empty;

            try
            {
                using var conn = new MySqlConnection(_connectionString);
                conn.Open();

                StringBuilder sb = new StringBuilder();

                var tables = new List<string>();

                using (var tablesCmd = new MySqlCommand("SHOW TABLES;", conn))
                using (var reader = tablesCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }

                foreach (var table in tables)
                {
                    if (table.Equals("__EFMigrationsHistory", StringComparison.OrdinalIgnoreCase))
                        continue;

                    // CREATE TABLE
                    using (var createCmd = new MySqlCommand($"SHOW CREATE TABLE `{table}`;", conn))
                    using (var createReader = createCmd.ExecuteReader())
                    {
                        if (createReader.Read())
                        {
                            sb.AppendLine($"DROP TABLE IF EXISTS `{table}`;");
                            sb.AppendLine(createReader.GetString(1) + ";");
                            sb.AppendLine();
                        }
                    }

                    // INSERT DATA
                    using (var dataCmd = new MySqlCommand($"SELECT * FROM `{table}`;", conn))
                    using (var dataReader = dataCmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            StringBuilder insert = new StringBuilder();
                            insert.Append($"INSERT INTO `{table}` VALUES (");

                            for (int i = 0; i < dataReader.FieldCount; i++)
                            {
                                insert.Append(ConvertToSqlValue(dataReader.GetValue(i)));

                                if (i < dataReader.FieldCount - 1)
                                    insert.Append(", ");
                            }

                            insert.Append(");");
                            sb.AppendLine(insert.ToString());
                        }
                    }

                    sb.AppendLine();
                }

                File.WriteAllText(outputFilePath, sb.ToString(), Encoding.UTF8);

                message = "Backup completed successfully.";
                return true;
            }
            catch (Exception ex)
            {
                message = $"Backup failed: {ex.Message}";
                return false;
            }
        }

        // ================= IMPORT =================
        public bool ImportDatabase(string inputFilePath, out string message)
        {
            message = string.Empty;

            try
            {
                if (!File.Exists(inputFilePath))
                {
                    message = "Import file not found.";
                    return false;
                }

                string sql = File.ReadAllText(inputFilePath);

                if (string.IsNullOrWhiteSpace(sql))
                {
                    message = "Import file is empty.";
                    return false;
                }

                using var conn = new MySqlConnection(_connectionString);
                conn.Open();

                using var cmd = conn.CreateCommand();

                cmd.CommandText = "SET FOREIGN_KEY_CHECKS = 0;";
                cmd.ExecuteNonQuery();

                string[] commands = sql.Split(';', StringSplitOptions.RemoveEmptyEntries);

                foreach (var rawCommand in commands)
                {
                    string command = rawCommand.Trim();

                    if (string.IsNullOrWhiteSpace(command))
                        continue;

                    try
                    {
                        cmd.CommandText = command;
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
                        // Ignore only expected duplicate-style errors
                        if (!IsIgnorableImportError(ex.Message))
                        {
                            cmd.CommandText = "SET FOREIGN_KEY_CHECKS = 1;";
                            cmd.ExecuteNonQuery();

                            message = $"Import failed: {ex.Message}";
                            return false;
                        }
                    }
                }

                cmd.CommandText = "SET FOREIGN_KEY_CHECKS = 1;";
                cmd.ExecuteNonQuery();

                message = "Database import completed successfully.";
                return true;
            }
            catch (Exception ex)
            {
                message = $"Import failed: {ex.Message}";
                return false;
            }
        }

        // ================= PURGE =================
        public bool PurgeDatabase(string[] tables, out string message)
        {
            message = string.Empty;

            try
            {
                using var conn = new MySqlConnection(_connectionString);
                conn.Open();

                using var cmd = conn.CreateCommand();

                cmd.CommandText = "SET FOREIGN_KEY_CHECKS = 0;";
                cmd.ExecuteNonQuery();

                foreach (var table in tables)
                {
                    if (string.Equals(table, "__EFMigrationsHistory", StringComparison.OrdinalIgnoreCase))
                        continue;

                    // DELETE is safer than TRUNCATE when FK relationships exist
                    cmd.CommandText = $"DELETE FROM `{table}`;";
                    cmd.ExecuteNonQuery();
                }

                cmd.CommandText = "SET FOREIGN_KEY_CHECKS = 1;";
                cmd.ExecuteNonQuery();

                message = "Database purged successfully.";
                return true;
            }
            catch (Exception ex)
            {
                message = $"Purge failed: {ex.Message}";
                return false;
            }
        }

        private static string ConvertToSqlValue(object value)
        {
            if (value == null || value == DBNull.Value)
                return "NULL";

            return value switch
            {
                string s => $"'{EscapeSqlString(s)}'",
                char c => $"'{EscapeSqlString(c.ToString())}'",
                DateTime dt => $"'{dt:yyyy-MM-dd HH:mm:ss}'",
                bool b => b ? "1" : "0",
                byte[] bytes => $"0x{BitConverter.ToString(bytes).Replace("-", "")}",
                sbyte or byte or short or ushort or int or uint or long or ulong or
                float or double or decimal => Convert.ToString(value, CultureInfo.InvariantCulture) ?? "NULL",
                _ => $"'{EscapeSqlString(value.ToString() ?? string.Empty)}'"
            };
        }

        private static string EscapeSqlString(string value)
        {
            return value.Replace("\\", "\\\\").Replace("'", "''");
        }

        private static bool IsIgnorableImportError(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                return false;

            string error = errorMessage.ToLowerInvariant();

            return error.Contains("already exists") ||
                   error.Contains("duplicate") ||
                   error.Contains("__efmigrationshistory");
        }
    }
}