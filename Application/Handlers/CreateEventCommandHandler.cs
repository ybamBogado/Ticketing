using Application.Commands;
using Application.Interfaces.Repositories;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Handlers
{
    public class CreateEventCommandHandler : ICreateEventCommandHandler
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
        {
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
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
            _eventRepository.AddEventAsync(newEvent);
            await _unitOfWork.SaveChangesAsync();
            return newEvent.Id;
        }       
    }
}
