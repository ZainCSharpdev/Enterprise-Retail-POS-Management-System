using POSbackend.DTO.Sale;
using POSbackend.Models;
using Microsoft.EntityFrameworkCore;
using POSbackend.Repository.Interface.Sales;

namespace POSbackend.Repository.implement.Sale
{
    public class SalesDetailRepo(PosdbContext _context) : ISaleDetailRepo
    {
        public async Task<IEnumerable<SalesDetailsDto>> GetSaleDetailsAsync(int saleId)
        {
            return await _context.SaleDetails
                .Where(sd => sd.SaleId == saleId)
                .Select(sd => new SalesDetailsDto
                {
                    SaleDetailId = sd.SaleDetailId,
                    SaleId = sd.SaleId,
                    ProductId = sd.ProductId,
                    ProductName = sd.Product.ProductName,
                    Quantity = sd.Qty,
                    UnitPrice = sd.UnitPrice,
                    Price = sd.TotalPrice
                }).ToListAsync();
        }

        public async Task AddSaleDetailAsync(SalesDetailsDto detail)
        {
            var saleDetail = new SaleDetail
            {
                SaleId = detail.SaleId,
                ProductId = detail.ProductId ?? 0,
                Qty = detail.Quantity ?? 0,
                UnitPrice = detail.UnitPrice ?? 0,
                TotalPrice = (detail.Quantity ?? 0) * (detail.UnitPrice ?? 0)
            };
            _context.SaleDetails.Add(saleDetail);
            await _context.SaveChangesAsync();

            await UpdateSaleTotals(detail.SaleId);
        }

        private async Task UpdateSaleTotals(int saleId)
        {
            var sale = await _context.Sales
                .Include(s => s.SaleDetails)
                .FirstOrDefaultAsync(s => s.SaleId == saleId);

            if (sale != null)
            {
                sale.TotalAmount = sale.SaleDetails.Sum(sd => sd.TotalPrice);
                sale.NetAmount = sale.TotalAmount - (sale.Discount ?? 0) + (sale.Tax ?? 0);
                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<SalesDetailsDto?> UpdateSaleDetailAsync(SalesDetailsDto detail)
        {
            var saleDetail = await _context.SaleDetails.FindAsync(detail.SaleDetailId);
            if (saleDetail == null) return null;

            saleDetail.ProductId = detail.ProductId ?? saleDetail.ProductId;
            saleDetail.Qty = detail.Quantity ?? saleDetail.Qty;
            saleDetail.UnitPrice = detail.UnitPrice ?? saleDetail.UnitPrice;
            saleDetail.TotalPrice = (detail.Quantity ?? saleDetail.Qty) * (detail.UnitPrice ?? saleDetail.UnitPrice);

            await _context.SaveChangesAsync();
            await UpdateSaleTotals(saleDetail.SaleId);

            return detail;
        }

        public async Task<bool> DeleteSaleDetailAsync(int saleDetailId)
        {
            var existing = await _context.SaleDetails.FindAsync(saleDetailId);
            if (existing == null) return false;

            var saleId = existing.SaleId;
            _context.SaleDetails.Remove(existing);
            await _context.SaveChangesAsync();

            await UpdateSaleTotals(saleId);
            return true;
        }
    }
}
