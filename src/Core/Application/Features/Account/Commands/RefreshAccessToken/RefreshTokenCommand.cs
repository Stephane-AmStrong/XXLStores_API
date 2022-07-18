using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Models;
using Application.Features.Account.Commands.Authenticate;

namespace Application.Features.Account.Commands.RefreshAccessToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string IpAddress { get; set; }
    }


    internal class RefreshTokenRequestCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly ILogger<RefreshTokenRequestCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public RefreshTokenRequestCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<RefreshTokenRequestCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"RefreshToken attempt with accessToken: {command.AccessToken}");
            var generateRefreshTokenModel = _mapper.Map<GenerateRefreshTokenModel>(command);
            var refreshTokenResponse = await _repository.Token.RefreshAsync(generateRefreshTokenModel, command.IpAddress);
            _logger.LogInformation($"RefreshToken succeeds");

            //var authenticationViewModel = _mapper.Map<AuthenticationViewModel>(authenticationModel);
            return refreshTokenResponse;
        }
    }
}
