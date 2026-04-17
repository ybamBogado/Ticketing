using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Commands
{
    public class CreateEventCommand : ICreateEventCommand
    {
        private readonly AppDbContext _context;
        public CreateEventCommand(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> ExecuteAsync(CreateEventRequest request)
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
