using System.Threading.Tasks;
using DEM.Common.Types;

namespace DEM.Common.Dispatchers
{
    public interface IQueryDispatcher
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}