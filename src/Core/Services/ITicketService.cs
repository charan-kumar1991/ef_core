using Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface ITicketService
    {
        Task<(IEnumerable<Ticket> tickets, int totalCount)> FetchTicketsAsync(TicketSearchParams searchParams);
        Task<Ticket> FetchTicketAsync(Guid pnr);
        Task<Ticket> AddTicketAsync(Ticket ticket);
    }
}
