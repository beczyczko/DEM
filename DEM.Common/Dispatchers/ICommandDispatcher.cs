using System.Threading.Tasks;
using DEM.Common.Messages;

namespace DEM.Common.Dispatchers
{
    public interface ICommandDispatcher
    {
         Task SendAsync<T>(T command) where T : ICommand;
    }
}