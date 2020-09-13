using System.Threading.Tasks;
using DEM.Common.Messages;

namespace DEM.Common.Handlers
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}