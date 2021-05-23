using Core.Interfaces;
using Core.Models;
using Core.Services;
using Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Customer> FetchAsync(string login)
        {
            Customer found = await _repository.FirstOrDefaultAsync((c) => c.Email == login || c.Username == login);
            if (found == default) return found;

            found.DateOfBirth = found.DateOfBirth.FromUTCToLocal();
            found.CreatedAt = found.CreatedAt.FromUTCToLocal();

            if (found.UpdatedAt.HasValue)
                found.UpdatedAt = found.UpdatedAt.Value.FromUTCToLocal();

            return found;
        }

        public async Task<Customer> RegisterAsync(Customer entity)
        {
            Customer created = await _repository.AddAsync(entity);

            created.DateOfBirth = created.DateOfBirth.FromUTCToLocal();
            created.CreatedAt = created.CreatedAt.FromUTCToLocal();

            return created;
        }
    }
}
