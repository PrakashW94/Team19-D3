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
    public partial class zRoomBooking
    {
        public zRoomBooking()
        {
            this.zRequest = new HashSet<zRequest>();
        }
    
        public int BookingId { get; set; }
        public Nullable<int> ParkId { get; set; }
        public Nullable<int> BuildingId { get; set; }
        public Nullable<int> RoomId { get; set; }
        public int Capacity { get; set; }
        public int Type { get; set; }
    
        public virtual zBuilding zBuilding { get; set; }
        public virtual zPark zPark { get; set; }
        public virtual zRoom zRoom { get; set; }
        public virtual ICollection<zRequest> zRequest { get; set; }
    }
    
}