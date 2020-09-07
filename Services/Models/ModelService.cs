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
    public class ModelService<TModel> where TModel : Model
    {
        protected readonly ApplicationContext _db;
        protected readonly DbSet<TModel> _models;

        public ModelService(ApplicationContext context)
        {
            _db = context;
            _models = context.Set<TModel>();
        }
        
        public virtual async Task Create(TModel modelObject)
        {
            modelObject.CreatedAt = DateTime.Now;
            modelObject.UpdatedAt = DateTime.Now;
            modelObject.DeletedAt = null;

            await _models.AddAsync(modelObject);
            await _db.SaveChangesAsync();
        }
        
        public virtual Task<TModel> FindByPk(int id, bool isDeleted = false)
        {
            return _models.AsNoTracking().FirstOrDefaultAsync(model => model.Id == id && model.DeletedAt == null);
        }

        public virtual Task<List<TModel>> Find(Expression<Func<TModel, bool>> predicate = null, int page = 1, int limit = 0)
        {
            var query = _models.AsQueryable();
            
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            
            return query
                .AsNoTracking()
                .Where(model => model.DeletedAt == null)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public virtual async Task Update(TModel modelObject)
        {
            modelObject.UpdatedAt = DateTime.Now;
            
            _models.Update(modelObject);
            await _db.SaveChangesAsync();
        }

        public virtual async Task Delete(TModel modelObject)
        {
            modelObject.UpdatedAt = DateTime.Now;
            modelObject.DeletedAt = DateTime.Now;

            _models.Update(modelObject);
            await _db.SaveChangesAsync();
        }

        public virtual async Task DeleteByPk(int id)
        {
            var model = await _models.FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                throw new EntityNotFoundException();
            }
            
            model.UpdatedAt = DateTime.Now;
            model.DeletedAt = DateTime.Now;

            _models.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}