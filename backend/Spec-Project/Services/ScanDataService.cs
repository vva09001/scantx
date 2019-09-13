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
using System.Data.SqlClient;
using LINQtoCSV;
using Microsoft.EntityFrameworkCore;

namespace Spec_Project.Services
{
    public interface IScanDataService
    {
        List<TblScanData> getScanData();
        ResponseModel addScanData(TblScanData tblscandata);
        ResponseModel DeleteScanData(string scanid);
        ResponseModel DeleteArrScanData(List<string> deleteIds);
        ResponseModel EditScandata(TblScanData tblscandata);
        ResponseModel ConvertTblScanDataToCSV(string Uid);
    }
    public class ScanDataService : IScanDataService
    {

        IHttpContextAccessor _httpContextAccessor;
        public ScanDataService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ResponseModel ConvertTblScanDataToCSV(string Uid)
        {

            var res = new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            };
            string mypath = @"h:\root\tmp\";
            string mypath1 = @"E:\Miracles\spec-project\scantx\backend\Spec-Project\";
            try
            {

                DataContext db = new DataContext();

                int role = int.Parse(UsersConstant.GetRole(Uid));

                List<TblScanData> datascan = new List<TblScanData>();

                if (role == RoleConstant.superadmin)
                {
                    datascan.Clear();
                    datascan = db.TblScanData.Select(o => new TblScanData
                    {
                        Uid = o.Uid,
                        CreatedOn = o.CreatedOn,
                        DataType = o.DataType,
                        DeletedOn = o.DeletedOn,
                        FileName = o.FileName,
                        Payload = o.Payload,
                        ScanId = o.ScanId,
                        Status = o.Status
                    }).ToList();

                }
                else
                if (role == RoleConstant.reader)
                {

                    //datascan.Clear();
                    //UsersConstant.GetRole(Uid);
                    //datascan = db.TblScanData.Where(o => o.Uid == Uid).Select(o => new TblScanData
                    //{
                    //    Uid = o.Uid,
                    //    CreatedOn = o.CreatedOn,
                    //    DataType = o.DataType,
                    //    DeletedOn = o.DeletedOn,
                    //    FileName = o.FileName,
                    //    Payload = o.Payload,
                    //    ScanId = o.ScanId,
                    //    Status = o.Status
                    //}).ToList();
                    return res = new ResponseModel
                    {
                        Data = "",
                        Status = "500",
                        Message = "Reader does not have permission to use this feature."
                    };

                }
                else
                if (role == RoleConstant.adminint)
                {

                    datascan.Clear();
                    datascan = db.TblScanData.Include(o => o.U).Where(o => o.Uid == int.Parse(Uid) && (o.U.RoleId == RoleConstant.user.ToString() || o.U.RoleId == RoleConstant.admin || o.U.RoleId == RoleConstant.reader.ToString())).Select(o => new TblScanData
                    {
                        Uid = o.Uid,
                        CreatedOn = o.CreatedOn,
                        DataType = o.DataType,
                        DeletedOn = o.DeletedOn,
                        FileName = o.FileName,
                        Payload = o.Payload,
                        ScanId = o.ScanId,
                        Status = o.Status
                    }).ToList();

                }
                else
                if (role == RoleConstant.userint)
                {
                    datascan.Clear();
                    datascan = db.TblScanData.Include(o => o.U).Where(o => o.Uid == int.Parse(Uid) && (o.U.RoleId == RoleConstant.user.ToString() || o.U.RoleId == RoleConstant.reader.ToString())).Select(o => new TblScanData
                    {
                        Uid = o.Uid,
                        CreatedOn = o.CreatedOn,
                        DataType = o.DataType,
                        DeletedOn = o.DeletedOn,
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
                res.Data = "scantx.miracles.vn/mycsv/scandata_" + now +".csv";
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

        public List<TblScanData> getScanData()
        {
            var cont = _httpContextAccessor.HttpContext;
            
                using (DataContext context = new DataContext())
                {
                    var rs = context.TblScanData.Where(p => p.DeletedOn == null).OrderByDescending(p => p.CreatedOn).ToList();
                    return rs;
                }
            
        }

        public ResponseModel addScanData(TblScanData tblscandata)
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
                            res.Data = x;
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
        public ResponseModel EditScandata(TblScanData tblscandata)
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
                            oldscandata.CreatedOn = DateTime.UtcNow;
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

    }
}
