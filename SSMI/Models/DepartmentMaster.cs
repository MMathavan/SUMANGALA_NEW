using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SSMI.Models
{
    [Table("DEPARTMENTMASTER")]
    public class DepartmentMaster
    {
        [Key]
        public int DEPTID { get; set; }
        [DisplayName("Description")]
        [Required(ErrorMessage = "Field is required")]
        [Remote("ValidateDEPTDESC", "Common", AdditionalFields = "i_DEPTDESC", ErrorMessage = "This is already used.")]
        public string DEPTDESC { get; set; }
        [DisplayName("Code")]
        [Required(ErrorMessage = "Field is required")]
        [Remote("ValidateDEPTCODE", "Common", AdditionalFields = "i_DEPTCODE", ErrorMessage = "This is already used.")]
        public string DEPTCODE { get; set; }
        public string CUSRID { get; set; }
        public int LMUSRID { get; set; }
        [DisplayName("Status")]
        [Required(ErrorMessage = "Field is required")]
        public short DISPSTATUS { get; set; }
        [DataType(DataType.Date)]
        public DateTime PRCSDATE { get; set; }

    }
}