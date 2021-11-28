using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaminhaoApi.Infrastructure.Database.Contexts
{
    public interface ICrudContext
    {
        Task<List<TEntity>> ObterTodos<TEntity>() where TEntity : class;
        Task<TEntity> ObterPorId<TEntity>(string id) where TEntity : class;
        Task<TEntity> Criar<TEntity>(TEntity tEntity) where TEntity : class;
        Task<TEntity> Atualizar<TEntity>(TEntity tEntity) where TEntity : class;
        Task Remover<TEntity>(string id) where TEntity : class;
    }

    public abstract class CrudContext : ICrudContext
    {
        private readonly ILogger<CrudContext> _logger;
        private readonly DatabaseContext Context;

        public CrudContext(ILogger<CrudContext> logger, DatabaseContext context)
        {
            _logger = logger;
            Context = context;
        }

        public async Task<List<TEntity>> ObterTodos<TEntity>() where TEntity : class
        {
            try
            {
                return await Context.Set<TEntity>().ToListAsync();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<TEntity> ObterPorId<TEntity>(string id) where TEntity : class
        {
            try
            {
                return await Context.Set<TEntity>().FindAsync(id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<TEntity> Criar<TEntity>(TEntity tEntity) where TEntity : class
        {
            try
            {
                Context.Set<TEntity>().Add(tEntity);
                await Context.SaveChangesAsync();
                return tEntity;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<TEntity> Atualizar<TEntity>(TEntity tEntity) where TEntity : class
        {
            try
            {
                Context.Entry(tEntity).State = EntityState.Modified;
                await Context.SaveChangesAsync();
                return tEntity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            }
        }

        public async Task Remover<TEntity>(string id) where TEntity : class
        {
            try
            {
                TEntity tEntity = await Context.Set<TEntity>().FindAsync(id);

                //if (tEntity == null)
                //        throw new KeyNotFoundException($"The Id specified for accessing {nameof(TEntity)} record does not match any Id in the database");

                Context.Set<TEntity>().Remove(tEntity);
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            }

        }
    }
}
