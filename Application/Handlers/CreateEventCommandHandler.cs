
using Domain.Entities;
using Application.Commands;


using Application.Interfaces;

namespace Application.Handlers
{
    public class CreateEventCommandHandler : ICreateEventCommandHandler
    {
        private readonly IAppDbContext _context;
        public CreateEventCommandHandler(IAppDbContext context)
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
