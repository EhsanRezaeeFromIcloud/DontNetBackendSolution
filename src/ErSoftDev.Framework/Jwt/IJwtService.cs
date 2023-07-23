using ErSoftDev.DomainSeedWork;

namespace ErSoftDev.Framework.Jwt
{
    public interface IJwtService
    {
        Task<ApiResult<TokenResponse>> Generate(TokenRequest tokenRequest);
    }
}