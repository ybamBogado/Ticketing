
using Application.DTOs;
using Application.Queries;
using System;

namespace Application.Interfaces
{
    public interface IGetEventCatalogQueryHandler
    {
        Task<IEnumerable<EventCatalogDto>> HandlerAsync(GetEventCatalogQuery query);
    }
}