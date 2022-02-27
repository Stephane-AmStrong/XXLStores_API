using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Payments.Queries.GetPagedList
{
    public class GetPaymentsQuery : QueryParameters, IRequest<PagedList<PaymentsViewModel>>
    {
        public GetPaymentsQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    internal class GetPaymentsQueryHandler : IRequestHandler<GetPaymentsQuery, PagedList<PaymentsViewModel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetPaymentsQueryHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<PagedList<PaymentsViewModel>> Handle(GetPaymentsQuery query, CancellationToken cancellationToken)
        {
            var payments = await _repository.Payment.GetPagedListAsync(query);
            return  _mapper.Map<PagedList<PaymentsViewModel>>(payments);
        }
    }
}
