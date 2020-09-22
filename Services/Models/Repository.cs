using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BackendServiceStarter.Databases;
using BackendServiceStarter.Models;
using BackendServiceStarter.Services.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BackendServiceStarter.Services.Models
{
    public class Repository<TModel> where TModel : Model
    {
        protected readonly ApplicationContext Db;
        protected readonly DbSet<TModel> Models;

        public Repository(ApplicationContext context)
        {
            Db = context;
            Models = context.Set<TModel>();
        }
        
        public virtual async Task Create(TModel modelObject)
        {
            modelObject.CreatedAt = DateTime.Now;
            modelObject.UpdatedAt = DateTime.Now;
            modelObject.DeletedAt = null;

            await Models.AddAsync(modelObject);
            await Db.SaveChangesAsync();
        }
        
        public virtual Task<TModel> FindByPk(int id, bool isDeleted = false)
        {
            return Models.AsNoTracking().FirstOrDefaultAsync(model => model.Id == id && model.DeletedAt == null);
        }

        public virtual Task<List<TModel>> Find(Expression<Func<TModel, bool>> predicate = null, int page = 1, int limit = 50)
        {
            var query = Models.AsQueryable().AsNoTracking();
            
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            
            return query
                .Where(model => model.DeletedAt == null)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public virtual async Task Update(TModel modelObject)
        {
            modelObject.UpdatedAt = DateTime.Now;
            
            Models.Update(modelObject);
            await Db.SaveChangesAsync();
        }

        public virtual async Task Delete(TModel modelObject)
        {
            modelObject.UpdatedAt = DateTime.Now;
            modelObject.DeletedAt = DateTime.Now;

            Models.Update(modelObject);
            await Db.SaveChangesAsync();
        }

        public virtual async Task DeleteByPk(int id)
        {
            var model = await Models.FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                throw new EntityNotFoundException();
            }
            
            model.UpdatedAt = DateTime.Now;
            model.DeletedAt = DateTime.Now;

            Models.Update(model);
            await Db.SaveChangesAsync();
        }
    }
}