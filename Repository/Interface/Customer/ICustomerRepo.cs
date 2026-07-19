using POSbackend.DTO.Customer;

namespace POSbackend.Repository.Interface.Customer
{
    public interface ICustomerRepo
    {
        Task<CustomerDto> AddCustomerAsync(CustomerDto customer);
        Task<CustomerDto> GetByNumberAsync(long customerNumber);
    }
}
