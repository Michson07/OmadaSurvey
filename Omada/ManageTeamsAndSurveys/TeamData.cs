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
                            team.IsPublic = reader.GetBoolean(2);
                            team.OpinionsVisible = reader.GetBoolean(3);
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
                SqlCommand command = new SqlCommand();
                using (command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO dbo.Teams 
                                    VALUES(@Name, @IsPublic, @OpinionsVisible)";
                    command.Parameters.AddWithValue("@Name", team.Name);
                    command.Parameters.AddWithValue("@IsPublic", team.IsPublic);
                    command.Parameters.AddWithValue("@OpinionsVisible", team.OpinionsVisible);
                    command.ExecuteNonQuery();
                }
                using (command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT Id
                                    FROM dbo.Teams
                                    WHERE Name = @Name";
                    command.Parameters.AddWithValue("@Name", team.Name);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            team.Id = reader.GetInt32(0);
                        }
                    }
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
                            team.IsPublic = reader.GetBoolean(2);
                            team.OpinionsVisible = reader.GetBoolean(3);
                        }
                    }
                }
            }
            return team;
        }

        public List<OmadaUser> GetTeamUsers(int teamId)
        {
            List<OmadaUser> teamUsers = new List<OmadaUser>();
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT dbo.AspNetUsers.Id, dbo.AspNetUsers.UserName, dbo.AspNetUsers.Email
                                            FROM dbo.Users_Teams
                                            JOIN DBO.AspNetUsers
                                            ON Dbo.AspNetUsers.Id = dbo.Users_Teams.UserId
                                            AND dbo.Users_Teams.TeamId = @teamId";
                    command.Parameters.AddWithValue("@teamId", teamId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OmadaUser user = new OmadaUser();
                            user.Id = reader.GetString(0);
                            user.UserName = reader.GetString(1);
                            user.Email = reader.GetString(2);
                            teamUsers.Add(user);
                        }
                    }
                }
            }
            return teamUsers;
        }
        public OmadaTeam Update (OmadaTeam team)
        {
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE dbo.Teams 
                                            SET Name = @Name, IsPublic = @IsPublic, OpinionsVisible = @OpinionsVisible
                                            WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", team.Id);
                    command.Parameters.AddWithValue("@Name", team.Name);
                    command.Parameters.AddWithValue("@IsPublic", team.IsPublic);
                    command.Parameters.AddWithValue("@OpinionsVisible", team.OpinionsVisible);

                    command.ExecuteNonQuery();
                }
            }
            return team;
        }
        public List<NotTeamMember> UsersNotInTeam(int teamId)
        {
            List<NotTeamMember> teamUsersList = new List<NotTeamMember>();
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT Id, UserName
                                            FROM dbo.AspNetUsers
                                            WHERE Id NOT IN
	                                            (SELECT UserId 
	                                            FROM dbo.Users_Teams
	                                            WHERE TeamId = @teamId)";
                    command.Parameters.AddWithValue("@teamId", teamId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OmadaUser user = new OmadaUser();
                            user.Id = reader.GetString(0);
                            user.UserName = reader.GetString(1);
                            teamUsersList.Add(new NotTeamMember() 
                            { 
                                User = user, 
                                IsSelected = false 
                            });
                        }
                    }
                }
            }
            return teamUsersList;
        }
        public void AddUserToTeam(string userId, int teamId, int isLeader)
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
        public List<OmadaTeam> GetLeaderTeams(string leaderId)
        {
            List<OmadaTeam> teams = new List<OmadaTeam>();
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT dbo.Teams.Id, dbo.Teams.Name, dbo.Teams.IsPublic, dbo.Teams.OpinionsVisible
                                            FROM dbo.Users_Teams
                                            JOIN dbo.Teams 
                                            ON dbo.Users_Teams.TeamId = dbo.Teams.Id
                                            AND dbo.Users_Teams.UserId = @leaderId
                                            AND dbo.Users_Teams.IsLeader = 1";
                    command.Parameters.AddWithValue("@leaderId", leaderId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OmadaTeam team = new OmadaTeam();
                            team.Id = reader.GetInt32(0);
                            team.Name = reader.GetString(1);
                            team.IsPublic = reader.GetBoolean(2);
                            team.OpinionsVisible = reader.GetBoolean(3);
                            teams.Add(team);
                        }
                    }
                }
            }
            return teams;
        }
        public List<OmadaTeam> GetUserTeams(string userId)
        {
            List<OmadaTeam> teams = new List<OmadaTeam>();
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT dbo.Teams.Id, dbo.Teams.Name, dbo.Teams.IsPublic, dbo.Teams.OpinionsVisible
                                            FROM dbo.Users_Teams
                                            JOIN dbo.Teams 
                                            ON dbo.Users_Teams.TeamId = dbo.Teams.Id
                                            AND dbo.Users_Teams.UserId = @userId";
                    command.Parameters.AddWithValue("@userId", userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OmadaTeam team = new OmadaTeam();
                            team.Id = reader.GetInt32(0);
                            team.Name = reader.GetString(1);
                            team.IsPublic = reader.GetBoolean(2);
                            team.OpinionsVisible = reader.GetBoolean(3);
                            teams.Add(team);
                        }
                    }
                }
            }
            return teams;
        }
        public List<OmadaTeam> GetTeamsWhereUserNotMember(string userId)
        {
            List<OmadaTeam> teams = new List<OmadaTeam>();
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT DISTINCT dbo.Teams.Id, dbo.Teams.Name, dbo.Teams.IsPublic, dbo.Teams.OpinionsVisible
                                            FROM dbo.Users_Teams
                                            JOIN dbo.Teams 
                                            ON dbo.Users_Teams.TeamId = dbo.Teams.Id
                                            AND dbo.Users_Teams.TeamId NOT IN 
                                                (SELECT TeamId 
                                                FROM dbo.Users_Teams
                                                WHERE UserId = @userId)";
                    command.Parameters.AddWithValue("@userId", userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OmadaTeam team = new OmadaTeam();
                            team.Id = reader.GetInt32(0);
                            team.Name = reader.GetString(1);
                            team.IsPublic = reader.GetBoolean(2);
                            team.OpinionsVisible = reader.GetBoolean(3);
                            teams.Add(team);
                        }
                    }
                }
            }
            return teams;
        }
        public List<OmadaUser> GetTeamLeaders(OmadaTeam team)
        {
            List<OmadaUser> teamUsers = new List<OmadaUser>();
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT dbo.AspNetUsers.Id, dbo.AspNetUsers.UserName, dbo.AspNetUsers.Email
                                            FROM dbo.Users_Teams
                                            JOIN DBO.AspNetUsers
                                            ON Dbo.AspNetUsers.Id = dbo.Users_Teams.UserId
                                            AND dbo.Users_Teams.TeamId = @teamId
                                            AND dbo.Users_Teams.IsLeader = 1";
                    command.Parameters.AddWithValue("@teamId", team.Id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OmadaUser user = new OmadaUser();
                            user.Id = reader.GetString(0);
                            user.UserName = reader.GetString(1);
                            user.Email = reader.GetString(2);
                            teamUsers.Add(user);
                        }
                    }
                }
            }
            return teamUsers;
        }
        public void RemoveTeamMember(string userId, OmadaTeam team)
        {
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"DELETE FROM dbo.Users_Teams 
                                            WHERE TeamId = @TeamId
                                            AND UserId = @UserId";
                    command.Parameters.AddWithValue("@TeamId", team.Id);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void SetNoLeaders(OmadaTeam team)
        {
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE dbo.Users_Teams 
                                            SET IsLeader = 0
                                            WHERE teamId = @teamId";
                    command.Parameters.AddWithValue("@teamId", team.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateLeaderStatus(string userId, OmadaTeam team)
        {
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE dbo.Users_Teams 
                                        SET IsLeader = 1
                                        WHERE teamId = @teamId
                                        AND UserId = @userId";
                    command.Parameters.AddWithValue("@teamId", team.Id);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
