using Microsoft.Data.SqlClient;
using Omada.Areas.Identity.Data;
using Omada.ManageTeamsAndSurveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omada.Pages.ManageUser
{
    public class UserDataNoEntity
    {
        public List<OmadaUser> GetAllUsers()
        {
            List<OmadaUser> users = new List<OmadaUser>();
            using (SqlConnection connection = DatabaseConnector.CreateConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT Id, Email
                                            FROM dbo.AspNetUsers";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OmadaUser user = new OmadaUser();
                            user.Id = reader.GetString(0);
                            user.Email = reader.GetString(1);
                            users.Add(user);
                        }
                    }
                }
                return users;
            }
        }
    }
}
