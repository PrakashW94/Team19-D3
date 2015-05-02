using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TeamProject2;

namespace TeamProject2.Models
{ 
    public class zFacilityRepository : IzFacilityRepository
    {
        DatabaseContext context = new DatabaseContext();

        public IQueryable<zFacility> All
        {
            get { return context.zFacility; }
        }

        public IQueryable<zFacility> AllIncluding(params Expression<Func<zFacility, object>>[] includeProperties)
        {
            IQueryable<zFacility> query = context.zFacility;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public zFacility Find(short id)
        {
            return context.zFacility.Find(id);
        }

        public void InsertOrUpdate(zFacility zfacility)
        {
            if (zfacility.FacilityId == default(short)) {
                // New entity
                context.zFacility.Add(zfacility);
            } else {
                // Existing entity
                context.Entry(zfacility).State = System.Data.EntityState.Modified;

            }
        }

        public void Delete(short id)
        {
            var zfacility = context.zFacility.Find(id);
            context.zFacility.Remove(zfacility);
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

    public interface IzFacilityRepository : IDisposable
    {
        IQueryable<zFacility> All { get; }
        IQueryable<zFacility> AllIncluding(params Expression<Func<zFacility, object>>[] includeProperties);
        zFacility Find(short id);
        void InsertOrUpdate(zFacility zfacility);
        void Delete(short id);
        void Save();
    }
}