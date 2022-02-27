using Application.Features.Payments.Queries.GetPagedList;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task<PagedList<Payment>> GetPagedListAsync(GetPaymentsQuery getPaymentsQuery);

        Task<Payment> GetByIdAsync(Guid id);
        Task<bool> ExistAsync(Payment payment);

        Task CreateAsync(Payment payment);
        Task UpdateAsync(Payment payment);
        Task UpdateAsync(IEnumerable<Payment> events);
        Task DeleteAsync(Payment payment);
    }
}
