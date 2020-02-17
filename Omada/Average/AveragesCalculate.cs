using Microsoft.Data.SqlClient;
using Omada.Areas.Identity.Data;
using Omada.ManageTeamsAndSurveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omada.Average
{
    public class AveragesCalculate
    {
        public List<OmadaSurveysAverage> GetSurveysAverages(OmadaTeam team)
        {
            List<OmadaSurveysAverage> averageWeeks = new List<OmadaSurveysAverage>();
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT DATEPART(ww, SurveyDate) AS SURVEYS_WEEK, ROUND(AVG(CAST(FirstAnswer AS FLOAT)), 2) AS SURVEYS_AVG
                                            FROM Surveys 
                                            WHERE USERID IN 
	                                            (SELECT UserID 
	                                            FROM Users_Teams
	                                            WHERE TeamId = @TeamId
                                                AND YEAR(SurveyDate) = @SurveysYear)
                                            GROUP BY DATEPART(ww, SurveyDate)
                                            ORDER BY SURVEYS_WEEK;";
                    command.Parameters.AddWithValue("@TeamId", team.Id);
                    command.Parameters.AddWithValue("@SurveysYear", DateTime.UtcNow.Year);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OmadaSurveysAverage averageWeek = new OmadaSurveysAverage();
                            averageWeek.Week = reader.GetInt32(0);
                            averageWeek.Average = reader.GetDouble(1);
                            averageWeeks.Add(averageWeek);
                        }
                    }
                }
                return averageWeeks;
            }
        }
    }
}
