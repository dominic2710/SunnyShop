using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SellManagement.Api.Entities
{
    public class SellManagementContext:DbContext
    {
        public SellManagementContext(DbContextOptions<SellManagementContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<TblUser> TblUsers { get; set; }
        public virtual DbSet<TblProduct> TblProducts { get; set; }
        public virtual DbSet<TblProductCombo> TblProductCombos { get; set; }
        public virtual DbSet<TblClassifyName> TblClassifiesName { get; set; }
        public virtual DbSet<TblCustomer> TblCustomers { get; set; }
        public virtual DbSet<TblSupplier> TblSuppliers { get; set; }
        public virtual DbSet<TblShippingCompany> TblShippingCompanys { get; set; }
        public virtual DbSet<TblPurchaseOrderHead> TblPurchaseOrderHeads { get; set; }
        public virtual DbSet<TblPurchaseOrderDetail> TblPurchaseOrderDetails { get; set; }
        public virtual DbSet<TblSellOrderHead> TblSellOrderHeads { get; set; }
        public virtual DbSet<TblSellOrderDetail> TblSellOrderDetails { get; set; }
        public virtual DbSet<TblVoucherNoManagement> TblVoucherNoManagements { get; set; }
        public virtual DbSet<TblProductInventory> TblProductInventories { get; set; }
        //public virtual DbSet<TblOperationLog> TblOperationLogs { get; set; }
    }
}
