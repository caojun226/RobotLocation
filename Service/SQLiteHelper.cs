using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotLocation.Service
{
    class SQLiteHelper : IDisposable
    {
        private SQLiteConnection _connection;

        public SQLiteHelper(string dbPath)
        {
            _connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            _connection.Open();
        }

        public int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            using (var cmd = new SQLiteCommand(sql, _connection))
            {
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        public DataTable ExecuteQuery(string sql, params SQLiteParameter[] parameters)
        {
            var dt = new DataTable();
            using (var cmd = new SQLiteCommand(sql, _connection))
            {
                cmd.Parameters.AddRange(parameters);
                using (var reader = cmd.ExecuteReader()) {
                    dt.Load(reader);
                }
                return dt;
            }
              
        }

        public object ExecuteScalar(string sql, params SQLiteParameter[] parameters)
        {
            using (var cmd = new SQLiteCommand(sql, _connection))
            {
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }

        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
