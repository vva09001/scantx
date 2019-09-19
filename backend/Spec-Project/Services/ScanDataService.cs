using Spec_Project.Models;
using System;
using Spec_Project.Entities;
using System.Linq;
using System.Collections.Generic;
using QRCoder;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using CsvHelper;

using System.Data.SqlClient;
using LINQtoCSV;
using Microsoft.EntityFrameworkCore;


namespace Spec_Project.Services
{
    public interface IScanDataService
    {
        ResponseModel getScanData();
        ResponseModel addScanData(ScanDataModel tblscandata);
        ResponseModel DeleteScanData(string scanid);
        ResponseModel DeleteArrScanData(List<string> deleteIds);

        ResponseModel EditScandata(ScanDataModel tblscandata);
        ResponseModel CreateQR();

        ResponseModel ConvertTblScanDataToCSV(int Uid);

    }
    public class ScanDataService : IScanDataService
    {
        DataContext context = new DataContext();
        IHttpContextAccessor _httpContextAccessor;
        public ScanDataService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ResponseModel ConvertTblScanDataToCSV(int Uid)
        {

            var res = new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            };
            string mypath = @"h:\root\tmp\";
            string mypath1 = @"D:\Git\scantx\spec-project\scantx\backend\Spec-Project\";
            try
            {

                DataContext db = new DataContext();

                int role = UsersConstant.GetRole(Uid);

                List<ScanDataModel> datascan = new List<ScanDataModel>();
                var context = _httpContextAccessor.HttpContext;
                if (UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.superadmin)
                {
                    datascan.Clear();
                    datascan = db.TblScanData.Select(o => new ScanDataModel
                    {
                        Uid = o.Uid,
                        CreatedOn = new DateTime(o.CreatedOn.Value.Ticks).ToString("o"),
                        DataType = o.DataType,
                        FileName = o.FileName,
                        Payload = o.Payload,
                        ScanId = o.ScanId,
                        Status = o.Status
                    }).ToList();

                }
                else
                if (UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.reader)
                {

                    return res = new ResponseModel
                    {
                        Data = "",
                        Status = "500",
                        Message = "Reader does not have permission to use this feature."
                    };

                }
                else
                if (UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.admin || UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.user)
                {
                    datascan.Clear();
                    datascan = db.TblScanData.Where(p => p.Uid == int.Parse(context.User.Identity.Name) && p.DeletedOn == null).Select(o => new ScanDataModel
                    {
                        Uid = o.Uid,
                        CreatedOn = new DateTime(o.CreatedOn.Value.Ticks).ToString("o"),
                        DataType = o.DataType,
                        FileName = o.FileName,
                        Payload = o.Payload,
                        ScanId = o.ScanId,
                        Status = o.Status
                    }).ToList();

                }

                CsvFileDescription outputFileDescription = new CsvFileDescription
                {
                    SeparatorChar = ',',
                    FirstLineHasColumnNames = true,
                    FileCultureName = "en-US"
                };
                CsvContext cc = new CsvContext();

                var now = DateTime.Now.ToString("yyyyMMddhhmmssfff");

                string finalPath = Directory.GetCurrentDirectory() + "\\tmp\\scandata_" + now + ".csv";
                cc.Write(datascan, finalPath, outputFileDescription);
                res.Data = "scantx.miracles.vn/mycsv/scandata_" + now + ".csv";
                res.Message = "";
                res.Status = "200";

            }
            catch (Exception ex)
            {
                res.Data = "";
                res.Message = ex.Message;
                res.Status = "500";
            }
            return res;
        }

