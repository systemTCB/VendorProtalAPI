using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using VendorPortal.Application.Helpers;
using VendorPortal.Application.Interfaces;
using VendorPortal.Application.Interfaces.v1;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Infrastructure.Extensions;
using VendorPortal.Logging;
using static VendorPortal.Application.Models.Common.AppEnum;

namespace VendorPortal.Application.Services.v1
{
    public class MasterDataService : IMasterDataService
    {
        private readonly AppConfigHelper _appConfigHelper;
        private readonly IMasterDataRepository _masterDataRepository;

        public MasterDataService(AppConfigHelper appConfigHelper, IMasterDataRepository masterDataRepository)
        {
            _appConfigHelper = appConfigHelper;
            _masterDataRepository = masterDataRepository;
        }

        public async Task<MasterCompanyByIdResponse> GetCompanyById(string companyId)
        {
            MasterCompanyByIdResponse response = new MasterCompanyByIdResponse();
            try
            {
                //Default Only Active Company 
                var result = await _masterDataRepository.SP_GET_MASTER_COMPANY(true);
                if (result != null)
                {
                    var nCompanyID = int.Parse(companyId);
                    var data = result.Where(e => e.nCompanyID == nCompanyID).FirstOrDefault();
                    if (data != null)
                    {
                        response.Data = new MasterCompanyByIdData()
                        {
                            Company_id = data.nCompanyID,
                            Company_name = data.sCompanyName,
                            Address_1 = data.sAddress1,
                            Address_2 = data.sAddress2,
                            District = data.sDistrict,
                            Province = data.sProvince,
                            Zip_Code = data.sZipCode,
                            Branch = data.sBranch,
                            Tax_number = data.sTaxNumber,
                            Contract_first_name = data.sContractFirstName,
                            Contract_last_name = data.sContractLastName,
                            Contract_Email = data.sContractEmail,
                            Contract_Phone = data.sContractPhone
                        };
                        response.Status = new Status()
                        {
                            Code = ResponseCode.Success.Text(),
                            Message = ResponseCode.Success.Description()
                        };
                    }
                    else
                    {
                        response = new MasterCompanyByIdResponse()
                        {
                            Status = new Status()
                            {
                                Code = ResponseCode.NotFound.Text(),
                                Message = ResponseCode.NotFound.Description()
                            },
                            Data = null
                        };
                    }
                }
                else
                {
                    response = new MasterCompanyByIdResponse()
                    {
                        Status = new Status()
                        {
                            Code = ResponseCode.NotFound.Text(),
                            Message = ResponseCode.NotFound.Description()
                        },
                        Data = null
                    };
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetCompanyById");
                response = new MasterCompanyByIdResponse()
                {
                    Status = new Application.Models.Common.Status()
                    {
                        Code = ResponseCode.InternalServerError.Text(),
                        Message = ResponseCode.InternalServerError.Description()
                    },
                    Data = null
                };
            }
            return response;
        }

        public async Task<MasterCompanyResponse> GetCompanyList(bool isShowAll)
        {
            var response = new MasterCompanyResponse();
            try
            {
                var result = await _masterDataRepository.SP_GET_MASTER_COMPANY(isShowAll);
                if (result.Count != 0 && result.Any())
                {
                    response.Data = [.. result.Select(s=> new MasterCompanyData
                    {
                        Company_id = s.nCompanyID,
                        Company_name = s.sCompanyName,
                        Address_1 = s.sAddress1,
                        Address_2 = s.sAddress2,
                        District = s.sDistrict,
                        Province = s.sProvince,
                        Zip_Code = s.sZipCode,
                        Branch = s.sBranch,
                        Tax_number = s.sTaxNumber,
                        Contract_first_name = s.sContractFirstName,
                        Contract_last_name = s.sContractLastName,
                        Contract_Email = s.sContractEmail,
                        Contract_Phone = s.sContractPhone,
                        IsActive = s.isActive,
                        CreatedBy = s.CreatedBy,
                        CreatedDate = s.CreatedDate,
                        ModifiedBy = s.ModifiedBy,
                        ModifiedDate = s.ModifiedDate
                    }).ToList()];
                    response.Status = new Status()
                    {
                        Code = ResponseCode.Success.Text(),
                        Message = ResponseCode.Success.Description()
                    };
                }
                else
                {
                    response = new MasterCompanyResponse()
                    {
                        Status = new Status()
                        {
                            Code = ResponseCode.NotFound.Text(),
                            Message = ResponseCode.NotFound.Description()
                        },
                        Data = null
                    };
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetCompanyList");
                response = new MasterCompanyResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.InternalServerError.Text(),
                        Message = ResponseCode.InternalServerError.Description()
                    },
                    Data = null
                };
            }
            return response;
        }
    }
}