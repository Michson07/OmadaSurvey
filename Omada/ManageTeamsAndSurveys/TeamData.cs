﻿using System;
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
                SqlCommand command = new SqlCommand();
                using (command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO dbo.Teams 
                                            VALUES(@Name)";
                    command.Parameters.AddWithValue("@Name", team.Name);
                    command.ExecuteNonQuery();
                }
                using(command = connection.CreateCommand())
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
                                            SET Name = @Name
                                            WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", team.Id);
                    command.Parameters.AddWithValue("@Name", team.Name);
                    command.ExecuteNonQuery();
                }
            }
            return team;
        }
        public List<TeamUsers> UsersNotInTeam(int teamId)
        {
            List<TeamUsers> teamUsersList = new List<TeamUsers>();
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
                            teamUsersList.Add(new TeamUsers() 
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
        public void AddUserToTeam(string userId, int teamId)
        {
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO dbo.Users_Teams VALUES(@UserId, @TeamId, @IsLeader)";
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@TeamId", teamId);
                    command.Parameters.AddWithValue("@IsLeader", "false");
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
