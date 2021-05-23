using Core.Interfaces;
using Core.Models;
using Core.Services;
using Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repository;
        public TicketService(ITicketRepository repository)
        {
            _repository = repository;
        }

        public async Task<Ticket> AddTicketAsync(Ticket ticket)
        {
            ticket.PNR = Guid.NewGuid();

            Random r = new Random();
            ticket.Fare = r.Next(1000, 10000);

            Ticket created = await _repository.AddAsync(ticket);
            created.TravelDate = created.TravelDate.FromUTCToLocal();
            created.CreatedAt = created.CreatedAt.FromUTCToLocal();
            return created;
        }

        public async Task<Ticket> FetchTicketAsync(Guid pnr)
        {
            Ticket found = await _repository.FirstOrDefaultAsync((t) => t.PNR.Equals(pnr));

            if (found == default) return found;

            if (found.Passenger != default && !string.IsNullOrEmpty(found.Passenger.Password))
            {
                found.Passenger.Password = string.Empty;
            }

            found.TravelDate = found.TravelDate.FromUTCToLocal();
            found.CreatedAt = found.CreatedAt.FromUTCToLocal();

            if (found.UpdatedAt.HasValue)
                found.UpdatedAt = found.UpdatedAt.Value.FromUTCToLocal();

            return found;
        }

        public async Task<(IEnumerable<Ticket> tickets, int totalCount)> FetchTicketsAsync(TicketSearchParams searchParams)
        {
            (IEnumerable<Ticket> tickets, int totalCount) = (Enumerable.Empty<Ticket>(), 0);

            if (string.IsNullOrEmpty(searchParams.From) && string.IsNullOrEmpty(searchParams.To))
            {
                tickets = await _repository.GetAllAsync(searchParams.Page, searchParams.Limit);
                totalCount = await _repository.CountAsync();
            }

            else if (!string.IsNullOrEmpty(searchParams.From) && string.IsNullOrEmpty(searchParams.To))
            {
                tickets = await _repository.GetAllAsync((t) => t.To == searchParams.To, searchParams.Page, searchParams.Limit);
                totalCount = await _repository.CountAsync((t) => t.To == searchParams.To);
            }

            else if (string.IsNullOrEmpty(searchParams.From) && !string.IsNullOrEmpty(searchParams.To))
            {
                tickets = await _repository.GetAllAsync((t) => t.From == searchParams.From, searchParams.Page, searchParams.Limit);
                totalCount = await _repository.CountAsync((t) => t.From == searchParams.From);
            }

            else if (!string.IsNullOrEmpty(searchParams.From) && !string.IsNullOrEmpty(searchParams.To))
            {
                Expression<Func<Ticket, bool>> predicate = (t) => t.From == searchParams.From && t.To == searchParams.To;
                tickets = await _repository.GetAllAsync(predicate, searchParams.Page, searchParams.Limit);
                totalCount = await _repository.CountAsync(predicate);
            }

            tickets = tickets.Select((ticket) =>
            {
                return new Ticket
                {
                    Id = ticket.Id,
                    PNR = ticket.PNR,
                    SeatNumber = ticket.SeatNumber,
                    Berth = ticket.Berth,
                    From = ticket.From,
                    To = ticket.To,
                    CustomerId = ticket.CustomerId,
                    Fare = ticket.Fare,
                    TravelDate = ticket.TravelDate.FromUTCToLocal(),
                    Passenger = new Customer
                    {
                        Id = ticket.Passenger.Id,
                        FirstName = ticket.Passenger.FirstName,
                        LastName = ticket.Passenger.LastName,
                        DateOfBirth = ticket.Passenger.DateOfBirth.FromUTCToLocal(),
                        Phone = ticket.Passenger.Phone,
                        GenderAbbreviation = ticket.Passenger.GenderAbbreviation,
                        Username = ticket.Passenger.Username,
                        Email = ticket.Passenger.Email,
                        Password = string.Empty,
                        CreatedAt = ticket.Passenger.CreatedAt.FromUTCToLocal(),
                        UpdatedAt = ticket.Passenger.UpdatedAt.HasValue ? ticket.Passenger.UpdatedAt.Value.FromUTCToLocal() : ticket.Passenger.UpdatedAt.GetValueOrDefault()
                    },
                    CreatedAt = ticket.CreatedAt.FromUTCToLocal(),
                    UpdatedAt = ticket.UpdatedAt.HasValue ? ticket.UpdatedAt.Value.FromUTCToLocal() : ticket.UpdatedAt.GetValueOrDefault()
                };
            });

            return (tickets, totalCount);
        }
    }
}
