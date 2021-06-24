using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using TestApp.Models;

namespace TestApp.Repositories
{
    public class ClientRepository: IClientRepository
    {
        public Client GetById(int id)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["appDatabase"].ConnectionString;

            using var db = new SqlConnection(connectionString);

            var client = db.QuerySingleOrDefault<Client>(
                "uspGetClientById",
                new {ClientId = id},
                commandType: CommandType.StoredProcedure);

            return client;
        }
    }
}