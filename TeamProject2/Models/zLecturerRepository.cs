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
    public class zLecturerRepository : IzLecturerRepository
    {
        DatabaseContext context = new DatabaseContext();

        public IQueryable<zLecturer> All
        {
            get { return context.zLecturer; }
        }

        public IQueryable<zLecturer> AllIncluding(params Expression<Func<zLecturer, object>>[] includeProperties)
        {
            IQueryable<zLecturer> query = context.zLecturer;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public zLecturer Find(int id)
        {
            return context.zLecturer.Find(id);
        }

        public void InsertOrUpdate(zLecturer zlecturer)
        {
            if (zlecturer.LecturerId == default(int)) 
            {
                // New entity
                foreach (var module in zlecturer.zModule)
                {
                    context.zModule.Attach(module);
                }
                context.zLecturer.Add(zlecturer);
            } else 
            {
                // Existing entity
                var updateLecturer = context.zLecturer.Find(zlecturer.LecturerId);
                updateLecturer.LecturerName = zlecturer.LecturerName;

                var removedMods1 = updateLecturer.zModule.ToList();
                var newMods1 = zlecturer.zModule.ToList();
                removedMods1.RemoveAll(x => newMods1.Any(y => y.ModCode == x.ModCode));
                foreach (var module in removedMods1)
                {
                    updateLecturer.zModule.Remove(module);
                }
                var removedMods2 = updateLecturer.zModule.ToList();
                var newMods2 = zlecturer.zModule.ToList();
                newMods2.RemoveAll(x => removedMods2.Any(y => y.ModCode == x.ModCode));
                foreach (var module in newMods2)
                {
                    context.zModule.Attach(module);
                    updateLecturer.zModule.Add(module);
                }
            }
        }

        public void Delete(int id)
        {
            var zlecturer = context.zLecturer.Find(id);
            context.zLecturer.Remove(zlecturer);
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

    public interface IzLecturerRepository : IDisposable
    {
        IQueryable<zLecturer> All { get; }
        IQueryable<zLecturer> AllIncluding(params Expression<Func<zLecturer, object>>[] includeProperties);
        zLecturer Find(int id);
        void InsertOrUpdate(zLecturer zlecturer);
        void Delete(int id);
        void Save();
    }
}