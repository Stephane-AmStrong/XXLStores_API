using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.InventoryLevels.Queries.GetInventoryLevels
{
    public record InventoryLevelsViewModel : AuditableBaseEntity
    {
        public int InStock { get; set; }
        public int StockAfter { get; set; }
        public DateTime UpdateAt { get; set; }
        public Guid ItemId { get; set; }
    }
}
