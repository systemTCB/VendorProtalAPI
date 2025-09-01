using System.Threading.Tasks;
using VendorPortal.Application.Models.v1.Response;

namespace VendorPortal.Application.Interfaces.v1
{
    public interface IAuthenticationService
    {
        // Define your method signatures here
        Task<AuthenticationResponse> AuthenticateToken(string token, string channel);
        Task<bool> CheckConnection();
    }
}