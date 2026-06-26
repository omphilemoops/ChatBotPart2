using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChatBotPart2
{
    //database manager class to handle database operations
    public class DatabaseManager
    {
        private string connectionString =
            @"Server=(localdb)\MSSQLLocalDB;
              Database=ChatBotPart2;
              Trusted_Connection=True;";

        public void AddTask(CyberTask task)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql =
                    "INSERT INTO Tasks (Title, Description, ReminderDate, Completed) " +
                    "VALUES (@Title, @Description, @ReminderDate, @Completed)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Title", task.Title);
                cmd.Parameters.AddWithValue("@Description", task.Description);
                cmd.Parameters.AddWithValue("@ReminderDate", task.ReminderDate);
                cmd.Parameters.AddWithValue("@Completed", task.Completed);

                cmd.ExecuteNonQuery();
            }
        }
        public void CompleteTask(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql =
                    @"UPDATE Tasks
              SET Completed = 1
              WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteTask(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql =
                    @"DELETE FROM Tasks
              WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }
        }

        public List<CyberTask> GetTasks()
        {
            List<CyberTask> tasks = new List<CyberTask>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM Tasks";

                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tasks.Add(new CyberTask
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Title = reader["Title"].ToString(),
                        Description = reader["Description"].ToString(),
                        ReminderDate = Convert.ToDateTime(reader["ReminderDate"]),
                        Completed = Convert.ToBoolean(reader["Completed"])
                    });
                }
            }

            return tasks;
        }
    }
}