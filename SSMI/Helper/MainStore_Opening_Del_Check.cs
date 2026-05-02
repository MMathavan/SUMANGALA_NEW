using SSMI.Data;
using SSMI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSMI.Helper
{
    public class MainStore_Opening_Del_Check
    {
        ApplicationDbContext context = new ApplicationDbContext();

        //public static string delete_check1(string fname, string pid, string tblname)
        //{
        //    var TmpPrcdStatus = "PROCEED";
        //    var TmpRCount = 0;
        //    using (var context = new ApplicationDbContext())
        //    {
        //        var s = context.Database.SqlQuery<Soft_Table_Delete_Detail>("select * from SOFT_TABLE_DELETE_DETAIL where OPTNSTR= '" + fname + "' AND TABNAME='" + tblname + "' Order by TABDID").ToList();


        //        var ss = s;

        //        foreach (var sss in ss)
        //        {
        //            var Tablename = sss.TABNAME;
        //            //var m = Tablename;
        //            //return m;
        //            var fieldname = sss.PFLDNAME;

        //            var condstr = sss.DCONDTNSTR + pid;

        //            var Dispdesc = sss.DISPDESC;

        //            if (sss.DCONDTNSTR.Equals("REGSTRID=20 AND TRANDREFID"))
        //            {
        //                var sel_query = context.Database.SqlQuery<StockBatchMaster>("select * from TRANSACTIONDETAIL inner join TRANSACTIONMASTER on TRANSACTIONDETAIL.TRANMID=TRANSACTIONMASTER.TRANMID inner join STOCKBATCHMASTER on TRANSACTIONDETAIL.TRANDID=STOCKBATCHMASTER.TRANDID where TRANSACTIONMASTER.TRANMID=" + Convert.ToInt32(pid) + " ");
        //                var Tmploop = 0;
        //                List<int> tmplist = new List<int>();
        //                foreach (var res in sel_query)
        //                {
        //                    var trandid = res.TRANDID;

        //                    var field = res.STKBID;


        //                    Tmploop = sel_recordCount(trandid, field);
        //                    tmplist.Add(Tmploop);
        //                }
        //                if (tmplist.Contains(1))
        //                {
        //                    TmpRCount = 1;
        //                }
        //                else
        //                {
        //                    TmpRCount = 0;
        //                }
        //            }
        //            else if (sss.DCONDTNSTR.Equals("REGSTRID=20 AND TRANBID"))
        //            {
        //                var sel_query = context.Database.SqlQuery<int>("select STKBID from STOCKBATCHMASTER where TRANDID=" + Convert.ToInt32(pid) + " ").ToList();
        //                var field = sel_query[0];


        //                TmpRCount = sel_recordCount(Convert.ToInt32(pid), field);

        //            }
        //            else
        //            {

        //                TmpRCount = recordCount(Tablename, fieldname, condstr);
        //            }
        //            if (TmpRCount > 0)
        //            {
        //                TmpPrcdStatus = Dispdesc; break;
        //            }
        //            //  return TmpPrcdStatus;
        //        }
        //        return TmpPrcdStatus;
        //    }
        //}


        public static int recordCount(string Tablename, string fieldname, string condstr)
        {
            ApplicationDbContext context = new ApplicationDbContext();



            if (condstr.Trim().Length > 0)
            {


                var d = context.Database.SqlQuery<Int32>("select Count(" + fieldname + ") As Rcount from " + Tablename + " where " + condstr + " Group by " + fieldname).ToList();
                // return d.Count();
                var count = d.Count();
                if (count != 0)
                {
                    return 1;
                }
                else
                {
                    return 0;

                }

            }

            return 2;

        }




        public static int sel_recordCount(int trandid, int fieldname)
        {
            ApplicationDbContext context = new ApplicationDbContext();



            //if (condstr.Trim().Length > 0)
            //{


            var d = context.Database.SqlQuery<Int32>("select Count(TRANBID) As Rcount from TRANSACTIONDETAIL inner join TRANSACTIONMASTER on TRANSACTIONDETAIL.TRANMID=TRANSACTIONMASTER.TRANMID inner join TRANSACTIONBATCHDETAIL on TRANSACTIONDETAIL.TRANDID=TRANSACTIONBATCHDETAIL.TRANDID  where REGSTRID=20 and STKBID=" + fieldname + "  Group by TRANSACTIONDETAIL.TRANDID").ToList();
            // return d.Count();
            var count = d.Count();
            if (count != 0)
            {
                return 1;
            }
            else
            {
                return 0;

            }

            //}

            //return 2;

        }

    }
}