using SSMI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSMI.Helper
{
    public class TMP_InsertPrint
    {
        public static string InsertToTMP(String table_name, String table_fld1, int table_fld2,string cusrid)
        {

            String temp = "";
            using (var context = new ApplicationDbContext())
            {


                //........addng value to TMPRPT....//
                TMPRPT_IDS RPTIDS = new TMPRPT_IDS();
                RPTIDS.KUSRID = cusrid;
                RPTIDS.OPTNSTR = table_fld1;
                RPTIDS.RPTID = Convert.ToInt32(table_fld2);

                context.TMPRPT_IDS.Add(RPTIDS);
                context.SaveChanges();//...End

                temp = "Successfully Added";
            }

            return temp;
        }
        public static string New_InsertToTMP(String table_name, String table_fld1, int table_fld2, string cusrid)
        {

            String temp = "";
            using (var context = new ApplicationDbContext())
            {


                //........addng value to TMPRPT....//
                NEW_TMP_RPT_IDS RPTIDS = new NEW_TMP_RPT_IDS();
                RPTIDS.KUSRID = cusrid;
                RPTIDS.OPTNSTR = table_fld1;
                RPTIDS.RPTID = Convert.ToInt32(table_fld2);

                context.NEW_TMP_RPT_IDS.Add(RPTIDS);
                context.SaveChanges();//...End

                temp = "Successfully Added";
            }

            return temp;
        }
    }
}