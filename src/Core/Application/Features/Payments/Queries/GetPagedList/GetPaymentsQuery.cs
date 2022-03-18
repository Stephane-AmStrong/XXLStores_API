using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Payments.Queries.GetPagedList
{
    public class GetPaymentsQuery : QueryParameters, IRequest<PagedListResponse<PaymentsViewModel>>
    {
        public GetPaymentsQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    internal class GetPaymentsQueryHandler : IRequestHandler<GetPaymentsQuery, PagedListResponse<PaymentsViewModel>>
    {
        private readonly ILogger<GetPaymentsQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetPaymentsQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetPaymentsQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedListResponse<PaymentsViewModel>> Handle(GetPaymentsQuery query, CancellationToken cancellationToken)
        {
            var payments = await _repository.Payment.GetPagedListAsync(query);
            var paymentsViewModel = _mapper.Map<List<PaymentsViewModel>>(payments);
            _logger.LogInformation($"Returned Paged List of Payments from database.");
            return new PagedListResponse<PaymentsViewModel>(paymentsViewModel, payments.MetaData);
        }
    }
}
