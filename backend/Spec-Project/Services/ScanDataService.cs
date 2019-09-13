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

namespace Spec_Project.Services
{
    public interface IScanDataService
    {
        List<TblScanData> getScanData();
        ResponseModel addScanData(ScanDataModel tblscandata);
        ResponseModel DeleteScanData(string scanid);
        ResponseModel DeleteArrScanData(List<string> deleteIds);
        ResponseModel EditScandata(ScanDataModel tblscandata);
        ResponseModel CreateQR();
    }
    public class ScanDataService : IScanDataService
    {
        DataContext context = new DataContext();
        IHttpContextAccessor _httpContextAccessor;
        public ScanDataService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public List<TblScanData> getScanData()
        {
            var cont = _httpContextAccessor.HttpContext;
            if (UsersConstant.GetRole(cont.User.Identity.Name) == "admin")
            {
                using (DataContext context = new DataContext())
                {
                    var rs = context.TblScanData.Where(p => p.DeletedOn == null).OrderByDescending(p => p.CreatedOn).ToList();
                    return rs;
                }
            }
            return null;
        }

        public ResponseModel addScanData(ScanDataModel tblscandata)
        {
            var contex = _httpContextAccessor.HttpContext;
            if (UsersConstant.GetRole(contex.User.Identity.Name) == "admin")
            {
                var res = new ResponseModel()
                {
                    Status = "200",
                    Message = "",
                };
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
                                Uid = tblscandata.Uid,
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
                                Uid = x.Uid,
                                CreatedOn = new DateTime(x.CreatedOn.Value.Ticks).ToString("o"),
                                Payload = x.Payload,
                                DataType = x.DataType,
                                FileName = x.FileName,
                                Status = x.Status,
                                DeletedOn = null,
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
            return null;
        }
        public ResponseModel DeleteScanData(string scanid)
        {
            var contex = _httpContextAccessor.HttpContext;
            if (UsersConstant.GetRole(contex.User.Identity.Name) == "admin")
            {
                var res = new ResponseModel()
                {
                    Status = "200",
                    Message = "",
                };
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
            return null;
        }

        public ResponseModel DeleteArrScanData(List<string> deleteIds)
        {
            var contex = _httpContextAccessor.HttpContext;
            if (UsersConstant.GetRole(contex.User.Identity.Name) == "admin")
            {
                var res = new ResponseModel()
                {
                    Status = "200",
                    Message = "",
                };
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
            return null;
        }
        public ResponseModel EditScandata(ScanDataModel tblscandata)
        {
            var contex = _httpContextAccessor.HttpContext;
            if (UsersConstant.GetRole(contex.User.Identity.Name) == "admin")
            {
                var res = new ResponseModel()
                {
                    Status = "200",
                    Message = "",
                };
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
            return null;
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
