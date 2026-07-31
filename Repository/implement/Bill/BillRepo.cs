using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.EntityFrameworkCore;
using POSbackend.DTO.Bill;
using POSbackend.Models;
using POSbackend.Repository.Interface.Bill;
using static Dropbox.Api.Riviera.FileIdOrUrl;

namespace POSbackend.Repository.implement.Bill
{
    public class BillRepo(PosdbContext _context) : IBillRepo
    {
        public async Task<BillDto?> CreateBillAsync(Models.Bill bill)
        {
            await _context.Bills.AddAsync(bill);
            await _context.SaveChangesAsync();

            return new BillDto 
            {
                BillId = bill.BillId,
                SaleId = bill.SaleId ?? 0,
                PdfUrl = bill.PdfUrl,
                BillImage = bill.BillImage,
                InsertedDate = bill.InsertedDate ?? DateTime.Now,
                Status = bill.Status!,
                Amount = bill.Amount ?? 0,
                CustomerNumber = bill.CustomerNumber ?? 0,
            };
        }

        public async Task<IEnumerable<BillDto>> GetAllAsync()
        {
            return await _context.Bills
                .Select(b => new BillDto
                {
                    SaleId = (int)b.SaleId,
                    PdfUrl = b.PdfUrl,
                    Status = b.Status,
                    Amount = b.Amount ?? 0,
                    InsertedDate = b.InsertedDate ?? DateTime.Now,
                    CustomerNumber = b.CustomerNumber ?? 0
                }).ToListAsync();
        }

        public async Task<IEnumerable<BillDto>> GetBillsBySaleAsync(int saleId)
        {
            return await _context.Bills
                .Where(b => b.SaleId == saleId)
                .Select(b => new BillDto
                {
                    BillId = b.BillId,
                    SaleId = (int)b.SaleId,
                    PdfUrl = b.PdfUrl,
                    BillImage = b.BillImage,
                    InsertedDate = (DateTime)b.InsertedDate,
                    CustomerNumber = (long)b.CustomerNumber
                })
                .ToListAsync();
        }

        public async Task<BillDto?> UpdateBillStatusAsync(int billId, string status)
        {
            var bill = await _context.Bills.FindAsync(billId);
            if (bill == null) return null;

            bill.Status = status;
            await _context.SaveChangesAsync();

            return new BillDto
            {
                BillId = bill.BillId,
                SaleId = (int)bill.SaleId,
                PdfUrl = bill.PdfUrl,
                BillImage = bill.BillImage,
                InsertedDate = (DateTime)bill.InsertedDate,
                CustomerNumber = (long)bill.CustomerNumber
            };
        }

        public async Task<bool> VoidBillAsync(int billId)
        {
            var bill = await _context.Bills.FindAsync(billId);
            if (bill == null) return false;

            bill.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }

    }

}