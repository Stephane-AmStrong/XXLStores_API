using Application.Features.Items.Queries.GetById;
using Domain.Common;
using System;

namespace Application.Features.InventoryLevels.Queries.GetById
{
    public record InventoryLevelViewModel : AuditableBaseEntity
    {
        public int InStock { get; set; }
        public int StockAfter { get; set; }
        public DateTime UpdateAt { get; set; }
        public Guid ItemId { get; set; }

        public virtual ItemViewModel Item { get; set; }
    }
}
