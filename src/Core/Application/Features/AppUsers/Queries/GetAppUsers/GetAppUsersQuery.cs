using Application.Interfaces;
using Application.Parameters;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Queries.GetAppUsers
{
    public class GetAppUsersQuery : QueryStringParameters, IRequest<PagedList<GetAppUsersViewModel>>
    {
        public GetAppUsersQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    public class GetAllAppUsersQueryHandler : IRequestHandler<GetAppUsersQuery, PagedList<GetAppUsersViewModel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetAllAppUsersQueryHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<PagedList<GetAppUsersViewModel>> Handle(GetAppUsersQuery query, CancellationToken cancellationToken)
        {
            var appUsers = await _repository.AccountService.GetPagedListAsync(query);
            return  _mapper.Map<PagedList<GetAppUsersViewModel>>(appUsers);
        }
    }
}
