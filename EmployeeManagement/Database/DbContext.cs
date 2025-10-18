using EmployeeManagement.Models;
using MongoDB.Driver;

namespace EmployeeManagement.MongoDb
{
    public class DbContext
    {
        private readonly IMongoDatabase mongoDatabase;
        public DbContext(DbConfig dbConfig)
        {
            var mongoClient = new MongoClient(dbConfig.ConnectionString);
            mongoDatabase = mongoClient.GetDatabase(dbConfig.DatabaseName);
        }

       public IMongoCollection<Employee> Employees()
        {
            return mongoDatabase.GetCollection<Employee>("Employees");

        }

    }
}
