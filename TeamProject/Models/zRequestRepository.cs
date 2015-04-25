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
            } //all stuff being added that already exists in the database needs to be attached 
            //otherwise a new entry for it will be added in the db which is baaad
            List<zPark> parkList = new List<zPark>();
            List<zBuilding> buildingList = new List<zBuilding>();
            List<zRoom> roomList = new List<zRoom>();
            foreach (var booking in zrequest.zRoomBooking)
            {
                switch (booking.Type)
                {
                    case 0: //any room not saved in db
                        break;

                    case 1: //any room in a park
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

                    case 2: //any room in a building
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

                    case 3: //specific room
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

                        /*
                         * the code in this section (checked if a booking is unique or not) 
                         * is a attempt to allow for multiple "Any room in a park/building" 
                         * in a single request but it didn't really work.
                         * if such a request is created, the second park/building will be 
                         * added to the db as a new park/building.
                         * need to find a way to fix this, attaching multiple times doesn't work
                         */
                }
            }
            context.zRequest.Add(zrequest);
        }

        public void Update(zRequest zrequest)
        {//this is called when editing a request
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

            //update all the simple stuff by simple assignment

            updateRequest.zWeek = zrequest.zWeek;
            //surprisingly, this works 

            var oldFacList = updateRequest.zFacility.ToList();
            foreach (var oldFacility in oldFacList)
            {
                updateRequest.zFacility.Remove(oldFacility);
            }

            foreach (var item in zrequest.zFacility)
            {
                context.zFacility.Attach(item);
                updateRequest.zFacility.Add(item);
            }
            //horrible way of doing this, remove all facilities then add them all as new ones
            //same principle for room bookings
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
                    case 1: //any room in a park
                        context.zPark.Attach(item.zPark);
                        updateRequest.zRoomBooking.Add(item);
                        break;

                    case 2: //any room in a building
                        context.zBuilding.Attach(item.zBuilding);
                        updateRequest.zRoomBooking.Add(item);
                        break;

                    case 3: //specific room
                        context.zRoom.Attach(item.zRoom);
                        updateRequest.zRoomBooking.Add(item);
                        break;
                }
            }
        }

                /* 
                 * this probably isn't needed anymore, delete soon.
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
                */

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