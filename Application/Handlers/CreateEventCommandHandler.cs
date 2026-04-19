
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Commands;

using Application.Interfaces;

namespace Application.Handlers
{
    public class CreateEventCommandHandler : ICreateEventCommandHandler
    {
        private readonly AppDbContext _context;
        public CreateEventCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> HandlerAsync(CreateEventCommand request)
        {
            var newEvent = new Event
            {
                Name = request.Name,
                EventDate = request.EventDate,
                Venue = request.Venue,
                Status = "Active"
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return newEvent.Id;
        }
        

       
    }
}
