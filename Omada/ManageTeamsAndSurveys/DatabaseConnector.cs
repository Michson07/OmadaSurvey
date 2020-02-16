using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omada.ManageTeamsAndSurveys

{
    public static class DatabaseConnector
    {
        public static SqlConnection CreateConnection()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot configurationRoot = configurationBuilder.Build();
            SqlConnection connection = new SqlConnection(configurationRoot.GetConnectionString("OmadaContextConnection"));
            connection.Open();
            return connection;
        }
    }
}
