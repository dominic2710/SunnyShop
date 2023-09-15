using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Entities
{
    public class TblProductInventory
    {
        public int Id { get; set; }
        public string ProductCd { get; set; }
        public int PlannedInpStock { get; set; }
        public int PlannedOutStock { get; set; }
        public int InStock { get; set; }
        public int AvailabilityInStock { get; set; }
        public int AveragePurchasePrice { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
