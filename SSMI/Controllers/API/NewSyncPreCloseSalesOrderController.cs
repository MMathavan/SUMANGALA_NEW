using SSMI.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
//using System.Web.Mvc;

namespace SSMI.Controllers.API
{
    [RoutePrefix("api/newsyncpreclosesalesorder")]
    public class NewSyncPreCloseSalesOrderController : ApiController
    {
        // GET: NewSyncPreCloseSalesOrder

        [HttpPost]
        [Route("save")]
        public async Task<IHttpActionResult> SavePreCloseSalesOrder()
        {
            try
            {
                string connStr = ConfigurationManager
                    .ConnectionStrings["SSMI_DefaultConnection"].ConnectionString;

                // ✅ Read JSON properly (async)
                string jsonString = await Request.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    return Ok(new
                    {
                        Status = false,
                        Message = "Empty JSON payload"
                    });
                }

                using (SqlConnection con = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand("USP_Update_PreClosure_Bulk", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // ✅ VERY IMPORTANT (fix truncation issue)
                    cmd.Parameters.Add("@Json", SqlDbType.NVarChar, -1).Value = jsonString;

                    await con.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            return Ok(new
                            {
                                Status = Convert.ToBoolean(dr["Status"]),
                                Message = dr["Message"]?.ToString()
                            });
                        }
                    }
                }

                return Ok(new
                {
                    Status = false,
                    Message = "No response from stored procedure"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Status = false,
                    Message = ex.Message,
                    Details = ex.InnerException?.Message // 🔥 extra debug
                });
            }
        }



    }
}