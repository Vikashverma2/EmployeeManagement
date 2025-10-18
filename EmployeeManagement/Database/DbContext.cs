using EmployeeManagement.Models;
using MongoDB.Driver;

namespace EmployeeManagement.MongoDb
{
    public class DbContext
    {
        private readonly IMongoDatabase mongoDatabase;
        public DbContext(DbConfig dbConfig)
        {
            if (dbConfig == null)
                throw new ArgumentNullException(nameof(dbConfig));

            var mongoClient = new MongoClient(dbConfig.ConnectionString);
            mongoDatabase = mongoClient.GetDatabase(dbConfig.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return mongoDatabase.GetCollection<T>(collectionName);
        }
       

    }  
}
