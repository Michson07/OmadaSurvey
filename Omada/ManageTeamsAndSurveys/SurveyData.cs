using Microsoft.Data.SqlClient;
using Omada.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omada.ManageTeamsAndSurveys
{
    public class SurveyData
    {
        public void AddSurvey(OmadaSurvey survey)
        {
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO dbo.Surveys 
                                        VALUES(@UserId, @FirstAnswer, @SecondAnswer, @ThirdAnswer, @Date)";
                    command.Parameters.AddWithValue("@UserId", survey.UserId);
                    command.Parameters.AddWithValue("@FirstAnswer", survey.FirstAnswer);
                    command.Parameters.AddWithValue("@SecondAnswer", survey.SecondAnswer);
                    command.Parameters.AddWithValue("@ThirdAnswer", survey.ThirdAnswer);
                    command.Parameters.AddWithValue("@Date", survey.Date);
                    command.ExecuteNonQuery();
                }
            }
        }
        public bool CheckIfUserHaveDoneSurveyThisWeek(string userId)
        {
            int count = 0;
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT COUNT(UserID)
                                            FROM Surveys
                                            WHERE UserId = @userId 
                                            AND DATEPART(ww, SurveyDate) = DATEPART(ww, @utcNow)"; 
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@utcNow", DateTime.UtcNow);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }
                    }
                }
            }
            return count <= 0;
        }
    }
}
