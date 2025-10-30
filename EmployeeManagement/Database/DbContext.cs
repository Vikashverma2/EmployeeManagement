using EmployeeManagement.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EmployeeManagement.MongoDb
{
    public class DbContext
    {
        private readonly IMongoDatabase mongoDatabase;
        public DbContext(IOptions<DbConfig> dbConfig)
        {
            if (dbConfig == null)
                throw new ArgumentNullException(nameof(dbConfig));

            var mongoClient = new MongoClient(dbConfig.Value.ConnectionString);
            mongoDatabase = mongoClient.GetDatabase(dbConfig.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return mongoDatabase.GetCollection<T>(collectionName);
        }
        


    }  
}
