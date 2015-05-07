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
    public class zRoundRepository : IzRoundRepository
    {
        DatabaseContext context = new DatabaseContext();

        public IQueryable<zRound> All
        {
            get { return context.zRound; }
        }

        public IQueryable<zRound> AllIncluding(params Expression<Func<zRound, object>>[] includeProperties)
        {
            IQueryable<zRound> query = context.zRound;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public zRound Find(int id)
        {
            return context.zRound.Find(id);
        }

        public void InsertOrUpdate(zRound zround)
        {
            if (zround.RoundNo == default(int)) 
            {
                // New entity
                zround.RoundNo = context.zRound.Count() + 1;
                context.zRound.Add(zround);
            } else {
                // Existing entity
                context.Entry(zround).State = System.Data.EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var zround = context.zRound.Find(id);
            context.zRound.Remove(zround);
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
        zRound Find(int id);
        void InsertOrUpdate(zRound zround);
        void Delete(int id);
        void Save();
    }
}