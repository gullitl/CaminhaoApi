using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CaminhaoApi.Domain.CaminhaoAggregate;
using CaminhaoApi.Infrastructure.Database.Contexts;
using Microsoft.Extensions.Logging;

namespace CaminhaoApi.Application.Services
{
    public interface IServiceCaminhao
    {
        Task<List<Caminhao>> ObterTodosCaminhoes();
        Task<Caminhao> ObterCaminhaoPorId(string id);
        Task<Caminhao> CadastrarCaminhao(Caminhao caminhao);
        Task<Caminhao> AtualizarCaminhao(Caminhao caminhao);
        Task RemoverCaminhao(string id);
    }

    public class ServiceCaminhao : IServiceCaminhao
    {
        private readonly ILogger<IServiceCaminhao> _logger;
        private readonly ICrudContext _crudContext;

        public ServiceCaminhao(ILogger<IServiceCaminhao> logger, ICrudContext crudContext)
        {
            _logger = logger;
            _crudContext = crudContext;
        }

        public async Task<List<Caminhao>> ObterTodosCaminhoes() => await _crudContext.ObterTodos<Caminhao>();

        public async Task<Caminhao> ObterCaminhaoPorId(string id) => await _crudContext.ObterPorId<Caminhao>(id);

        public async Task<Caminhao> CadastrarCaminhao(Caminhao caminhao)
        {
            Validator.ValidateObject(caminhao, new ValidationContext(caminhao, null, null), true);
            return await _crudContext.Criar(caminhao);
        }

        public async Task<Caminhao> AtualizarCaminhao(Caminhao caminhao)
        {
            Validator.ValidateObject(caminhao, new ValidationContext(caminhao, null, null), true);
            return await _crudContext.Atualizar(caminhao);
        }

        public async Task RemoverCaminhao(string id)
        {
            await _crudContext.Remover<Caminhao>(id);
        }
    }
}
