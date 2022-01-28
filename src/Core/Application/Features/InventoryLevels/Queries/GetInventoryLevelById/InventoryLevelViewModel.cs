using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.InventoryLevels.Queries.GetInventoryLevelById
{
    public class InventoryLevelViewModel
    {
        public virtual Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<InventoryLevelViewModel> Events { get; set; }
    }
}
