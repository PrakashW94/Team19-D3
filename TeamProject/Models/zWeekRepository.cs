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
    public class zWeekRepository : IzWeekRepository
    {
        DatabaseContext context = new DatabaseContext();

        public IQueryable<zWeek> All
        {
            get { return context.zWeeks; }
        }

        public IQueryable<zWeek> AllIncluding(params Expression<Func<zWeek, object>>[] includeProperties)
        {
            IQueryable<zWeek> query = context.zWeeks;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public zWeek Find(int id)
        {
            return context.zWeeks.Find(id);
        }

        public void InsertOrUpdate(zWeek zweek)
        {
            context.zWeeks.Add(zweek);
            /*
            if (zweek.WeekId == default(int)) {
                // New entity
                context.zWeeks.Add(zweek);
            } else {
                // Existing entity
                context.Entry(zweek).State = System.Data.Entity.EntityState.Modified;
            }
            */
        }

        public void Delete(int id)
        {
            var zweek = context.zWeeks.Find(id);
            context.zWeeks.Remove(zweek);
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

    public interface IzWeekRepository : IDisposable
    {
        IQueryable<zWeek> All { get; }
        IQueryable<zWeek> AllIncluding(params Expression<Func<zWeek, object>>[] includeProperties);
        zWeek Find(int id);
        void InsertOrUpdate(zWeek zweek);
        void Delete(int id);
        void Save();
    }
}