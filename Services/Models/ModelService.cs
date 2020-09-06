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
        private readonly ApplicationContext _db;
        private readonly DbSet<TModel> _models;

        public ModelService(ApplicationContext context)
        {
            _db = context;
            _models = context.Set<TModel>();
        }
        
        public async Task Create(TModel modelObject)
        {
            modelObject.CreatedAt = DateTime.Now;
            modelObject.UpdatedAt = DateTime.Now;
            modelObject.DeletedAt = null;

            await _models.AddAsync(modelObject);
            await _db.SaveChangesAsync();
        }
        
        public Task<TModel> FindByPk(int id, bool isDeleted = false)
        {
            return _models.FirstOrDefaultAsync(model => model.Id == id && model.DeletedAt == null);
        }

        public Task<List<TModel>> Find(Expression<Func<TModel, bool>> predicate, int page = 1, int limit = 0)
        {
            return _models
                .Where(predicate)
                .Where(model => model.DeletedAt == null)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public async Task Update(TModel modelObject)
        {
            modelObject.UpdatedAt = DateTime.Now;
            
            _models.Update(modelObject);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(TModel modelObject)
        {
            modelObject.UpdatedAt = DateTime.Now;
            modelObject.DeletedAt = DateTime.Now;

            _models.Update(modelObject);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteByPk(int id)
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