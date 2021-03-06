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
    public partial class zRequest
    {
        public zRequest()
        {
            this.zRoom = new HashSet<zRoom>();
            this.zFacility = new HashSet<zFacility>();
            this.zRoomBooking = new HashSet<zRoomBooking>();
        }
    
        public int RequestId { get; set; }
        public string ModCode { get; set; }
        public short StatusId { get; set; }
        public int WeekId { get; set; }
        public short DayId { get; set; }
        public short PeriodId { get; set; }
        public short SessionLength { get; set; }
        public short Semester { get; set; }
        public int RoundNo { get; set; }
        public string SpecialRequirement { get; set; }
        public short UserId { get; set; }
        public int RoomCount { get; set; }
    
        public virtual zDay zDay { get; set; }
        public virtual zModule zModule { get; set; }
        public virtual zPeriod zPeriod { get; set; }
        public virtual zRound zRound { get; set; }
        public virtual zStatus zStatus { get; set; }
        public virtual zUser zUser { get; set; }
        public virtual zWeek zWeek { get; set; }
        public virtual ICollection<zRoom> zRoom { get; set; }
        public virtual ICollection<zFacility> zFacility { get; set; }
        public virtual ICollection<zRoomBooking> zRoomBooking { get; set; }
    }
    
}
