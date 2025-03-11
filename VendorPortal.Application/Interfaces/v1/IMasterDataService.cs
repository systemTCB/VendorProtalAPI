using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using VendorPortal.Application.Models.v1.Response;

namespace VendorPortal.Application.Interfaces.v1
{
    public interface IMasterDataService
    {
        Task<MasterCompanyResponse> GetCompanyList(bool isShowAll);
        Task<MasterCompanyByIdResponse> GetCompanyById(string companyId);
    }
}