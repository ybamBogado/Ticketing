using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.DTOs;

namespace Application.Interfaces
{
    public interface ICreateEventCommand
    {
        Task<int> ExecuteAsync(CreateEventRequest request);
    }
}
