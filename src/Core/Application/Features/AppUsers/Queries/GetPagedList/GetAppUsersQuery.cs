using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Queries.GetPagedList
{
    public class GetAppUsersQuery : QueryParameters, IRequest<PagedList<GetAppUsersViewModel>>
    {
        public GetAppUsersQuery()
        {
            OrderBy = "name";
        }

        public string HavingTheName { get; set; }
    }

    public class GetAppUsersQueryHandler : IRequestHandler<GetAppUsersQuery, PagedList<GetAppUsersViewModel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public GetAppUsersQueryHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<PagedList<GetAppUsersViewModel>> Handle(GetAppUsersQuery query, CancellationToken cancellationToken)
        {
            var appUsers = await _repository.AppUser.GetPagedListAsync(query);
            return  _mapper.Map<PagedList<GetAppUsersViewModel>>(appUsers);
        }
    }
}
