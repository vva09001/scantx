using Scanx.Web.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using QRCoder;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Http;
using LINQtoCSV;
using Scanx.Common;

namespace Scanx.Web.Services
{
    public interface IScanDataService
    {
        ResponseModel GetScanData();
        ResponseModel AddScanData(ScanDataModel tblscandata);
        ResponseModel ImportScanData(ImportDataModel tblscandata);
        ResponseModel DeleteScanData(string scanid);
        ResponseModel DeleteArrScanData(List<string> deleteIds);

        ResponseModel EditScandata(ScanDataModel tblscandata);
        ResponseModel CreateQR();
        ResponseModel CheckUserScanData(string scandataID);

        ResponseModel ConvertTblScanDataToCSV(int Uid);

    }
    public class ScanDataService : IScanDataService
    {
        IHttpContextAccessor _httpContextAccessor;
        private IUserService _userService;
        public ScanDataService(IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
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

                int role = _userService.GetRole(Uid);

                List<ScanDataModel> datascan = new List<ScanDataModel>();
                var context = _httpContextAccessor.HttpContext;
                if (role == Constant.Users.Superadmin)
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
                else if (role == Constant.Users.Reader)
                {

                    return res = new ResponseModel
                    {
                        Data = "",
                        Status = "500",
                        Message = "Reader does not have permission to use this feature."
                    };

                }
                else if (role == Constant.Users.Admin || role == Constant.Users.User)
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

        public ResponseModel GetScanData()
        {
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });
            var conts = _httpContextAccessor.HttpContext;
            var role = _userService.GetRole(int.Parse(conts.User.Identity.Name));
            using (DataContext db = new DataContext())
            {
                if (role == Constant.Users.Superadmin)
                {
                    var rs = db.TblScanData.Where(p => p.DeletedOn == null).OrderByDescending(p => p.CreatedOn).ToList();
                    res.Data = rs;
                }
                else
                {
                    if (role == Constant.Users.Admin || role == Constant.Users.User)
                    {
                        var uid = int.Parse(conts.User.Identity.Name);
                        var rs = db.TblScanData.Where(p => p.DeletedOn == null && p.Uid == uid).OrderByDescending(p => p.CreatedOn).ToList();
                        res.Data = rs;
                    }
                }
            }
            return res;
        }

        public ResponseModel CheckUserScanData(string scandataID)
        {
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });
            var conts = _httpContextAccessor.HttpContext;
            using (DataContext db = new DataContext())
            {
                //if (UsersConstant.GetRole(int.Parse(conts.User.Identity.Name)) == UsersConstant.superadmin)
                //{
                var rs = db.TblScanData.Where(p => p.ScanId == scandataID).FirstOrDefault();
                if (rs != null)
                {
                    res.Data = rs.Uid;
                }
                else
                {
                    res.Message = "scanID not found !";
                }
                //}

                //    if (UsersConstant.GetRole(int.Parse(conts.User.Identity.Name)) == UsersConstant.admin)
                //    {
                //        var rs = context.TblScanData.Where(p => p.DeletedOn == null).OrderByDescending(p => p.CreatedOn).ToList();
                //        res.Data = rs;
                //    }
                //    if (UsersConstant.GetRole(int.Parse(conts.User.Identity.Name)) == UsersConstant.user)
                //    {

                //        res.Message = "Access deny !";
                //    }

            }
            return res;
        }
        public ResponseModel ImportScanData(ImportDataModel postData)
        {
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };

            // check role, login info.
            using (DataContext db = new DataContext())
            {
                var user = db.TblUsers.FirstOrDefault(o => o.UserName == postData.User);
                #region Validate
                if (user == null)
                {
                    res.Message = "Account with user name " + postData.User + " is not existed";
                    res.Status = "500";
                    return res;
                }
                if (user.Token != postData.Token)
                {
                    res.Message = "Token is not correct";
                    res.Status = "500";
                    return res;
                }
                if (user.RoleId != Constant.Users.Admin && user.RoleId != Constant.Users.User && user.RoleId != Constant.Users.Superadmin)
                {
                    res.Message = "You dont have permission to do.";
                    res.Status = "500";
                    return res;
                }
                #endregion
                try
                {
                    Guid g;
                    g = Guid.NewGuid();
                    var x = new TblScanData
                    {
                        ScanId = g.ToString(),
                        Uid = user.Id,
                        CreatedOn = DateTime.UtcNow,
                        StationName = postData.StationName,
                        Payload = postData.ScanData.Payload,
                        DataType = postData.ScanData.DataType,
                        FileName = postData.ScanData.FileName,
                        Status = postData.ScanData.Status,
                        DeletedOn = null,
                    };
                    db.TblScanData.Add(x);
                    db.SaveChanges();
                    x.U = null;
                    res.Data = x;
                }
                catch (Exception ex)
                {
                    res.Status = "500";
                    res.Message = ex.Message;
                }
            }
            return res;
        }


        public ResponseModel AddScanData(ScanDataModel tblscandata)
        {
            var contex = _httpContextAccessor.HttpContext;
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            var role = _userService.GetRole(int.Parse(contex.User.Identity.Name));
            if (role == Constant.Users.Admin || role == Constant.Users.Superadmin || role == Constant.Users.User)
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
            var role = _userService.GetRole(int.Parse(contex.User.Identity.Name));
            if (role == Constant.Users.Admin || role == Constant.Users.Superadmin)
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
            var role = _userService.GetRole(int.Parse(contex.User.Identity.Name));
            if (role == Constant.Users.Admin || role == Constant.Users.Superadmin)
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
            var role = _userService.GetRole(int.Parse(contex.User.Identity.Name));
            if (role == Constant.Users.Admin || role == Constant.Users.Superadmin)
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
            if (role == Constant.Users.User)
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
            var createqr = new QRModel();
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
                var x = _userService.GetUser(context.User.Identity.Name);
                if (x != null)
                {
                    createqr.User = x.UserName;
                    createqr.EncryptionKey = x.Token;
                }
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

        public ResponseModel ImportScanData(ScanDataModel tblscandata)
        {
            throw new NotImplementedException();
        }
    }
}
