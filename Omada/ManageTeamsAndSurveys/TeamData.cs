using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Omada.Areas.Identity.Data;

namespace Omada.ManageTeamsAndSurveys
{
    public class TeamData : ITeamData
    {
        public List<OmadaTeam> GetAllTeams()
        {
            List<OmadaTeam> teams = new List<OmadaTeam>();
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT *
                                            FROM dbo.Teams";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OmadaTeam team = new OmadaTeam();
                            team.Id = reader.GetInt32(0);
                            team.Name = reader.GetString(1);
                            teams.Add(team);
                        }
                    }
                }
                return teams;
            }
        }
        public OmadaTeam Add(OmadaTeam team)
        {
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO dbo.Teams 
                                            VALUES(@Id, @Name)";
                    command.Parameters.AddWithValue("@Id", team.Id);
                    command.Parameters.AddWithValue("@Name", team.Name);
                    command.ExecuteNonQuery();
                }
            }
            return team;
        }

        public void Delete(int teamId)
        {
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"DELETE 
                                            FROM dbo.Teams 
                                            WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", teamId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public OmadaTeam GetTeamById(int teamId)
        {
            OmadaTeam team = new OmadaTeam();
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT *
                                            FROM dbo.Teams
                                            WHERE Id = @teamId";
                    command.Parameters.AddWithValue("@teamId", teamId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            team.Id = reader.GetInt32(0);
                            team.Name = reader.GetString(1);
                        }
                    }
                }
                return team;
            }
        }
    }
}
