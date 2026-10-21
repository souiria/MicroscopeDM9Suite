using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace MicroscopeApp
{
    public static class DatabaseHelper
    {
        private static string dbFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InspectionLogs.db");
        private static string connectionString = $"Data Source={dbFilePath};Version=3;";

        public static void InitializeDatabase()
        {
            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = @"CREATE TABLE IF NOT EXISTS Inspections (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Timestamp TEXT,
                                SerialNumber TEXT,
                                ReferenceName TEXT,
                                Status TEXT,
                                ShotIndex INTEGER,
                                ImagePath TEXT,
                                ShiftName TEXT
                              );";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void SaveInspection(string serialNumber, string referenceName, string status, int shotIndex, string imagePath, string shiftName)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = @"INSERT INTO Inspections (Timestamp, SerialNumber, ReferenceName, Status, ShotIndex, ImagePath, ShiftName)
                               VALUES (@Timestamp, @SerialNumber, @ReferenceName, @Status, @ShotIndex, @ImagePath, @ShiftName)";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@SerialNumber", serialNumber);
                    command.Parameters.AddWithValue("@ReferenceName", referenceName);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@ShotIndex", shotIndex);
                    command.Parameters.AddWithValue("@ImagePath", imagePath);
                    command.Parameters.AddWithValue("@ShiftName", shiftName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static (int okCount, int nokCount) GetShiftCounts(string shiftName, DateTime shiftStartTime)
        {
            int ok = 0;
            int nok = 0;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = @"SELECT Status, COUNT(*) as Count
                               FROM Inspections
                               WHERE ShiftName = @ShiftName AND Timestamp >= @StartTime
                               GROUP BY Status";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ShiftName", shiftName);
                    command.Parameters.AddWithValue("@StartTime", shiftStartTime.ToString("yyyy-MM-dd HH:mm:ss"));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string status = reader["Status"].ToString();
                            int count = Convert.ToInt32(reader["Count"]);
                            if (status == "OK") ok = count;
                            else if (status == "NOK") nok = count;
                        }
                    }
                }
            }
            return (ok, nok);
        }

        public static DataTable SearchInspections(string filterText, DateTime? startDate = null, DateTime? endDate = null)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Inspections WHERE 1=1";

                if (!string.IsNullOrEmpty(filterText))
                {
                    sql += " AND (SerialNumber LIKE @filter OR ReferenceName LIKE @filter OR Status LIKE @filter)";
                }

                if (startDate.HasValue)
                {
                    sql += " AND Timestamp >= @startDate";
                }

                if (endDate.HasValue)
                {
                    sql += " AND Timestamp <= @endDate";
                }

                sql += " ORDER BY Id DESC";

                using (var adapter = new SQLiteDataAdapter(sql, connection))
                {
                    if (!string.IsNullOrEmpty(filterText))
                        adapter.SelectCommand.Parameters.AddWithValue("@filter", $"%{filterText}%");
                    if (startDate.HasValue)
                        adapter.SelectCommand.Parameters.AddWithValue("@startDate", startDate.Value.ToString("yyyy-MM-dd 00:00:00"));
                    if (endDate.HasValue)
                        adapter.SelectCommand.Parameters.AddWithValue("@endDate", endDate.Value.ToString("yyyy-MM-dd 23:59:59"));

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
