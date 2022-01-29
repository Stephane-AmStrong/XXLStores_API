using Domain.Common;
using System;

namespace Application.Features.Payments.Queries.GetPagedList
{
    public record PaymentsViewModel : AuditableBaseEntity
    {
        public int MoneyAmount { get; set; }
        public DateTime? PaidAt { get; set; }
        public Guid ShoppingCartId { get; set; }
    }
}
