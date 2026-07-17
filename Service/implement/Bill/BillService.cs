using Dropbox.Api;
using Dropbox.Api.Files;
using iTextSharp.text;
using iTextSharp.text.pdf;
using POSbackend.DTO.Bill;
using POSbackend.Models;
using POSbackend.Repository.Interface.Bill;
using POSbackend.Service.Interface.Bill;

namespace POSbackend.Service.implement.Bill
{
    public class BillService(PosdbContext _context, DropboxClient _dropbox, IBillRepo _billRepo) : IBillService
    {
        public async Task<BillDto?> CreateBillAsync(BillDto bill)
        {
            byte[] pdf = GenerateBillPdf(bill);

            var fileName = $"bill_{bill.SaleId}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            using (var memStream = new MemoryStream(pdf))
            {
                var uploadResult = await _dropbox.Files.UploadAsync(
                    $"/Bills/{fileName}",
                    WriteMode.Overwrite.Instance,
                    body: memStream);

                var link = await _dropbox.Sharing.CreateSharedLinkWithSettingsAsync(uploadResult.PathLower);
                bill.PdfUrl = link.Url;
                bill.BillImage = $"{link.Url}?raw=1"; // preview image
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

        private byte[] GenerateBillPdf(BillDto bill)
        {
            using (var ms = new MemoryStream())
            {
                var doc = new Document(PageSize.A4);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                doc.Add(new Paragraph("Invoice / Bill", titleFont));
                doc.Add(new Paragraph($"Bill ID: {bill.BillId}"));
                doc.Add(new Paragraph($"Sale ID: {bill.SaleId}"));
                doc.Add(new Paragraph($"Customer: {bill.Name} ({bill.Email})"));
                doc.Add(new Paragraph($"Date: {DateTime.Now:dd-MMM-yyyy}"));
                doc.Add(new Paragraph($"Amount: {bill.Amount:C}"));
                doc.Add(new Paragraph($"Status: {bill.Status}"));

                doc.Close();
                return ms.ToArray();
            }
        }

        public async Task<IEnumerable<BillDto>> GetBillsBySaleAsync(int saleId)
        {
            return await _billRepo.GetBillsBySaleAsync(saleId);
        }

        public async Task<BillDto?> UpdateBillStatusAsync(int billId, string status)
        {
            return await _billRepo.UpdateBillStatusAsync(billId, status);
        }
    }
}
