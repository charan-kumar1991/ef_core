using Core.Interfaces;
using Core.Models;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class TicketRepository : ITicketRepository
    {
        private readonly RailwayTicketContext _context;
        public TicketRepository(RailwayTicketContext context)
        {
            _context = context;
        }

        public async Task<Ticket> AddAsync(Ticket entity)
        {
            _context.Set<Ticket>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> CountAsync()
        {
            // select count(*) from [dbo].tickets;
            return await _context.Set<Ticket>().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<Ticket, bool>> predicate)
        {
            // select count(*) from [dbo].tickets where col1=val1 AND col2=val2...
            return await _context.Set<Ticket>().CountAsync(predicate);
        }

        public async Task<Ticket> FirstOrDefaultAsync(Expression<Func<Ticket, bool>> predicate)
        {
            // select top 1 ticket.Id, ticket.pnr, ... from [dbo].tickets where col1=val1 AND col2=val2...
            Ticket ticket = await _context.Set<Ticket>()
                .Include(t => t.Passenger)
                .FirstOrDefaultAsync(predicate);

            if (ticket == default) return ticket;

            Ticket found = new Ticket
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

            return found;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync(int page, int limit)
        {
            // select * from [dbo].tickets
            // ORDER BY c.Id
            // OFFSET offset ROWS
            // FETCH NEXT @limit ROWS ONLY;

            int offset = (page - 1) * limit;

            return await _context.Set<Ticket>()
                .Include(t => t.Passenger)
                .OrderBy(t => t.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync(Expression<Func<Ticket, bool>> predicate, int page, int limit)
        {
            int offset = (page - 1) * limit;

            /*
             select *
             from [dbo].tickets t
             left join [dbo].customers c
             ON t.CustomerId = c.Id
             WHERE col1=val1, col2=val2...
             ORDER BY c.Id
             OFFSET offset ROWS
             FETCH NEXT @limit ROWS ONLY;
            */
            return await _context.Set<Ticket>()
                .Include(t => t.Passenger)
                .Where(predicate)
                .OrderBy(c => c.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }
    }
}
