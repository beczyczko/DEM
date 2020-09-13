using System.Threading.Tasks;
using DEM.Common.Messages;
using DEM.Common.Types;

namespace DEM.Common.Dispatchers
{
    public interface IDispatcher
    {
        Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}