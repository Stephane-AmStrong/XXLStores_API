using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Queries.GetPagedList
{
    public class GetAppUsersQuery : QueryParameters, IRequest<PagedListResponse<GetAppUsersViewModel>>
    {
        public GetAppUsersQuery()
        {
            OrderBy = "name";
        }

        public string HavingTheName { get; set; }
    }

    public class GetAppUsersQueryHandler : IRequestHandler<GetAppUsersQuery, PagedListResponse<GetAppUsersViewModel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public GetAppUsersQueryHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<PagedListResponse<GetAppUsersViewModel>> Handle(GetAppUsersQuery query, CancellationToken cancellationToken)
        {
            var appUsers = await _repository.AppUser.GetPagedListAsync(query);
            var appUsersViewModel = _mapper.Map<List<GetAppUsersViewModel>>(appUsers);
            return new PagedListResponse<GetAppUsersViewModel>(appUsersViewModel, appUsers.MetaData);
        }
    }
}
