using SSMI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSMI.Helper
{
    public class Delete_fun
    {

        ApplicationDbContext context = new ApplicationDbContext();

        public static string delete_check1(string fname, string pid)
        {
            var TmpPrcdStatus = "PROCEED";
            var TmpRCount = 0;
            using (var context = new ApplicationDbContext())
            {
                var s = context.Database.SqlQuery<Soft_Table_Delete_Detail>("select * from SOFT_TABLE_DELETE_DETAIL where OPTNSTR= '" + fname + "' Order by TABDID");
                var ss = s;


                foreach (var sss in ss)
                {
                    var Tablename = sss.TABNAME;
                    //var m = Tablename;
                    //return m;
                    var fieldname = sss.PFLDNAME;

                    var condstr = sss.DCONDTNSTR + pid;

                    var Dispdesc = sss.DISPDESC;
                    // if (fieldname != "TRANSACTIONDETAIL")
                    // {
                    // if (fieldname != "TRANSACTIONDETAILSCHEDULE")
                    //{
                    TmpRCount = recordCount(Tablename, fieldname, condstr);
                    // }
                    // }
                    // else
                    //{
                    //    TmpRCount = recordCount(Tablename, fieldname, condstr);
                    // }

                    if (TmpRCount > 0)
                    {
                        TmpPrcdStatus = Dispdesc; break;
                    }
                    //  return TmpPrcdStatus;
                }
                return TmpPrcdStatus;
            }
        }



        public static int recordCount(string Tablename, string fieldname, string condstr)
        {
            ApplicationDbContext context = new ApplicationDbContext();



            if (condstr.Trim().Length > 0)
            {


                var d = context.Database.SqlQuery<Int32>("select Count(" + fieldname + ") As Rcount from " + Tablename + "  where " + condstr + " Group by " + fieldname).ToList();
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


        public static string tran_del(string fname, string pid, string tblname)
        {
            var TmpPrcdStatus = "PROCEED";
            var TmpRCount = 0;
            using (var context = new ApplicationDbContext())
            {
                var s = context.Database.SqlQuery<Soft_Table_Delete_Detail>("select * from SOFT_TABLE_DELETE_DETAIL where OPTNSTR= '" + fname + "'  Order by TABDID").ToList();
                var ss = s;

                var x1 = 0; var x2 = 0;
                //  var x = "";
                if (pid.Contains('-'))
                {
                    var x = pid.Split('-');
                    x1 = Convert.ToInt32(x[0]);
                    x2 = Convert.ToInt32(x[1]);
                }
                int i = 0;
                foreach (var sss in ss)
                {
                    if (i == 0) pid = x1.ToString(); else pid = x2.ToString();
                    var Tablename = sss.TABNAME;
                    //var m = Tablename;
                    //return m;
                    var fieldname = sss.PFLDNAME;

                    var condstr = sss.DCONDTNSTR + pid;

                    var Dispdesc = sss.DISPDESC;

                    TmpRCount = recordCount(Tablename, fieldname, condstr);

                    if (TmpRCount > 0)
                    {
                        TmpPrcdStatus = Dispdesc; break;
                    }
                    //  return TmpPrcdStatus;
                    i++;
                }
                return TmpPrcdStatus;
            }
        }


    }
}