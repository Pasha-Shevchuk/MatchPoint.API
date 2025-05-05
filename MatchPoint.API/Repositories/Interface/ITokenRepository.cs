using Microsoft.AspNetCore.Identity;

namespace MatchPoint.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> rolse);
    }
}
