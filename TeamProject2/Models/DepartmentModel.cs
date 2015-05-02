using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeamProject2.Models
{
    public class DepartmentModel
    {
        [StringLength(5)]
        [Display(Name = "D,epartment Code")]
        public IEnumerable<string> DeptCode { get; set; }

        [StringLength(100)]
        [Display(Name = "Department Name")]
        public string DeptName { get; set; }
    }
}