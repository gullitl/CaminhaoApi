using Caminhao.Api.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Caminhao.Api.Application.Services
{
    public interface ICrudService<TEntity>
    {
        Task<List<TEntity>> GetAll();
        Task<TEntity> GetById(string id);
        Task<bool> Delete(string id);
        Task<TEntity> Create(TEntity tEntity);
        Task<TEntity> Update(TEntity tEntity);
    }

    public abstract class CrudService<TEntity> where TEntity : class {
        protected readonly ILogger<CrudService<TEntity>> _logger;
        protected DatabaseContext Context { get; }

        public CrudService(ILogger<CrudService<TEntity>> logger, DatabaseContext context) {
            _logger = logger;
            Context = context;
        }
        public async Task<TEntity> Create(TEntity tEntity) {
            try {
                Context.Set<TEntity>().Add(tEntity);
                await Context.SaveChangesAsync();
                return tEntity;
            } catch(InvalidOperationException ex) {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            } catch(DbUpdateException ex) {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }

        }

        public async Task<bool> Delete(string id) {
            try {
                TEntity tEntity = await Context.Set<TEntity>().FindAsync(id);
                if(tEntity == null) {
                    return false;
                }
                Context.Set<TEntity>().Remove(tEntity);
                await Context.SaveChangesAsync();

                return true;
            } catch(DbUpdateException ex) {
                _logger.LogError(ex, ex.Message);
                throw ex;
            } catch(InvalidOperationException ex) {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            }

        }

        public async Task<List<TEntity>> GetAll() {
            try {
                return await Context.Set<TEntity>().ToListAsync();
            } catch(InvalidOperationException ex) {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<TEntity> GetById(string id) { 
            try {
                return await Context.Set<TEntity>().FindAsync(id);
            } catch(InvalidOperationException ex) {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<TEntity> Update(TEntity tEntity) {
            try {
                Context.Entry(tEntity).State = EntityState.Modified;
                await Context.SaveChangesAsync();
                return tEntity;
            } catch(DbUpdateException ex) {
                _logger.LogError(ex, ex.Message);
                throw ex;
            } catch(InvalidOperationException ex) {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            }

        }

    }
}
