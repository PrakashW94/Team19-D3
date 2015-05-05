using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeamProject2.Models
{
    public class UserModel
    {
        [Display(Name = "Department Name")]
        public string DeptName { get; set; }

        [StringLength(5)]
        [Display(Name="Department Code")]
        public string DeptCode { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(10)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}