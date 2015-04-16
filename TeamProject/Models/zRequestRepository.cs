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

        public void InsertOrUpdate(zRequest zrequest)
        {
            if (zrequest.RequestId == default(int)) {
                // New entity
                foreach (var facility in zrequest.zFacility)
                {
                    context.zFacility.Attach(facility);
                }
                context.zRequest.Add(zrequest);
            } else {
                // Existing entity
                context.Entry(zrequest).State = System.Data.Entity.EntityState.Modified;
            }
        }

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
        void InsertOrUpdate(zRequest zrequest);
        void Delete(int id);
        void Save();
    }
}