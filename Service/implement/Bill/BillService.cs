using Dropbox.Api;
using Dropbox.Api.Files;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using POSbackend.DTO.Bill;
using POSbackend.DTO.Sale;
using POSbackend.Models;
using POSbackend.Repository.Interface.Bill;
using POSbackend.Service.Interface.Bill;
using System.Globalization;

namespace POSbackend.Service.implement.Bill
{
    public class BillService(PosdbContext _context, DropboxClient _dropbox, IBillRepo _billRepo) : IBillService
    {
        public async Task<BillDto?> CreateBillAsync(int saleId)
        {
            var saleHeader = await _context.Sales
                 .FirstOrDefaultAsync(s => s.SaleId == saleId);

            if(saleHeader == null) return null;

            var saleDetails = await _context.SaleDetails
                .Where(sd => sd.SaleId == saleId)
                .Include(sd => sd.Product)
                .ToListAsync();

            var billDto = new BillDto
            {
                SaleId = saleId,
                CustomerNumber = long.Parse(saleHeader.CustomerNumber),
                Amount = saleHeader.NetAmount,
                Status = "Completed",
                InsertedDate = DateTime.Now
            };

            byte[] pdf = GenerateBillPdf(billDto, saleHeader, saleDetails);

            var fileName = $"bill_sale_{saleId}_{DateTime.Now:yyyyMMddHHmss}.pdf";
            using (var memStreamed = new MemoryStream(pdf))
            {
                var uploadResult = await _dropbox.Files.UploadAsync(
                    $"/Bills/{fileName}",
                    WriteMode.Overwrite.Instance,
                    body: memStreamed);

                var link = await _dropbox.Sharing.CreateSharedLinkWithSettingsAsync(uploadResult.PathLower);
                billDto.PdfUrl = link.Url;
                billDto.BillImage = $"{link.Url}?raw=1";
            }

            var newBill = new Models.Bill
            {
                SaleId = saleId,
                BillDate = DateTime.Now,
                Amount = saleHeader.TotalAmount,
                PdfUrl = billDto.PdfUrl,
                BillImage = billDto.BillImage,
                Status = "Paid",
                InsertedDate = DateTime.Now,
                CustomerNumber = long.Parse(saleHeader.CustomerNumber)
            };

            
            return await _billRepo.CreateBillAsync(newBill);
        }

        private byte[] GenerateBillPdf(BillDto bill, Models.Sale saleHeader,List<SaleDetail> saleDetails)
        {
            using (var ms = new MemoryStream())
            {
                var doc = new Document(PageSize.A4);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Title
                doc.Add(new Paragraph("Invoice / Bill"));
                doc.Add(new Paragraph($"Sale ID: {bill.SaleId}"));
                doc.Add(new Paragraph($"Customer: {bill.CustomerNumber}"));
                doc.Add(new Paragraph($"Date: {DateTime.Now:dd-MMM-yyyy}"));
                doc.Add(new Paragraph(" "));

                // Table
                var table = new PdfPTable(4);
                table.AddCell(new Phrase("Product"));
                table.AddCell(new Phrase("Qty"));
                table.AddCell(new Phrase("Unit Price"));
                table.AddCell(new Phrase("Total Price"));

                foreach (var sd in saleDetails)
                {
                    table.AddCell(new Phrase(sd.Product.ProductName));
                    table.AddCell(new Phrase(sd.Qty.ToString()));
                    table.AddCell(new Phrase(
                        sd.UnitPrice.ToString("C", CultureInfo.CreateSpecificCulture("en-IN"))));
                    table.AddCell(new Phrase(
                        sd.TotalPrice.ToString("C", CultureInfo.CreateSpecificCulture("en-IN"))));
                }

                doc.Add(table);
                doc.Add(new Paragraph(" "));

                // Grand total
                doc.Add(new Paragraph(
                    $"Grand Total : {saleHeader.TotalAmount.ToString("C", CultureInfo.CreateSpecificCulture("en-IN"))}"));

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
