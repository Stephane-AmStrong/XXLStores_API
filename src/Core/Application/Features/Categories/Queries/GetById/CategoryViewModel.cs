using Application.Features.Items.Queries.GetPagedList;
using Domain.Common;

namespace Application.Features.Categories.Queries.GetById
{
    public record CategoryViewModel : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ItemsViewModel[] Items { get; set; }
    }
}
