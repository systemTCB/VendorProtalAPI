using System;
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
                            company_id = data.nCompanyID,
                            company_name = data.sCompanyName,
                            address_1 = data.sAddress1,
                            address_2 = data.sAddress2,
                            district = data.sDistrict,
                            sub_district = data.sSubDistrict,
                            province = data.sProvince,
                            zip_code = data.sZipCode,
                            branch = data.sBranch,
                            tax_number = data.sTaxNumber,
                            contract_first_name = data.sContractFirstName,
                            contract_last_name = data.sContractLastName,
                            contract_email = data.sContractEmail,
                            contract_phone = data.sContractPhone
                        };
                        response.status = new Status()
                        {
                            code = ResponseCode.Success.Text(),
                            message = ResponseCode.Success.Description()
                        };
                    }
                    else
                    {
                        response = new MasterCompanyByIdResponse()
                        {
                            status = new Status()
                            {
                                code = ResponseCode.NotFound.Text(),
                                message = ResponseCode.NotFound.Description()
                            },
                            Data = null
                        };
                    }
                }
                else
                {
                    response = new MasterCompanyByIdResponse()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.NotFound.Text(),
                            message = ResponseCode.NotFound.Description()
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
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
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
                        company_id = s.nCompanyID,
                        company_name = s.sCompanyName,
                        address_1 = s.sAddress1,
                        address_2 = s.sAddress2,
                        district = s.sDistrict,
                        sub_district = s.sSubDistrict,
                        province = s.sProvince,
                        zip_code = s.sZipCode,
                        branch = s.sBranch,
                        tax_number = s.sTaxNumber,
                        contract_first_name = s.sContractFirstName,
                        contract_last_name = s.sContractLastName,
                        contract_email = s.sContractEmail,
                        contract_phone = s.sContractPhone,
                        IsActive = s.isActive,
                        createdBy = s.CreatedBy,
                        createddate = s.CreatedDate,
                        modifiedBy = s.ModifiedBy,
                        modifieddate = s.ModifiedDate
                    }).ToList()];
                    response.status = new Status()
                    {
                        code = ResponseCode.Success.Text(),
                        message = ResponseCode.Success.Description()
                    };
                }
                else
                {
                    response = new MasterCompanyResponse()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.NotFound.Text(),
                            message = ResponseCode.NotFound.Description()
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
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    Data = null
                };
            }
            return response;
        }

        public async Task<MasterCompanySyncUpdateResponse> GetCompanySyncUpdate(DateTime lastSyncDate)
        {
            MasterCompanySyncUpdateResponse response = new MasterCompanySyncUpdateResponse();
            try
            {
                // if (string.IsNullOrEmpty(lastSyncDate))
                if (lastSyncDate == DateTime.MinValue)
                {
                    response = new MasterCompanySyncUpdateResponse()
                    {
                        status = new Application.Models.Common.Status()
                        {
                            code = ResponseCode.BadRequest.Text(),
                            message = ResponseCode.BadRequest.Description()
                        },
                        data = null
                    };
                    return response;
                }
                else if (lastSyncDate == DateTime.MaxValue)
                {
                    response = new MasterCompanySyncUpdateResponse()
                    {
                        status = new Application.Models.Common.Status()
                        {
                            code = ResponseCode.BadRequest.Text(),
                            message = ResponseCode.BadRequest.Description()
                        },
                        data = null
                    };
                    return response;
                }
                else
                {
                    var result = await _masterDataRepository.SP_GET_MASTER_COMPANY(true);
                    if (result.Count != 0 && result.Any())
                    {
                        var data = result.Where(e => e.ModifiedDate >= lastSyncDate).ToList();
                        if (data.Count != 0 && data.Any())
                        {
                            response.data = [.. data.Select(s => new MasterCompanyData
                                {
                                    company_id = s.nCompanyID,
                                    company_name = s.sCompanyName,
                                    address_1 = s.sAddress1,
                                    address_2 = s.sAddress2,
                                    district = s.sDistrict,
                                    sub_district = s.sSubDistrict,
                                    province = s.sProvince,
                                    zip_code = s.sZipCode,
                                    branch = s.sBranch,
                                    tax_number = s.sTaxNumber,
                                    contract_first_name = s.sContractFirstName,
                                    contract_last_name = s.sContractLastName,
                                    contract_email = s.sContractEmail,
                                    contract_phone = s.sContractPhone,
                                    IsActive = s.isActive,
                                    createdBy = s.CreatedBy,
                                    createddate = s.CreatedDate,
                                    modifiedBy = s.ModifiedBy,
                                    modifieddate = s.ModifiedDate
                                }).ToList()];
                            response.status = new Status()
                            {
                                code = ResponseCode.Success.Text(),
                                message = ResponseCode.Success.Description()
                            };
                        }
                        else
                        {
                            response.data.Clear();
                            response.status = new Status()
                            {
                                code = ResponseCode.NotFound.Text(),
                                message = ResponseCode.NotFound.Description()
                            };
                        }
                    }
                    else
                    {
                        response.data.Clear();
                        response.status = new Status()
                        {
                            code = ResponseCode.NotFound.Text(),
                            message = ResponseCode.NotFound.Description()
                        };
                    }

                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetCompanySyncUpdate");
                response = new MasterCompanySyncUpdateResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    data = null
                };
            }
            return response;
        }
    }
}