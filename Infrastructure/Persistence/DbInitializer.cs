using Domain.Entities;
using Infrastructure.Persistence;
using Domain.Entities;

namespace Ticketinador2000.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            var bocaPlayers = new string[] 
            {
                "Juan Román Riquelme", 
                "Martín Palermo", 
                "Carlos Tevez",
                "Diego Maradona", 
                "Roberto Abbondanzieri", 
                "Sebastián Battaglia",
                "Guillermo Barros Schelotto", 
                "Hugo Ibarra", 
                "Rolando Schiavi", 
                "Mauricio Serna"
            };

            var users = new List<User>();
            foreach (var name in bocaPlayers)
            {
                var emailPrefix = name.Replace(" ", "").Replace("ó", "o").Replace("á", "a").ToLowerInvariant();
                users.Add(new User
                {
                    Name = name,
                    Email = $"{emailPrefix}@bocajuniors.com",
                    PasswordHash = "daleboca123"
                });
            }
            context.Users.AddRange(users);
            context.SaveChanges();
        }

        if (context.Events.Any())
        {
            return;
        }

        var mainEvent = new Event
        {
            Name = "Concierto de Rock",
            EventDate = DateTime.UtcNow.AddMonths(1),
            Venue = "Estadio Alberto J. Armando",
            Status = "Active"
        };
        context.Events.Add(mainEvent);
        context.SaveChanges(); 

        var plateaSector = new Sector
        {
            EventId = mainEvent.Id,
            Name = "Platea",
            Price = 50000.00m,
            Capacity = 50
        };
        var popularSector = new Sector
        {
            EventId = mainEvent.Id,
            Name = "Popular",
            Price = 15000.00m,
            Capacity = 50
        };
        context.Sectors.AddRange(plateaSector, popularSector);
        context.SaveChanges(); 

        var seats = new List<Seat>();
        int plateaSeatCounter = 1;
        for (int i = 0; i < 50; i++)
        {
            seats.Add(new Seat
            {
                Id = Guid.NewGuid(),
                SectorId = plateaSector.Id,
                RowIdentifier = "PL-" + ((i / 10) + 1),
                SeatNumber = plateaSeatCounter,
                Status = "Available",
                Version = 1
            });

            if (plateaSeatCounter == 10)
                plateaSeatCounter = 1; 
            else
                plateaSeatCounter++;
        }
        int popularSeatCounter = 1;
        for (int i = 0; i < 50; i++)
        {
            seats.Add(new Seat
            {
                Id = Guid.NewGuid(),
                SectorId = popularSector.Id,
                RowIdentifier = "PO-" + ((i / 10) + 1),
                SeatNumber = popularSeatCounter,
                Status = "Available",
                Version = 1
            });

            if (popularSeatCounter == 10)
                popularSeatCounter = 1; 
            else
                popularSeatCounter++;
        }
        context.Seats.AddRange(seats);
        context.SaveChanges();
    }
}