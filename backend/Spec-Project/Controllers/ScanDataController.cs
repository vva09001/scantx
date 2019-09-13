using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using Spec_Project.Entities;
using Spec_Project.Models;
using Spec_Project.Services;

namespace Spec_Project.Controllers
{
    [Authorize]
    [Route("api/ScanData")]
    [ApiController]
    public class ScanDataController : ControllerBase
    {
        IHttpContextAccessor _httpContextAccessor;
        private IScanDataService _IScanDataService;
        DataContext context = new DataContext();
        //private IMapper _mapper;
        //private readonly AppSettings _appSettings;
        public ScanDataController(IScanDataService scandataService)
        {
            _IScanDataService = scandataService;
            //_mapper = mapper;
            //_appSettings = appSettings.Value;
        }
        [Authorize]
        [DisableCors]
        [HttpGet("convert-scandata-to-csv")]
        public IActionResult ConvertTblScanDataToCSV(string uid)
        {
            return Ok(_IScanDataService.ConvertTblScanDataToCSV(uid));
        }

        [Authorize]
        [DisableCors]
        [HttpGet("get-scandata")]
        public IActionResult getScanData()
        {
            return Ok(_IScanDataService.getScanData());
        }

        [Authorize]
        [DisableCors]
        [HttpPost("add-scandata")]
        public IActionResult addScanData(ScanDataModel tblscandata)
        {
            return Ok(_IScanDataService.addScanData(tblscandata));
        }

        [Authorize]
        [DisableCors]
        [HttpPost("delete-scandata")]
        public IActionResult DeleteScanData(string scanid)
        {
            return Ok(_IScanDataService.DeleteScanData(scanid));
        }

        [DisableCors]
        [HttpPost("delete-arr-scandata")]
        public IActionResult DeleteArrScanData(List<string> deleteIds)
        {
            return Ok(_IScanDataService.DeleteArrScanData(deleteIds));
        }

        [Authorize]
        [DisableCors]
        [HttpPut("edit-scandata")]
        public IActionResult EditScandata(ScanDataModel tblscandata)
        {
            return Ok(_IScanDataService.EditScandata(tblscandata));
        }

        //[Authorize]
        [DisableCors]
        [HttpGet("getqr")]
        public ResponseModel CreateQR()

        {
            return Ok(_IScanDataService.CreateQR());
        }

        [DisableCors]
        [HttpGet("getfile")]
        public IActionResult MyExportAction()
        {


            var progresses = context.TblScanData.Where(p => p.DeletedOn == null).ToList().Select(progress =>
                    new TblScanData()
                    {
                        ScanId = progress.ScanId,
                        Uid = progress.Uid,
                        Status = progress.Status,
                        DataType = progress.DataType,
                        CreatedOn = progress.CreatedOn,
                        FileName = progress.FileName,
                        Payload = progress.Payload
                    }
                );

            List<TblScanData> reportCSVModels = progresses.ToList();
            using (var writer = new StreamWriter("Tmp/file.csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(reportCSVModels);
            }
            //var stream = new MemoryStream();
            //var writeFile = new StreamWriter(stream);
            //var csv = new CsvWriter(writeFile);
            ////csv.Configuration.RegisterClassMap<GroupReportCSVMap>();
            //csv.WriteRecords(reportCSVModels);
            //stream.Position = 0; //reset stream
            //return File(stream, "application/octet-stream", "Reports.csv");
            return null;
        }
        public class GroupReportCSVMap : ClassMap<ScanDataModel>
        {
            public GroupReportCSVMap()
            {
                Map(m => m.DeletedOn).TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss.fff");
            }
        }
    }
}