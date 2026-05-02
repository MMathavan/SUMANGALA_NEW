using SSMI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSMI.Helper
{
    public class Autonumber
    {

        //public static string supplierautonum(String table_name, String table_fld, String stateid)
        //{

        //    String temp = "";
        //    int sid = Convert.ToInt32(stateid);
        //    using (var context = new ApplicationDbContext())
        //    {
        //        temp = "1";

        //        if (context.suppliermasters.Where(c => c.STATEID == sid).Count() != 0)
        //        {
        //            var autonumber = (context.Database.SqlQuery<int>("SELECT MAX(" + table_fld + ") as " + table_fld + " from " + table_name + " where STATEID =" + sid + "and DISPSTATUS = 0")).ToList();
        //            if (autonumber != null)
        //                temp = (autonumber[0] + 1).ToString();
        //        }
        //    }

        //    return temp;
        //}

        //public static string customerautonum(String table_name, String table_fld, String stateid)
        //{

        //    String temp = "";
        //    int sid = Convert.ToInt32(stateid);
        //    using (var context = new ApplicationDbContext())
        //    {
        //        temp = "1";

        //        if (context.customermasters.Where(c => c.STATEID == sid).Count() != 0)
        //        {
        //            var autonumber = (context.Database.SqlQuery<int>("SELECT MAX(" + table_fld + ") as " + table_fld + " from " + table_name + " where STATEID =" + sid + "and DISPSTATUS = 0")).ToList();
        //            if (autonumber != null)
        //                temp = (autonumber[0] + 1).ToString();
        //        }
        //    }

        //    return temp;
        //}

        public static string employeeautonum(String table_name, String table_fld, String stateid)
        {

            String temp = "";
            int sid = Convert.ToInt32(stateid);
            using (var context = new ApplicationDbContext())
            {
                temp = "1";

                if (context.employeemasters.Where(c => c.DISPSTATUS == 0).Count() != 0)
                {
                    var autonumber = (context.Database.SqlQuery<int>("SELECT MAX(" + table_fld + ") as " + table_fld + " from " + table_name + " where DISPSTATUS = 0")).ToList();
                    if (autonumber != null)
                        temp = (autonumber[0] + 1).ToString();
                }
            }

            return temp;
        }
        //public static string autonum(String table_name, String table_fld, String regid, Int32 compyid, Int32 brnchid)
        //{

        //    String temp = "";
        //    int regids = Convert.ToInt32(regid);
        //    int compyids = Convert.ToInt32(compyid);
        //    int brnchids = Convert.ToInt32(brnchid);
        //    using (var context = new ApplicationDbContext())
        //    {
        //        temp = "1";

        //        if (context.transactionmasters.Where(c => c.REGSTRID == regids && c.COMPYID == compyids && c.BRNCHID == brnchids).Count() != 0)
        //        {
        //            var autonumber = (context.Database.SqlQuery<int>("SELECT MAX(" + table_fld + ") as " + table_fld + " from " + table_name + " where REGSTRID =" + regids + "and DISPSTATUS = 0 and COMPYID = " + compyids + " and BRNCHID = " + brnchids)).ToList();
        //            if (autonumber != null)
        //                temp = (autonumber[0] + 1).ToString();
        //        }
        //    }

        //    return temp;
        //}


        public static string transactiomaster_autonum(String table_name, String table_fld, String record_condn)
        {

            String temp = "";

            using (var context = new ApplicationDbContext())
            {
                temp = "1";
                var autonum = context.Database.SqlQuery<Nullable<Int32>>("SELECT ISNULL(MAX(" + table_fld + "),0) as " + table_fld + " from " + table_name + " where " + record_condn).ToList();
                if (autonum[0] != null)
                {
                    temp = (autonum[0] + 1).ToString();
                }
                else
                {
                    return temp;
                }
            }


            return temp;
        }


        public static string salesorderautonum(String table_name, String table_fld, String regid, Int32 compyid, Int32 brnchid)
        {

            String temp = "";
            int regids = Convert.ToInt32(regid);
            int compyids = Convert.ToInt32(compyid);
            int brnchids = Convert.ToInt32(brnchid);
            using (var context = new ApplicationDbContext())
            {
                temp = "1";

                //if (context.salesordermasters.Where(c => c.REGSTRID == regids && c.COMPYID == compyids && c.BRNCHID == brnchids).Count() != 0)
                //{
                //    var autonumber = (context.Database.SqlQuery<int>("SELECT MAX(" + table_fld + ") as " + table_fld + " from " + table_name + " where REGSTRID =" + regids + "and DISPSTATUS = 0 and COMPYID = " + compyids + " and BRNCHID = " + brnchids)).ToList();
                //    if (autonumber != null)
                //        temp = (autonumber[0] + 1).ToString();
                //}
            }

            return temp;
        }
        public static string stockreceiptautonum(String table_name, String table_fld, String regid, Int32 compyid, Int32 brnchid)
        {

            String temp = "";
            int regids = Convert.ToInt32(regid);
            int compyids = Convert.ToInt32(compyid);
            int brnchids = Convert.ToInt32(brnchid);
            using (var context = new ApplicationDbContext())
            {
                temp = "1";

                //if (context.stockreceiptmasters.Where(c => c.REGSTRID == regids && c.COMPYID == compyids).Count() != 0)
                //{
                //    var autonumber = (context.Database.SqlQuery<int>("SELECT MAX(" + table_fld + ") as " + table_fld + " from " + table_name + " where REGSTRID =" + regids + "and DISPSTATUS = 0 and COMPYID = " + compyids )).ToList();
                //    if (autonumber != null)
                //        temp = (autonumber[0] + 1).ToString();
                //}
            }

            return temp;
        }


        public static string invautonum(String table_name, String table_fld, String regid, Int32 compyid)
        {

            String temp = "";
            int regids = Convert.ToInt32(regid);
            int compyids = Convert.ToInt32(compyid);
            using (var context = new ApplicationDbContext())
            {
                temp = "1";

                //if (context.invoicemasters.Where(c => c.REGSTRID == regids && c.COMPYID == compyids).Count() != 0)
                //{
                //    var autonumber = (context.Database.SqlQuery<int>("SELECT MAX(" + table_fld + ") as " + table_fld + " from " + table_name + " where REGSTRID =" + regids + "and DISPSTATUS=0 and COMPYID=" + compyids)).ToList();
                //    if (autonumber != null)
                //        temp = (autonumber[0] + 1).ToString();
                //}
            }

            return temp;
        }


        public static string newautonum(String table_name, String table_fld, String condtn)
        {

            String temp = "";

            using (var context = new ApplicationDbContext())
            {
                temp = "1";
                var autonumber = context.Database.SqlQuery<Nullable<Int32>>("SELECT MAX(" + table_fld + ") as " + table_fld + " from " + table_name + " where " + condtn + "").ToList();
                if (autonumber[0] != null)
                    temp = (autonumber[0] + 1).ToString();
                else
                    return temp;
            }


            return temp;

        }
    }
}