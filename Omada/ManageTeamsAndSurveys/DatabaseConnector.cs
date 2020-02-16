using Microsoft.Data.SqlClient;
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
            var connection = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=Omada;Trusted_Connection=True;MultipleActiveResultSets=true");
            connection.Open();
            return connection;
        }
    }
}
