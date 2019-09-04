using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lucene.Net.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Spec_Project.Entities;
using Spec_Project.Services;

namespace Spec_Project.Controllers
{

    [Route("api/Company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private ICompanyService _ICompanyService;
        //private IMapper _mapper;
        //private readonly AppSettings _appSettings;
        public CompanyController(
        ICompanyService companyService)
        {
            _ICompanyService = companyService;
            //_mapper = mapper;
            //_appSettings = appSettings.Value;
        }

        [DisableCors]
        [HttpGet("get-company")]
        public IActionResult getCompany()
        {
            return Ok(_ICompanyService.getCompany());
        }

        [DisableCors]
        [HttpPost("add-company")]
        public IActionResult addCompany(TblCustomer tblcustomer)
        {
            return Ok(_ICompanyService.addCompany(tblcustomer));
        }

        [DisableCors]
        [HttpPost("delete-company")]
        public IActionResult DeleteCompany(string cid)
        {
            return Ok(_ICompanyService.DeleteCompany(cid));
        }
      
        [DisableCors]
        [HttpPost("delete-arr-company")]
        public IActionResult DeleteArrCompany(List<string> deleteIds)
        {
            return Ok(_ICompanyService.DeleteArrCompany(deleteIds));
        }

        [DisableCors]
        [HttpPut("edit-company")]
        public IActionResult EditCompany(TblCustomer customer)
        {
            return Ok(_ICompanyService.EditCompany(customer));
        }
    }
}