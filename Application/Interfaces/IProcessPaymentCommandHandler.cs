using Application.DTOs;
using Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProcessPaymentCommandHandler
    {
        Task<bool> HandlerAsync(ProcessPaymentCommand command);
    }
}
