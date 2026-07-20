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
                    $"Bills/{fileName}",
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
                Status = "Open",
                InsertedDate = DateTime.Now,
                CustomerNumber = long.Parse(saleHeader.CustomerNumber)
            };

            await _context.Bills.AddAsync(newBill);
            await _context.SaveChangesAsync();

            billDto.BillId = newBill.BillId;
            return billDto;
        }

        private byte[] GenerateBillPdf(BillDto bill, Models.Sale saleHeader,List<SaleDetail> saleDetails)
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
                doc.Add(new Paragraph($"Customer: {bill.CustomerNumber})"));
                doc.Add(new Paragraph($"Date: {DateTime.Now:dd-MMM-yyyy}"));
                doc.Add(new Paragraph(" "));

                var table = new PdfPTable(4);
                table.AddCell("Product");
                table.AddCell("Qty");
                table.AddCell("Unit Price");
                table.AddCell("Total Price");

                foreach(var sd in saleDetails)
                {
                    table.AddCell(sd.Product.ProductName);
                    table.AddCell(sd.Qty.ToString());
                    table.AddCell(sd.UnitPrice.ToString("C"));
                    table.AddCell(sd.TotalPrice.ToString("C"));
                }

                doc.Add(table);
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph($"Grand Total : {saleHeader.TotalAmount:C}", titleFont));

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
