using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DEM.Common.Dispatchers;
using DEM.Common.Messages;
using DEM.Common.Types;
using Microsoft.AspNetCore.Mvc;

namespace DEM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public BaseController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        protected async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
            => await _dispatcher.QueryAsync<TResult>(query);

        protected ActionResult<T> Single<T>(T data)
        {
            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        protected ActionResult<T> Single<T>(Maybe<T> data)
        {
            if (data.HasNoValue)
            {
                return NotFound();
            }

            return Ok(data.Value);
        }

        protected async Task SendAsync<T>(T command) where T : ICommand
            => await _dispatcher.SendAsync(command);

        protected ActionResult<PagedResult<T>> Collection<T>(PagedResult<T> pagedResult)
        {
            if (pagedResult == null)
            {
                return NotFound();
            }

            return Ok(pagedResult);
        }

        protected ActionResult<IEnumerable<T>> Collection<T>(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                return NotFound();
            }

            return Ok(collection);
        }
    }
}