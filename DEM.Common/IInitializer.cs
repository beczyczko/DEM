using System.Threading.Tasks;

namespace DEM.Common
{
    public interface IInitializer
    {
        Task InitializeAsync();
    }
}