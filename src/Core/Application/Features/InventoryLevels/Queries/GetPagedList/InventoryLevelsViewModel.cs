using Domain.Common;
using System;

namespace Application.Features.InventoryLevels.Queries.GetPagedList
{
    public record InventoryLevelsViewModel : AuditableBaseEntity
    {
        public int InStock { get; set; }
        public int StockAfter { get; set; }
        public DateTime UpdateAt { get; set; }
        public Guid ItemId { get; set; }
    }
}
