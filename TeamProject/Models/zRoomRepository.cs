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
            get { return context.zRooms; }
        }

        public IQueryable<zRoom> AllIncluding(params Expression<Func<zRoom, object>>[] includeProperties)
        {
            IQueryable<zRoom> query = context.zRooms;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public zRoom Find(string id)
        {
            return context.zRooms.Find(id);
        }

        public void InsertOrUpdate(zRoom zroom)
        {
            if (zroom.RoomCode == default(string)) {
                // New entity
                context.zRooms.Add(zroom);
            } else {
                // Existing entity
                context.Entry(zroom).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(string id)
        {
            var zroom = context.zRooms.Find(id);
            context.zRooms.Remove(zroom);
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
        zRoom Find(string id);
        void InsertOrUpdate(zRoom zroom);
        void Delete(string id);
        void Save();
    }
}