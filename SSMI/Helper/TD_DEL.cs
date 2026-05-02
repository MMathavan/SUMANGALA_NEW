using SSMI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSMI.Helper
{
    public class TD_DEL
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public static string delete_check1(string fname, string pid, string tabname)
        {
            var TmpPrcdStatus = "PROCEED";
            var TmpRCount = 0; var Tmpcount = 0;
            using (var context = new ApplicationDbContext())
            {
                var s = context.Database.SqlQuery<Soft_Table_Delete_Detail>("select * from SOFT_TABLE_DELETE_DETAIL where OPTNSTR= '" + fname + "' and TABNAME='" + tabname + "' Order by TABDID");
                var ss = s;


                foreach (var sss in ss)
                {
                    var Tablename = sss.TABNAME;
                    //var m = Tablename;
                    //return m;
                    var fieldname = sss.PFLDNAME;

                    var condstr = sss.DCONDTNSTR + pid;

                    var Dispdesc = sss.DISPDESC;

                    TmpRCount = recordCount(Tablename, fieldname, condstr);
                    Tmpcount = recordCount_det("TRANSACTIONDETAIL", "TRANDID", "TRANDCID=" + pid + "");



                    if (TmpRCount > 0)
                    {
                        TmpPrcdStatus = Dispdesc; break;
                    }
                    if (Tmpcount > 0)
                    {
                        TmpPrcdStatus = "Selected Record Referred in Other  Transaction"; break;
                    }
                    //  return TmpPrcdStatus;
                }
                return TmpPrcdStatus;
            }
        }


        public static int recordCount_det(string Tablename, string fieldname, string condstr)
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

    }
}