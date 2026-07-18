using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.EntityFrameworkCore;
using POSbackend.DTO.Bill;
using POSbackend.Models;
using POSbackend.Repository.Interface.Bill;

namespace POSbackend.Repository.implement.Bill
{
    public class BillRepo(PosdbContext _context, DropboxClient _dropbox) : IBillRepo
    {
        public async Task<BillDto?> CreateBillAsync(BillDto bill)
        {
            var pdfBytes = GenerateBillPdf(bill);

            // 2. Upload to Dropbox
            var fileName = $"bill_{bill.SaleId}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            using (var memStream = new MemoryStream(pdfBytes))
            {
                var uploadResult = await _dropbox.Files.UploadAsync(
                    $"/Bills/{fileName}",
                    WriteMode.Overwrite.Instance,
                    body: memStream);

                var link = await _dropbox.Sharing.CreateSharedLinkWithSettingsAsync(uploadResult.PathLower);
                bill.PdfUrl = link.Url;
                bill.BillImage = $"{link.Url}?raw=1"; // preview image if needed
            }

            // 3. Save in DB
            var newBill = new Models.Bill
            {
                SaleId = bill.SaleId,
                BillDate = DateTime.Now,
                Amount = bill.Amount,
                PdfUrl = bill.PdfUrl,
                BillImage = bill.BillImage,
                Status = "Open",
                InsertedDate = DateTime.Now,
                Email = bill.Email,
                Name = bill.Name
            };

            await _context.Bills.AddAsync(newBill);
            await _context.SaveChangesAsync();

            bill.BillId = newBill.BillId;
            return bill;
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
                    Status = b.Status,
                    Amount = (decimal)b.Amount,
                    Email = b.Email,
                    Name = b.Name
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
                Status = bill.Status,
                Amount = (decimal)bill.Amount,
                Email = bill.Email,
                Name = bill.Name
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