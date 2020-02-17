using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omada.ManageTeamsAndSurveys
{
    public class AddUserToTeam
    {
        public void AddUser(string userId, int teamId, int isLeader)
        {
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO dbo.Users_Teams VALUES(@UserId, @TeamId, @IsLeader)";
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@TeamId", teamId);
                    command.Parameters.AddWithValue("@IsLeader", isLeader);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
