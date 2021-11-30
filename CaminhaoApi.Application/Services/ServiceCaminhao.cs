using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CaminhaoApi.Domain.CaminhaoAggregate;
using CaminhaoApi.Infrastructure.Database.Contexts;

namespace CaminhaoApi.Application.Services
{
    public interface IServiceCaminhao
    {
        Task<List<Caminhao>> ObterTodosOsCaminhoes();
        Task<Caminhao> ObterCaminhaoPorId(string id);
        Task<Caminhao> CadastrarCaminhao(Caminhao caminhao);
        Task<Caminhao> AtualizarCaminhao(Caminhao caminhao);
        Task<bool> RemoverCaminhao(string id);
    }

    public class ServiceCaminhao : IServiceCaminhao
    {
        private readonly ICaminhaoContext _caminhaoContext;

        public ServiceCaminhao(ICaminhaoContext caminhaoContext)
        {
            _caminhaoContext = caminhaoContext;
        }

        public async Task<List<Caminhao>> ObterTodosOsCaminhoes() => await _caminhaoContext.ObterTodos();

        public async Task<Caminhao> ObterCaminhaoPorId(string id) => await _caminhaoContext.ObterPorId(id);

        public async Task<Caminhao> CadastrarCaminhao(Caminhao caminhao)
        {
            Validator.ValidateObject(caminhao, new ValidationContext(caminhao, null, null), true);
            return await _caminhaoContext.Criar(caminhao);
        }

        public async Task<Caminhao> AtualizarCaminhao(Caminhao caminhao)
        {
            Validator.ValidateObject(caminhao, new ValidationContext(caminhao, null, null), true);
            return await _caminhaoContext.Atualizar(caminhao);
        }

        public async Task<bool> RemoverCaminhao(string id) => await _caminhaoContext.Remover(id);
    }
}
