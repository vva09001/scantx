using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lucene.Net.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QRCoder;
using Spec_Project.Entities;
using Spec_Project.Models;
using Spec_Project.Services;

namespace Spec_Project.Controllers
{

    [Route("api/ScanData")]
    [ApiController]
    public class ScanDataController : ControllerBase
    {
        private IScanDataService _IScanDataService;
        //private IMapper _mapper;
        //private readonly AppSettings _appSettings;
        public ScanDataController(
        IScanDataService scandataService)
        {
            _IScanDataService = scandataService;
            //_mapper = mapper;
            //_appSettings = appSettings.Value;
        }

        [DisableCors]
        [HttpGet("get-scandata")]
        public IActionResult getScanData()
        {
            return Ok(_IScanDataService.getScanData());
        }

        [DisableCors]
        [HttpPost("add-scandata")]
        public IActionResult addScanData(TblScanData tblscandata)
        {
            return Ok(_IScanDataService.addScanData(tblscandata));
        }

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

        [DisableCors]
        [HttpPut("edit-scandata")]
        public IActionResult EditScandata(TblScanData tblscandata)
        {
            return Ok(_IScanDataService.EditScandata(tblscandata));
        }

        [DisableCors]
        [HttpPost("getqr")]
        public ResponseModel CreateQR()
        {
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            try
            {
                DataContext db = new DataContext();
                CreateQR createqr = new CreateQR();
                var textboxQR = (createqr.Command + createqr.ServerAddress + createqr.Port + createqr.URLPart + createqr.EncryptionKey + createqr.User + createqr.Password);
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(textboxQR.ToString(), QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                var bitmapBytes = BitmapToBytes(qrCodeImage); //Convert bitmap into a byte array
                string base64String = Convert.ToBase64String(bitmapBytes);
                res.Data = base64String; //tra data kieu responsemodel
                return res;
            }
            catch (Exception ex)
            {
                res.Status = "500";
                res.Message = ex.Message;
            }

            return res;
        }
        // This method is for converting bitmap into a byte array
        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}