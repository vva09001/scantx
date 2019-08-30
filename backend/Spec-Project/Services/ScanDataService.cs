using Spec_Project.Models;
using System;
using Spec_Project.Entities;
using System.Linq;
using System.Collections.Generic;

namespace Spec_Project.Services
{
    public interface IScanDataService
    {
        List<TblScanData> getScanData();
        ResponseModel addScanData(TblScanData tblscandata);
        ResponseModel DeleteScanData(string scanid);
        ResponseModel EditScandata(TblScanData tblscandata);
    }
    public class ScanDataService : IScanDataService
    {
        public List<TblScanData> getScanData()
        {
            using (DB_9A9CCA_scantxContext context = new DB_9A9CCA_scantxContext())
            {
                var rs = context.TblScanData.Where(p => p.DeletedOn == null).ToList();
                return rs;
            }
        }

        public ResponseModel addScanData(TblScanData tblscandata)
        {
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            try
            {
                using (DB_9A9CCA_scantxContext context = new DB_9A9CCA_scantxContext())
                {
                    var rs = context.TblScanData.FirstOrDefault(p => p.ScanId != tblscandata.ScanId);
                    if (rs != null)
                    {
                        context.TblScanData.Add(new TblScanData
                        {
                            ScanId = tblscandata.ScanId,
                            Uid = tblscandata.Uid,
                            CreatedOn = DateTime.UtcNow.AddHours(7),
                            Payload = tblscandata.Payload,
                            DataType = tblscandata.DataType,
                            FileName = tblscandata.FileName,
                            Status = tblscandata.Status,
                            DeletedOn = null,
                        });

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
        public ResponseModel DeleteScanData(string scanid)
        {
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            try
            {
                using (DB_9A9CCA_scantxContext context = new DB_9A9CCA_scantxContext())
                {
                    var rs = context.TblScanData.FirstOrDefault(o => o.ScanId == scanid);
                    if (rs != null)
                    {
                        rs.DeletedOn = DateTime.UtcNow.AddHours(7);
                        context.SaveChanges();
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
        public ResponseModel EditScandata(TblScanData tblscandata)
        {
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            try
            {
                using (DB_9A9CCA_scantxContext context = new DB_9A9CCA_scantxContext())
                {
                    var oldscandata = (from u in context.TblScanData where u.ScanId == tblscandata.ScanId select u).FirstOrDefault();
                    if (oldscandata != null)
                    {
                        oldscandata.Uid = tblscandata.Uid;
                        oldscandata.CreatedOn = DateTime.UtcNow.AddHours(7);
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
    }
}
