using Microsoft.EntityFrameworkCore;
using POSbackend.DTO.Customer;
using POSbackend.Models;
using POSbackend.Repository.Interface.Customer;

namespace POSbackend.Repository.implement.Customer
{
    public class CustomerRepo(PosdbContext _context) : ICustomerRepo
    {
        public async Task<CustomerDto> AddCustomerAsync(CustomerDto customerDto)
        {
            // Map DTO → Entity
            var customer = new Models.Customer
            {
                CustomerNumber = customerDto.CustomerNumber
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Map Entity → DTO
            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                CustomerNumber = (long)customer.CustomerNumber
            };
        }

        public async Task<CustomerDto?> GetByNumberAsync(long customerNumber)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerNumber == customerNumber);

            if (customer == null) return null;

            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                CustomerNumber = (long)customer.CustomerNumber
            };
        }
    }
}
