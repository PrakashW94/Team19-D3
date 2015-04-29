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
    public class zRoomRepository : IzRoomRepository
    {
        DatabaseContext context = new DatabaseContext();

        public IQueryable<zRoom> All
        {
            get { return context.zRoom; }
        }

        public IQueryable<zRoom> AllIncluding(params Expression<Func<zRoom, object>>[] includeProperties)
        {
            IQueryable<zRoom> query = context.zRoom;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public zRoom Find(int id)
        {
            return context.zRoom.Find(id);
        }

        public void InsertOrUpdate(zRoom zroom)
        {
            if (zroom.RoomId == default(int)) {
                // New entity
                context.zRoom.Add(zroom);
            } 
            else {
                // Existing entity
                var updateRoom = context.zRoom.Find(zroom.RoomId);

                var removedFacs1 = updateRoom.zFacility.ToList();
                var newFacs1 = zroom.zFacility.ToList();
                removedFacs1.RemoveAll(x => newFacs1.Any(y => y.FacilityId == x.FacilityId));
                foreach (var facility in removedFacs1)
                {
                    updateRoom.zFacility.Remove(facility);
                }

                var removedFacs2 = updateRoom.zFacility.ToList();
                var newFacs2 = zroom.zFacility.ToList();
                newFacs2.RemoveAll(x => removedFacs2.Any(y => y.FacilityId == x.FacilityId));
                foreach (var item in newFacs2)
                {
                    context.zFacility.Attach(item);
                    updateRoom.zFacility.Add(item);
                }
            }
        }

        public void Delete(int id)
        {
            var zroom = context.zRoom.Find(id);
            context.zRoom.Remove(zroom);
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

    public interface IzRoomRepository : IDisposable
    {
        IQueryable<zRoom> All { get; }
        IQueryable<zRoom> AllIncluding(params Expression<Func<zRoom, object>>[] includeProperties);
        zRoom Find(int id);
        void InsertOrUpdate(zRoom zroom);
        void Delete(int id);
        void Save();
    }
}