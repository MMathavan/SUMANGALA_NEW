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
    [Table("DESIGNATIONMASTER")]
    public class DesignationMaster
    {
        [Key]
        public int DSGNID { get; set; }
        [DisplayName("Description")]
        [Required(ErrorMessage = "Field is required")]
        [Remote("ValidateDSGNDESC", "Common", AdditionalFields = "i_DSGNDESC", ErrorMessage = "This is already used.")]
        public string DSGNDESC { get; set; }
        [DisplayName("Code")]
        [Required(ErrorMessage = "Field is required")]
        [Remote("ValidateDSGNCODE", "Common", AdditionalFields = "i_DSGNCODE", ErrorMessage = "This is already used.")]
        public string DSGNCODE { get; set; }
        public string CUSRID { get; set; }
        public int LMUSRID { get; set; }
        [DisplayName("Status")]
        [Required(ErrorMessage = "Field is required")]
        public short DISPSTATUS { get; set; }
        [DataType(DataType.Date)]
        public DateTime PRCSDATE { get; set; }
    }
}