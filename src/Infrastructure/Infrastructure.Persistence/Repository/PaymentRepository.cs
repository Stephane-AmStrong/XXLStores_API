using Application.Features.Payments.Queries.GetPagedList;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        private ISortHelper<Payment> _sortHelper;

        public PaymentRepository
        (
            RepositoryContext repositoryContext,
            ISortHelper<Payment> sortHelper
        ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Payment>> GetPagedListAsync(GetPaymentsQuery paymentsQuery)
        {
            var payments = Enumerable.Empty<Payment>().AsQueryable();

            ApplyFilters(ref payments, paymentsQuery);

            PerformSearch(ref payments, paymentsQuery.SearchTerm);

            var sortedPayments = _sortHelper.ApplySort(payments, paymentsQuery.OrderBy);

            return await Task.Run(() =>
                PagedList<Payment>.ToPagedList
                (
                    sortedPayments,
                    paymentsQuery.PageNumber,
                    paymentsQuery.PageSize)
                );
        }


        public async Task<Payment> GetByIdAsync(Guid id)
        {
            return await BaseFindByCondition(payment => payment.Id.Equals(id))
                .FirstOrDefaultAsync();
        }


        public async Task<bool> ExistAsync(Payment payment)
        {
            return await BaseFindByCondition(x => x.MoneyAmount == payment.MoneyAmount && x.ShoppingCartId == payment.ShoppingCartId)
                .AnyAsync();
        }

        public async Task CreateAsync(Payment payment)
        {
            await BaseCreateAsync(payment);
        }

        public async Task UpdateAsync(Payment payment)
        {
            await BaseUpdateAsync(payment);
        }

        public async Task UpdateAsync(IEnumerable<Payment> payments)
        {
            await BaseUpdateAsync(payments);
        }

        public async Task DeleteAsync(Payment payment)
        {
            await BaseDeleteAsync(payment);
        }

        private void ApplyFilters(ref IQueryable<Payment> payments, GetPaymentsQuery paymentsQuery)
        {
            payments = BaseFindAll();

            /*
            if (paymentsQuery.MinCreateAt != null)
            {
                payments = payments.Where(x => x.CreateAt >= paymentsQuery.MinCreateAt);
            }

            if (paymentsQuery.MaxCreateAt != null)
            {
                payments = payments.Where(x => x.CreateAt < paymentsQuery.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Payment> payments, string searchTerm)
        {
            if (!payments.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            payments = payments.Where(x => x.ShoppingCart.Customer.Firstname.ToLower().Contains(searchTerm.Trim().ToLower()) || x.ShoppingCart.Customer.Lastname.ToLower().Contains(searchTerm.Trim().ToLower()));
        }


    }
}
