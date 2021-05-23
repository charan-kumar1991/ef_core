using Api.Models.Requests;
using Api.Models.Responses;
using Core.Models;
using Core.Services;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IPasswordService _passwordService;
        public AccountsController(
            ICustomerService customerService, 
            IPasswordService passwordService
        )
        {
            _customerService = customerService;
            _passwordService = passwordService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto dto)
        {
            try
            {
                Customer c = new Customer
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    GenderAbbreviation = dto.GenderAbbreviation,
                    DateOfBirth = dto.DateOfBirth.ToUniversalTime(),
                    Username = dto.Username,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Password = _passwordService.HashPassword(dto.Password),
                    CreatedAt = DateTime.UtcNow
                };

                await _customerService.RegisterAsync(c);
                return Ok();
            }
            catch(UniqueConstraintException)
            {
                return Conflict(new HttpError("DUPLICATE_ENTRY", "Username / Email is already taken!"));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new HttpError("SERVER_ERROR", ex.Message));
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto dto)
        {
            try
            {
                Customer found = await _customerService.FetchAsync(dto.Login);
                if (found == default)
                {
                    return NotFound(new HttpError("ENTRY_NOT_FOUND", "No user found with username/email"));
                }

                if (!_passwordService.VerifyPassword(found.Password, dto.Password))
                {
                    return BadRequest(new HttpError("ENTRY_NOT_FOUND", "Incorrect password"));
                }

                found.Password = string.Empty;
                return Ok(found);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new HttpError("SERVER_ERROR", ex.Message));
            }
        }
    }
}