        public ResponseModel getScanData()
        {
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });
            var conts = _httpContextAccessor.HttpContext;
            using (DataContext _context = new DataContext())
            {
                if (UsersConstant.GetRole(int.Parse(conts.User.Identity.Name)) == UsersConstant.superadmin)
                {
                    var rs = context.TblScanData.Where(p => p.DeletedOn == null).OrderByDescending(p => p.CreatedOn).ToList();
                    res.Data = rs;
                }
                else
                {
                    if (UsersConstant.GetRole(int.Parse(conts.User.Identity.Name)) == UsersConstant.admin || UsersConstant.GetRole(int.Parse(conts.User.Identity.Name)) == UsersConstant.user)
                    {
                        var uid = int.Parse(conts.User.Identity.Name);
                        var rs = context.TblScanData.Where(p => p.DeletedOn == null && p.Uid == uid).OrderByDescending(p => p.CreatedOn).ToList();
                        res.Data = rs;
                    }
                }
            }
            return res;
        }

        public ResponseModel addScanData(ScanDataModel tblscandata)
        {
            var contex = _httpContextAccessor.HttpContext;
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            if (UsersConstant.GetRole(int.Parse(contex.User.Identity.Name)) == UsersConstant.admin || UsersConstant.GetRole(int.Parse(contex.User.Identity.Name)) == UsersConstant.superadmin || UsersConstant.GetRole(int.Parse(contex.User.Identity.Name)) == UsersConstant.user)
            {

                try
                {
                    using (DataContext context = new DataContext())
                    {
                        Guid g;
                        g = Guid.NewGuid();
                        var rs = context.TblScanData.FirstOrDefault(p => p.ScanId != tblscandata.ScanId);
                        if (rs != null)
                        {
                            var x = new TblScanData
                            {
                                ScanId = g.ToString(),
                                Uid = int.Parse(contex.User.Identity.Name),
                                CreatedOn = DateTime.UtcNow,
                                Payload = tblscandata.Payload,
                                DataType = tblscandata.DataType,
                                FileName = tblscandata.FileName,
                                Status = tblscandata.Status,
                                DeletedOn = null,
                            };
                            context.TblScanData.Add(x);
                            context.SaveChanges();
                            var item = new ScanDataModel
                            {
                                ScanId = x.ScanId,
                                Uid = int.Parse(contex.User.Identity.Name),
                                CreatedOn = new DateTime(x.CreatedOn.Value.Ticks).ToString("o"),
                                Payload = x.Payload,
                                DataType = x.DataType,
                                FileName = x.FileName,
                                Status = x.Status,
                            };
                            res.Data = item;
                        }
                    }
                }
                catch (Exception ex)
                {
                    res.Status = "500";
                    res.Message = ex.Message;
                }

                return res;
            }
            return res;
        }
        public ResponseModel DeleteScanData(string scanid)
        {
            var contex = _httpContextAccessor.HttpContext;

            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            if (UsersConstant.GetRole(int.Parse(contex.User.Identity.Name)) == UsersConstant.admin || UsersConstant.GetRole(int.Parse(contex.User.Identity.Name)) == UsersConstant.superadmin)
            {
                try
                {
                    using (DataContext context = new DataContext())
                    {
                        var rs = context.TblScanData.FirstOrDefault(o => o.ScanId == scanid);
                        if (rs != null)
                        {
                            rs.DeletedOn = DateTime.UtcNow;
                            context.SaveChanges();
                            res.Data = rs;
                        }
                    }

                }
                catch (Exception ex)
                {
                    res.Status = "500";
                    res.Message = ex.Message;
                }
                return res;
            }
            return res;
        }

        public ResponseModel DeleteArrScanData(List<string> deleteIds)
        {
            var contex = _httpContextAccessor.HttpContext;
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            if (UsersConstant.GetRole(int.Parse(contex.User.Identity.Name)) == UsersConstant.admin || UsersConstant.GetRole(int.Parse(contex.User.Identity.Name)) == UsersConstant.superadmin)
            {

                try
                {
                    using (DataContext context = new DataContext())
                    {
                        foreach (var deleteId in deleteIds)
                        {
                            var scandata = context.TblScanData.FirstOrDefault(o => o.ScanId == deleteId);
                            if (scandata != null)
                            {
                                scandata.DeletedOn = DateTime.UtcNow;
                            }
                            res.Data = deleteIds;
                        }

                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    res.Status = "500";
                    res.Message = ex.Message;
                }
                return res;
            }
            return res;
        }
        public ResponseModel EditScandata(ScanDataModel tblscandata)
        {
            var contex = _httpContextAccessor.HttpContext;
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            if (UsersConstant.GetRole(int.Parse(contex.User.Identity.Name)) == UsersConstant.admin || UsersConstant.GetRole(int.Parse(contex.User.Identity.Name)) == UsersConstant.superadmin)
            {

                try
                {
                    using (DataContext context = new DataContext())
                    {
                        var oldscandata = (from u in context.TblScanData where u.ScanId == tblscandata.ScanId select u).FirstOrDefault();
                        if (oldscandata != null)
                        {
                            oldscandata.Uid = tblscandata.Uid;
                            oldscandata.Payload = tblscandata.Payload;
                            oldscandata.DataType = tblscandata.DataType;
                            oldscandata.FileName = tblscandata.FileName;
                            oldscandata.Status = tblscandata.Status;
                            oldscandata.DeletedOn = null;
                            context.SaveChanges();
                            res.Data = oldscandata;
                        }
                    }
                }
                catch (Exception ex)
                {
                    res.Status = "500";
                    res.Message = ex.Message;
                }
                return res;
            }
            if (UsersConstant.GetRole(int.Parse(contex.User.Identity.Name)) == UsersConstant.user)
            {

                try
                {
                    using (DataContext context = new DataContext())
                    {
                        var oldscandata = (from u in context.TblScanData where u.ScanId == tblscandata.ScanId && u.Uid == int.Parse(contex.User.Identity.Name) select u).FirstOrDefault();
                        if (oldscandata != null)
                        {

                            oldscandata.Payload = tblscandata.Payload;
                            oldscandata.DataType = tblscandata.DataType;
                            oldscandata.FileName = tblscandata.FileName;
                            oldscandata.Status = tblscandata.Status;
                            oldscandata.DeletedOn = null;
                            context.SaveChanges();
                            res.Data = oldscandata;
                        }
                    }
                }
                catch (Exception ex)
                {
                    res.Status = "500";
                    res.Message = ex.Message;
                }
                return res;
            }
            return res;
        }
        public ResponseModel CreateQR()
        {
            var context = _httpContextAccessor.HttpContext;
            CreateQR createqr = new CreateQR();
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            try
            {
                createqr.Command = "CONNECTTOTRX";
                createqr.ServerAddress = "h2673771.stratoserver.net";
                createqr.Port = 80;
                createqr.URLPart = "webservicestx";
                var x = UsersConstant.GetUserName(context.User.Identity.Name);
                createqr.User = x;
                createqr.EncryptionKey = "kJDJzwrVS6RTFgdafgc3d ";
                var textboxQR = (createqr.Command + ":" + createqr.ServerAddress + ":" + createqr.Port + "/" + createqr.URLPart + "|" + createqr.EncryptionKey + "|" + createqr.User);
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
