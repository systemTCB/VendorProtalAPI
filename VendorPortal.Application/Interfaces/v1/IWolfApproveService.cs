using System.Threading.Tasks;
using VendorPortal.Application.Models.v1.Response;

namespace VendorPortal.Application.Interfaces.v1
{
    public interface IWolfApproveService
    {
        Task<RFQResponse> GetRFQ_List();
        Task<RFQShowResponse> GetRFQ_Show(string rfq_id);
    }
}