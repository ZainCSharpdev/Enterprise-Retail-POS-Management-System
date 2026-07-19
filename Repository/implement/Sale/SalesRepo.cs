using POSbackend.DTO.Sale;
using POSbackend.Models;
using Microsoft.EntityFrameworkCore;
using POSbackend.Repository.Interface.Sales;

namespace POSbackend.Repository.implement.Sale
{
    public class SalesRepo(PosdbContext _context) : ISaleRepo
    {
        public async Task<IEnumerable<SaleDto>> GetAllSalesAsync()
        {
            return await _context.Sales
                .Select(s => new SaleDto
                {
                    CustomerNumber = long.Parse(s.CustomerNumber),
                    status = s.Status,
                    Method = s.Method,
                    TotalAmount = s.TotalAmount
                }).ToListAsync();
        }

        public async Task<SaleDto?> GetSaleByIdAsync(int saleId)
        {
            return await _context.Sales
                .Where(s => s.SaleId == saleId)
                .Select(s => new SaleDto
                {
                    CustomerNumber = long.Parse(s.CustomerNumber),
                    status = s.Status,
                    Method = s.Method,
                    TotalAmount = s.TotalAmount
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SaleDto>> GetSalesByDateAsync(DateTime SaleDate)
        {
            return await _context.Sales
                .Where(s => s.SaleDate == SaleDate)
                .Select(s => new SaleDto
                {
                    CustomerNumber = long.Parse(s.CustomerNumber),
                    status = s.Status,
                    Method = s.Method,
                    TotalAmount = s.TotalAmount
                }).ToListAsync();
        }

        public async Task<IEnumerable<SaleDto>> GetSalesByCustomerAsync(long customerNumber)
        {
            return await _context.Sales
                .Where(s => s.CustomerNumber == customerNumber.ToString())
                .Select(s => new SaleDto
                {
                    CustomerNumber = long.Parse(s.CustomerNumber),
                    status = s.Status,
                    Method = s.Method,
                    TotalAmount = s.TotalAmount
                }).ToListAsync();
        }

        public async Task<IEnumerable<SaleDto?>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Sales
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .Select(s => new SaleDto
                {
                    CustomerNumber = long.Parse(s.CustomerNumber),
                    status = s.Status,
                    Method = s.Method,
                    TotalAmount = s.TotalAmount
                }).ToListAsync();
        }



        // Implement the CreateSaleAsync, UpdateSaleAsync,and DeleteSaleAsync
        private long GenerateCustomerNumber()
        {
            return DateTime.UtcNow.Ticks;
        }

        public async Task<SaleDto?> CreateSaleAync(SaleDto sale, List<SalesDetailsDto> details)
        {
            long customerNumber = GenerateCustomerNumber();
            var customer = new Models.Customer
            {
                CustomerNumber = customerNumber,
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var newSale = new Models.Sale
            {
                SaleDate = DateTime.Now,
                CustomerNumber = customerNumber.ToString(), // FIX: Uses generated token, not 0 from front-end
                Status = "InProcess",
                Method = sale.Method,
                CreatedDate = DateTime.Now
            };

            await _context.Sales.AddAsync(newSale);
            await _context.SaveChangesAsync(); // Generates newSale.SaleId database identity primary key

            // Add line items if any are passed
            if (details != null && details.Count > 0)
            {
                foreach (var d in details)
                {
                    var saleDetail = new SaleDetail
                    {
                        SaleId = newSale.SaleId,
                        ProductId = (int)d.ProductId,
                        Qty = (int)d.Quantity,
                        UnitPrice = (decimal)d.UnitPrice,
                        TotalPrice = (decimal)((decimal)d.Quantity * d.UnitPrice)
                    };
                    await _context.SaleDetails.AddAsync(saleDetail);
                }
                await _context.SaveChangesAsync();

                newSale.TotalAmount = (decimal)details.Sum(x => x.Quantity * x.UnitPrice);
            }
            else
            {
                newSale.TotalAmount = 0;
            }

            newSale.NetAmount = (newSale.TotalAmount - (sale.Discount ?? 0)) + (sale.Tax ?? 0);

            _context.Sales.Update(newSale);
            await _context.SaveChangesAsync();

            // CRITICAL FIX: Explicitly map identity values to the returning DTO properties
            sale.SaleId = newSale.SaleId;            // <-- THIS FIXES THE REACT "UNDEFINED" / NOT RETRIEVED ERROR
            sale.CustomerNumber = customerNumber;    // <-- Passes the autogenerated tracking number to the frontend
            sale.SaleDate = newSale.SaleDate;
            sale.TotalAmount = newSale.TotalAmount;
            sale.NetAmount = newSale.NetAmount;
            sale.status = newSale.Status;

            return sale;
        }

        public async Task<SaleDto?> UpdateSaleAync(SaleDto sale, List<SalesDetailsDto> details)
        {
            var existingSale = await _context.Sales
                .Include(x => x.SaleDetails)
                .FirstOrDefaultAsync(x => x.SaleId == sale.SaleId);

            if (existingSale == null) return null;

            _context.SaleDetails.RemoveRange(existingSale.SaleDetails);
            foreach (var d in details)
            {
                var saleDetail = new SaleDetail
                {
                    SaleId = existingSale.SaleId,
                    ProductId = (int)d.ProductId,
                    Qty = (int)d.Quantity,
                    UnitPrice = (decimal)d.UnitPrice,
                    TotalPrice = (decimal)((decimal)d.Quantity * d.UnitPrice)
                };
                await _context.SaleDetails.AddAsync(saleDetail);
            }

            existingSale.TotalAmount = (decimal)details.Sum(x => x.Quantity * x.UnitPrice);
            existingSale.NetAmount = (existingSale.TotalAmount - (sale.Discount ?? 0)) + (sale.Tax ?? 0);
            existingSale.Method = sale.Method;
            existingSale.Status = sale.status;

            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<bool> DeleteSaleAsync(int saleId)
        {
            var sale = await _context.Sales
                .Include(x => x.SaleDetails)
                .FirstOrDefaultAsync(x => x.SaleId == saleId);

            if (sale == null) return false;

            _context.SaleDetails.RemoveRange(sale.SaleDetails);
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
            return true;
        }



        //Finalize Sale


        public async Task<SaleDto?> FinalizeSaleAsync(int saleId, string method, decimal discount, decimal tax)
        {
            var sale = await _context.Sales
                .Include(s => s.SaleDetails)
                .FirstOrDefaultAsync(s => s.SaleId == saleId);

            if (sale == null) return null;

            var subtotal = sale.SaleDetails.Sum(sd => sd.TotalPrice);
            sale.TotalAmount = subtotal;
            sale.Discount = discount;
            sale.Tax = tax;
            sale.NetAmount = (subtotal - discount) + tax;
            sale.Method = method;
            sale.Status = "Paid";

            await _context.SaveChangesAsync();

            return new SaleDto
            {
                SaleId = sale.SaleId,
                SaleDate = sale.SaleDate,
                CustomerNumber = long.Parse(sale.CustomerNumber),
                status = sale.Status,
                Method = sale.Method,
                TotalAmount = sale.TotalAmount,
                Discount = sale.Discount,
                Tax = sale.Tax,
                NetAmount = sale.NetAmount
            };
        }
    }
}
