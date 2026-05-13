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
                Status = "Active",
                Sectors = new List<Sector>()
            };

            foreach (var sectorDto in request.Sectors)
            {
                var sector = new Sector
                {
                    Name = sectorDto.Name,
                    Price = sectorDto.Price,
                    Capacity = sectorDto.Capacity,
                    Seats = new List<Seat>()
                };

                int seatsPerRow = 20;
                char currentRow = 'A';
                int currentSeatInRow = 1;
                string prefix = sectorDto.Name.Length >= 2 ? sectorDto.Name.Substring(0, 2).ToUpper() : "XX";

                for (int i = 0; i < sectorDto.Capacity; i++)
                {
                    sector.Seats.Add(new Seat
                    {
                        Id = Guid.NewGuid(),
                        RowIdentifier = $"{prefix}-{currentRow}",
                        SeatNumber = currentSeatInRow,
                        Status = "Available",
                        Version = 0
                    });

                    currentSeatInRow++;
                    if (currentSeatInRow > seatsPerRow)
                    {
                        currentSeatInRow = 1;
                        currentRow++; 
                    }
                }

                newEvent.Sectors.Add(sector);
            }

            await _eventRepository.AddEventAsync(newEvent);
            await _unitOfWork.SaveChangesAsync();
            return newEvent.Id;
        }       
    }
}
