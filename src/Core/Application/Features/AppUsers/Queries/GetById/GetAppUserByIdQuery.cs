using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Queries.GetById
{
    public class GetAppUserByIdQuery : IRequest<AppUserViewModel>
    {
        public string Id { get; set; }
    }

    public class GetAppUserByIdQueryHandler : IRequestHandler<GetAppUserByIdQuery, AppUserViewModel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetAppUserByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AppUserViewModel> Handle(GetAppUserByIdQuery query, CancellationToken cancellationToken)
        {
            var appUser = await _repository.AppUser.GetByIdAsync(query.Id);
            if (appUser == null) throw new ApiException($"AppUser with id: {query.Id}, hasn't been found.");
            return _mapper.Map<AppUserViewModel>(appUser);
        }
    }
}
