using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface ICustomerService
    {
        Task<Customer> RegisterAsync(Customer entity);
        Task<Customer> FetchAsync(string login);
    }
}
