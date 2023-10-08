namespace SellManagement.Api.Entities
{
    public class TblProduct
    {
        public int Id { get; set; }
        public string ProductCd { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public int CategoryCd { get; set; }
        public int TradeMarkCd { get; set; }
        public int OriginCd { get; set; }
        public int CostPrice { get; set; }
        public int SoldPrice { get; set; }
        public string Detail { get; set; }
        public string SapoProductId { get; set; }

    }
}
