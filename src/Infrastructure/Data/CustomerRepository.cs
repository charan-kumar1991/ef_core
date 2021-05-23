using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly RailwayTicketContext _context;
        public CustomerRepository(RailwayTicketContext context)
        {
            _context = context;
        }

        public async Task<Customer> AddAsync(Customer entity)
        {
            _context.Set<Customer>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> CountAsync()
        {
            // select count(*) from [dbo].customers;
            return await _context.Set<Customer>().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<Customer, bool>> predicate)
        {
            // select count(*) from [dbo].customers where col1=val1 AND col2=val2...
            return await _context.Set<Customer>().CountAsync(predicate);
        }

        public async Task<Customer> FirstOrDefaultAsync(Expression<Func<Customer, bool>> predicate)
        {
            // select top 1 customer.Id, customer.FirstName, ... from [dbo].customers where col1=val1 AND col2=val2...
            return await _context.Set<Customer>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(int page, int limit)
        {
            // select * from [dbo].customers
            // ORDER BY c.Id
            // OFFSET offset ROWS
            // FETCH NEXT @limit ROWS ONLY;

            int offset = (page - 1) * limit;

            return await _context.Set<Customer>()
                        .OrderBy(c => c.Id)
                        .Skip(offset)
                        .Take(limit)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(Expression<Func<Customer, bool>> predicate, int page, int limit)
        {
            int offset = (page - 1) * limit;

            /*
             select *
             from [dbo].Customers c
             left join [dbo].Tickets t 
             ON t.CustomerId = c.Id
             WHERE col1=val1, col2=val2...
             ORDER BY c.Id
             OFFSET offset ROWS
             FETCH NEXT @limit ROWS ONLY;
            */
            return await _context.Set<Customer>()
                .Include(c => c.Tickets) // Eager loading
                .Where(predicate)
                .OrderBy(c => c.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }
    }
}
