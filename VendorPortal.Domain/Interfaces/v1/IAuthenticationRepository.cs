using System.Threading.Tasks;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;

namespace VendorPortal.Domain.Interfaces.v1
{
    public interface IAuthenticationRepository
    {
        // Define method signatures for authentication repository
        Task<SP_AUTHENTICATE_CHANNEL> SP_AUTHENTICATE_CHANNEL(string token, string channel);
        Task<bool> CHECK_CONNECTION();

    }
}