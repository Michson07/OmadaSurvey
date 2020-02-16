using Microsoft.Data.SqlClient;
using Omada.Areas.Identity.Data;
using Omada.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Omada.ManageTeamsAndSurveys
{
    public class LeaderTeam
    {
        public OmadaTeam GetTeam(string leaderId)
        {
            OmadaTeam team = new OmadaTeam();
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using(SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT dbo.Teams.Id, dbo.Teams.Name
                                            FROM dbo.Users_Teams
                                            JOIN dbo.Teams 
                                            ON dbo.Users_Teams.TeamId = dbo.Teams.Id
                                            AND dbo.Users_Teams.UserId = @leaderId
                                            AND dbo.Users_Teams.IsLeader = 1";
                    command.Parameters.AddWithValue("@leaderId", leaderId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            team.Id = reader.GetInt32(0);
                            team.Name = reader.GetString(1);
                        }
                    }
                }
            }
            return team;
        }
    }
}
