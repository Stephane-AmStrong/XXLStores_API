using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Queries.GetById
{
    public class GetAppUserByIdQuery : IRequest<GetAppUserViewModel>
    {
        public string Id { get; set; }

        public class GetAppUserByIdQueryHandler : IRequestHandler<GetAppUserByIdQuery, GetAppUserViewModel>
        {
            private readonly IRepositoryWrapper _repository;
            private readonly IMapper _mapper;

            public GetAppUserByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<GetAppUserViewModel> Handle(GetAppUserByIdQuery query, CancellationToken cancellationToken)
            {
                var appUser = await _repository.AccountService.GetByIdAsync(query.Id);
                if (appUser == null) throw new ApiException($"AppUser Not Found.");
                return _mapper.Map<GetAppUserViewModel>(appUser);
            }
        }
    }
}
