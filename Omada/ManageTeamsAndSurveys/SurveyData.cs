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
    }
}
