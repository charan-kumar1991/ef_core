using Api.Models.Requests;
using Api.Models.Responses;
using Core.Models;
using Core.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _service;
        private readonly LinkHelper _linkHelper;
        public TicketsController(
            ITicketService service,
            LinkHelper linkHelper
        )
        {
            _service = service;
            _linkHelper = linkHelper;
        }

        [HttpGet]
        public async Task<IActionResult> FetchTicketsAsync([FromQuery] TicketSearchParams searchParams)
        {
            (IEnumerable<Ticket> tickets, int totalCount) = await _service.FetchTicketsAsync(searchParams);

            PagedResponse<Ticket> pagedTickets = _linkHelper.BuildPaginationResponse<Ticket>(
                tickets,
                totalCount,
                searchParams.Page,
                searchParams.Limit,
                Request.Path.Value
            );

            Response.Headers.Add("X-Pagination-Per-Page", searchParams.Limit.ToString());
            Response.Headers.Add("X-Pagination-Current-Page", searchParams.Page.ToString());
            Response.Headers.Add("X-Pagination-Total-Pages", pagedTickets.Pages.ToString());
            Response.Headers.Add("X-Pagination-Total-Entries", totalCount.ToString());

            return Ok(pagedTickets);
        }

        [HttpGet("{pnr:Guid}")]
        public async Task<IActionResult> FetchTicketAsync([FromRoute] Guid pnr)
        {
            Ticket found = await _service.FetchTicketAsync(pnr);
            if (found == default) return NotFound(new HttpError("ENTRY_NOT_FOUND", "No ticket found with PNR"));
            return Ok(found);
        }

        [HttpPost]
        public async Task<IActionResult> AddTicketAsync([FromBody] AddTicketDto dto)
        {
            Ticket ticket = new Ticket
            {
                CustomerId = dto.CustomerId,
                SeatNumber = dto.SeatNumber,
                Berth = dto.Berth,
                From = dto.From,
                To = dto.To,
                TravelDate = dto.TravelDate.ToUniversalTime(),
                CreatedAt = DateTime.UtcNow
            };
            Ticket created = await _service.AddTicketAsync(ticket);
            return Ok(created);
        }

    }
}
