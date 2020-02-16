using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omada.ManageTeamsAndSurveys
{
    public class AddUserToTeam
    {
        private int CountUserTeam()
        {
            int count = 0;
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT COUNT(*) FROM dbo.Users_Teams";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }
                    }
                }
            }
            return count;
        }
        public void AddUser(string userId, int teamId, int isLeader)
        {
            int count = CountUserTeam() + 1;
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO dbo.Users_Teams VALUES(@Id, @UserId, @TeamId, @IsLeader)";
                    command.Parameters.AddWithValue("@Id", count);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@TeamId", teamId);
                    command.Parameters.AddWithValue("@IsLeader", isLeader);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
