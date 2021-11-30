using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaminhaoApi.Infrastructure.Database.Contexts
{
    public interface ICrudContext<TEntity>
    {
        Task<List<TEntity>> ObterTodos();
        Task<TEntity> ObterPorId(string id);
        Task<TEntity> Criar(TEntity tEntity);
        Task<TEntity> Atualizar(TEntity tEntity);
        Task<bool> Remover(string id);
    }

    public abstract class CrudContext<TEntity> : ICrudContext<TEntity> where TEntity : class
    {
        private readonly ILogger<ICrudContext<TEntity>> _logger;
        protected readonly DatabaseContext _databaseContext;

        public CrudContext(ILogger<ICrudContext<TEntity>> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task<List<TEntity>> ObterTodos()
        {
            try
            {
                return await _databaseContext.Set<TEntity>().ToListAsync();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<TEntity> ObterPorId(string id)
        {
            try
            {
                return await _databaseContext.Set<TEntity>().FindAsync(id);
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

        public async Task<TEntity> Criar(TEntity tEntity)
        {
            try
            {
                _databaseContext.Add(tEntity);
                await _databaseContext.SaveChangesAsync();

                _databaseContext.Entry(tEntity).State = EntityState.Detached;
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

        public async Task<TEntity> Atualizar(TEntity tEntity)
        {
            try
            {
                _databaseContext.Update(tEntity);
                await _databaseContext.SaveChangesAsync();

                _databaseContext.Entry(tEntity).State = EntityState.Detached;
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

        public async Task<bool> Remover(string id)
        {
            try
            {
                TEntity tEntity = await _databaseContext.Set<TEntity>().FindAsync(id);

                if (tEntity == null)
                    throw new KeyNotFoundException($"The Id specified for accessing {typeof(TEntity).Name} record " +
                                                    "does not match any Id in the database");

                _databaseContext.Set<TEntity>().Remove(tEntity);
                return await _databaseContext.SaveChangesAsync() == 1;
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
