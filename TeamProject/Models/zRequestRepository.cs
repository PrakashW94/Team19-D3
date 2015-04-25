using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TeamProject;

namespace TeamProject.Models
{ 
    public class zRequestRepository : IzRequestRepository
    {
        DatabaseContext context = new DatabaseContext();

        public IQueryable<zRequest> All
        {
            get { return context.zRequest; }
        }

        public IQueryable<zRequest> AllIncluding(params Expression<Func<zRequest, object>>[] includeProperties)
        {
            IQueryable<zRequest> query = context.zRequest;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public zRequest Find(int id)
        {
            return context.zRequest.Find(id);
        }

        public void Insert(zRequest zrequest)
        {
            // New entity
            foreach (var facility in zrequest.zFacility)
            {
                context.zFacility.Attach(facility);
            }
            List<zPark> parkList = new List<zPark>();
            List<zBuilding> buildingList = new List<zBuilding>();
            List<zRoom> roomList = new List<zRoom>();
            foreach (var booking in zrequest.zRoomBooking)
            {
                switch (booking.Type)
                {
                    case 0:
                        break;

                    case 1:
                        var newPark = true;
                        foreach (var item in parkList)
                        {
                            if (booking.zPark.ParkId == item.ParkId)
                            {
                                newPark = false;
                            }
                        }
                        if (newPark)
                        {
                            parkList.Add(booking.zPark);
                            context.zPark.Attach(booking.zPark);
                        }
                        break;

                    case 2:
                        var newBuilding = false;
                        foreach (var item in buildingList)
                        {
                            if (booking.zBuilding.BuildingId == item.BuildingId)
                            {
                                newBuilding = false;
                            }
                        }
                        if (newBuilding)
                        {
                            buildingList.Add(booking.zBuilding);
                            context.zBuilding.Attach(booking.zBuilding);
                        }
                        break;

                    case 3:
                        var newRoom = false;
                        foreach (var item in roomList)
                        {
                            if (booking.zRoom.RoomId == item.RoomId)
                            {
                                newRoom = true;
                            }
                        }
                        if (newRoom)
                        {
                            roomList.Add(booking.zRoom);
                            context.zRoom.Attach(booking.zRoom);
                        }
                        break;
                }
            }
            context.zRequest.Add(zrequest);
        }
                /* this stuff probably isn't needed anymore, delete soon.
                foreach (var park in zrequest.zPark)
                {
                    context.zPark.Attach(park);
                }

                List<zBuilding> buildingList = new List<zBuilding>();
                foreach (var item in zrequest.zBuilding)
                {
                    bool newItem = true;
                    foreach (var building in buildingList)
                    {
                        if (building.BuildingCode == item.BuildingCode)
                        {
                            newItem = false;
                        }
                    }
                    if (newItem)
                    {
                        context.zBuilding.Attach(item);
                        buildingList.Add(item);
                    }
                }

                foreach (var room in zrequest.zRoom)
                {
                    context.zRoom.Attach(room);
                }
                */

        public void Update(zRequest zrequest)
        {
            // Existing entity
            var updateRequest = context.zRequest.Find(zrequest.RequestId);

            updateRequest.RequestId = zrequest.RequestId;
            updateRequest.ModCode = zrequest.ModCode;
            updateRequest.StatusId = zrequest.StatusId;
            updateRequest.WeekId = zrequest.WeekId;
            updateRequest.DayId = zrequest.DayId;
            updateRequest.PeriodId = zrequest.PeriodId;
            updateRequest.SessionLength = zrequest.SessionLength;
            updateRequest.Semester = zrequest.Semester;
            updateRequest.RoundNo = zrequest.RoundNo;
            updateRequest.SpecialRequirement = zrequest.SpecialRequirement;
            updateRequest.UserId = zrequest.UserId;
            updateRequest.RoomCount = zrequest.RoomCount;

            updateRequest.zWeek = zrequest.zWeek;

            var facList = updateRequest.zFacility.ToList();
            foreach (var item in zrequest.zFacility)
            {
                bool newItem = true;
                foreach (var fac in facList)
                {
                    if (fac.FacilityId == item.FacilityId)
                    {
                        newItem = false;
                    }
                }
                if (newItem)
                {
                    context.zFacility.Attach(item);
                    updateRequest.zFacility.Add(item);
                }
            }//this method doesn't take into account facilities that have been removed
            //consider using same horrible method as rooms...

            List<int> bookingList = new List<int>();
            foreach (var booking in updateRequest.zRoomBooking)
            {
                bookingList.Add(booking.BookingId);
            }
            foreach (var bookingId in bookingList)
            {
                var booking = context.zRoomBooking.Find(bookingId);
                context.zRoomBooking.Remove(booking);
            }
            context.SaveChanges(); 
            //THIS WORKS BUT IS A HORRIBLE WAY OF DOING IT!!!
            foreach (var item in zrequest.zRoomBooking)
            {
                switch (item.Type)
                {
                    case 0:
                        break;

                    case 1:
                        context.zPark.Attach(item.zPark);
                        updateRequest.zRoomBooking.Add(item);
                        break;

                    case 2:
                        context.zBuilding.Attach(item.zBuilding);
                        updateRequest.zRoomBooking.Add(item);
                        break;

                    case 3:
                        context.zRoom.Attach(item.zRoom);
                        updateRequest.zRoomBooking.Add(item);
                        break;
                }
            }
        }

                /* this probably isn't needed yet, delete soon.
                var parkList = updateRequest.zPark.ToList();
                foreach (var item in zrequest.zPark)
                {
                    bool newItem = true;
                    foreach (var park in parkList)
                    {
                        if (park.ParkId == item.ParkId)
                        {
                            newItem = false;
                        }
                    }
                    if (newItem)
                    {
                        context.zPark.Attach(item);
                        updateRequest.zPark.Add(item);
                    }
                    
                }

                var buildingList = updateRequest.zBuilding.ToList();
                foreach (var item in zrequest.zBuilding)
                {
                    bool newItem = true;
                    foreach (var building in buildingList)
                    {
                        if (building.BuildingCode== item.BuildingCode)
                        {
                            newItem = false;
                        }
                    }
                    if (newItem)
                    {
                        context.zBuilding.Attach(item);
                        updateRequest.zBuilding.Add(item);
                    }
                    
                }

                var roomList = updateRequest.zRoom.ToList();
                updateRequest.zRoom.Clear();
                foreach (var item in zrequest.zRoom)
                {
                    bool newItem = true;
                    foreach (var room in roomList)
                    {
                        if (room.RoomId == item.RoomId)
                        {
                            newItem = false;
                        }
                    }
                    if (newItem)
                    {
                        context.zRoom.Attach(item);
                        updateRequest.zRoom.Add(item);
                    }
                }*/
            

        public void Delete(int id)
        {
            var zrequest = context.zRequest.Find(id);
            context.zRequest.Remove(zrequest);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }
    }

    public interface IzRequestRepository : IDisposable
    {
        IQueryable<zRequest> All { get; }
        IQueryable<zRequest> AllIncluding(params Expression<Func<zRequest, object>>[] includeProperties);
        zRequest Find(int id);
        void Insert(zRequest zrequest);
        void Update(zRequest zrequest);
        void Delete(int id);
        void Save();
    }
}