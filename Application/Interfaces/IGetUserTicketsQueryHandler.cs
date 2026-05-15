using Application.Queries;
using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGetUserTicketsQueryHandler
    {
        Task<IEnumerable<UserTicketDto>> HandlerAsync(GetUserTicketsQuery query);
    }
}
