//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace TeamProject2
{
    public partial class zDay
    {
        public zDay()
        {
            this.zRequest = new HashSet<zRequest>();
        }
    
        public short DayId { get; set; }
        public string DayValue { get; set; }
    
        public virtual ICollection<zRequest> zRequest { get; set; }
    }
    
}