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
    public partial class zBuilding
    {
        public zBuilding()
        {
            this.zRoom = new HashSet<zRoom>();
            this.zRoomBooking = new HashSet<zRoomBooking>();
        }
    
        public int BuildingId { get; set; }
        public string BuildingCode { get; set; }
        public string BuildingName { get; set; }
        public int ParkId { get; set; }
    
        public virtual zPark zPark { get; set; }
        public virtual ICollection<zRoom> zRoom { get; set; }
        public virtual ICollection<zRoomBooking> zRoomBooking { get; set; }
    }
    
}
