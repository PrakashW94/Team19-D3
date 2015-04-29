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
    public class zProgrammeRepository : IzProgrammeRepository
    {
        DatabaseContext context = new DatabaseContext();

        public IQueryable<zProgramme> All
        {
            get { return context.zProgrammes; }
        }

        public IQueryable<zProgramme> AllIncluding(params Expression<Func<zProgramme, object>>[] includeProperties)
        {
            IQueryable<zProgramme> query = context.zProgrammes;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public zProgramme Find(string id)
        {
            return context.zProgrammes.Find(id);
        }

        public void InsertOrUpdate(zProgramme zprogramme)
        {
            if (zprogramme.ProgrammeCode == default(string)) {
                // New entity
                context.zProgrammes.Add(zprogramme);
            } else {
                // Existing entity
                context.Entry(zprogramme).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(string id)
        {
            var zprogramme = context.zProgrammes.Find(id);
            context.zProgrammes.Remove(zprogramme);
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

    public interface IzProgrammeRepository : IDisposable
    {
        IQueryable<zProgramme> All { get; }
        IQueryable<zProgramme> AllIncluding(params Expression<Func<zProgramme, object>>[] includeProperties);
        zProgramme Find(string id);
        void InsertOrUpdate(zProgramme zprogramme);
        void Delete(string id);
        void Save();
    }
}