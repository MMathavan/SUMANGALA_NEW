using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSMI.Models
{
    public class ApiPreCloseResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<PreCloseSalesOrderData> data { get; set; }   // ✅ LIST
    }
}