using System.Threading.Tasks;

namespace DEM.Common.Mongo
{
    public interface IMongoDbSeeder
    {
        Task SeedAsync();
    }
}