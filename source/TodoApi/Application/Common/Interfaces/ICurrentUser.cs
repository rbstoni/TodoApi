using System.Security.Claims;

namespace TodoApi.Application.Common.Interfaces
{
    public interface ICurrentUser
    {

        string? Name { get; }

        IEnumerable<Claim>? GetUserClaims();
        string? GetUserEmail();
        Guid GetUserId();
        bool IsAuthenticated();
        bool IsInRole(string role);

    }
}