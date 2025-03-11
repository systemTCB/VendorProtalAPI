using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VendorPortal.Application.Interfaces.v1;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Logging;
using static VendorPortal.Application.Models.Common.AppEnum;

namespace VendorPortal.API.Controllers.v1
{

    [ApiController]
    public class MasterDataController : ControllerBase
    {
        private readonly IMasterDataService _masterDataService;
        public MasterDataController(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        [HttpGet]
        [Route("api/v1/wolf-approve/master/company")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "MasterData" }, Summary = "", Description = "ใช้สำหรับขอข้อมูลของบริษัททั้งหมด")]
        public async Task<IActionResult> GetCompanyList(bool isShowAll = false)
        {
            var response = new MasterCompanyResponse();
            try
            {
                response = await _masterDataService.GetCompanyList(isShowAll);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetCompanyList");
                response = new MasterCompanyResponse()
                {
                    Status = new Application.Models.Common.Status()
                    {
                        Code = ResponseCode.InternalServerError.Text(),
                        Message = ResponseCode.InternalServerError.Description()
                    },
                    Data = null
                };

            }
            return Ok(response);
        }
        [HttpGet]
        [Route("api/v1/wolf-approve/master/company/{companyId}")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "MasterData" }, Summary = "", Description = "ใช้สำหรับขอข้อมูลของบริษัทตาม CompanyId")]
        public async Task<IActionResult> GetCompanyById(string companyId)
        {
            var response = new MasterCompanyByIdResponse();
            try
            {
                response = await _masterDataService.GetCompanyById(companyId);
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
            return Ok(response);
        }
    }
}