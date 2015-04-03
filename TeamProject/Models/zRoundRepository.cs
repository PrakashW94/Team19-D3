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
    public class zRoundRepository : IzRoundRepository
    {
        DatabaseContext context = new DatabaseContext();

        public IQueryable<zRound> All
        {
            get { return context.zRounds; }
        }

        public IQueryable<zRound> AllIncluding(params Expression<Func<zRound, object>>[] includeProperties)
        {
            IQueryable<zRound> query = context.zRounds;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public zRound Find(short id)
        {
            return context.zRounds.Find(id);
        }

        public void InsertOrUpdate(zRound zround)
        {
            if (zround.RoundNo == default(short)) {
                // New entity
                context.zRounds.Add(zround);
            } else {
                // Existing entity
                context.Entry(zround).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(short id)
        {
            var zround = context.zRounds.Find(id);
            context.zRounds.Remove(zround);
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

    public interface IzRoundRepository : IDisposable
    {
        IQueryable<zRound> All { get; }
        IQueryable<zRound> AllIncluding(params Expression<Func<zRound, object>>[] includeProperties);
        zRound Find(short id);
        void InsertOrUpdate(zRound zround);
        void Delete(short id);
        void Save();
    }
}