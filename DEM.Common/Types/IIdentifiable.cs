using System;

namespace DEM.Common.Types
{
    public interface IIdentifiable
    {
         Guid Id { get; }
    }
}