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
    public class zProgrammeRepository : IzProgrammeRepository
    {
        DatabaseContext context = new DatabaseContext();

        public IQueryable<zProgramme> All
        {
            get { return context.zProgramme; }
        }

        public IQueryable<zProgramme> AllIncluding(params Expression<Func<zProgramme, object>>[] includeProperties)
        {
            IQueryable<zProgramme> query = context.zProgramme;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public zProgramme Find(int id)
        {
            return context.zProgramme.Find(id);
        }

        public void InsertOrUpdate(zProgramme zprogramme)
        {
            if (zprogramme.ProgrammeCode == default(int)) 
            {
                // New entity
                foreach (var module in zprogramme.zModule)
                {
                    context.zModule.Attach(module);
                }
                context.zProgramme.Add(zprogramme);
            } else 
            {
                // Existing entity
                var updateProgramme = context.zProgramme.Find(zprogramme.ProgrammeCode);
                updateProgramme.ProgrammeTitle = zprogramme.ProgrammeTitle;
                updateProgramme.ProgramType = zprogramme.ProgramType;
                updateProgramme.GradType = zprogramme.GradType;
                updateProgramme.DeptCode = zprogramme.DeptCode;

                var removedMods1 = updateProgramme.zModule.ToList();
                var newMods1 = zprogramme.zModule.ToList();
                removedMods1.RemoveAll(x => newMods1.Any(y => y.ModCode == x.ModCode));
                foreach (var module in removedMods1)
                {
                    updateProgramme.zModule.Remove(module);
                }
                var removedMods2 = updateProgramme.zModule.ToList();
                var newMods2 = zprogramme.zModule.ToList();
                newMods2.RemoveAll(x => removedMods2.Any(y => y.ModCode == x.ModCode));
                foreach (var module in newMods2)
                {
                    context.zModule.Attach(module);
                    updateProgramme.zModule.Add(module);
                }
            }
        }

        public void Delete(int id)
        {
            var zprogramme = context.zProgramme.Find(id);
            context.zProgramme.Remove(zprogramme);
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
        zProgramme Find(int id);
        void InsertOrUpdate(zProgramme zprogramme);
        void Delete(int id);
        void Save();
    }
}