using CaminhaoApi.Application.Services;
using CaminhaoApi.Domain.CaminhaoAggregate;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaminhaoApi.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CaminhaoController : ControllerBase
    {
        private readonly IServiceCaminhao _serviceCaminhao;

        public CaminhaoController(IServiceCaminhao serviceCaminhao)
        {
            _serviceCaminhao = serviceCaminhao;
        }

        [HttpGet("obtertodososcaminhoes")]
        public async Task<ActionResult<List<Caminhao>>> ObterTodosOsCaminhoes() => await _serviceCaminhao.ObterTodosOsCaminhoes();

        [HttpGet("obtercaminhaoporid")]
        public async Task<ActionResult<Caminhao>> ObterCaminhaoPorId(string id) => await _serviceCaminhao.ObterCaminhaoPorId(id);

        [HttpPost("cadastrarcaminhao")]
        public async Task<ActionResult<Caminhao>> CadastrarCaminhao(Caminhao caminhao) => await _serviceCaminhao.CadastrarCaminhao(caminhao);

        [HttpPut("atualizarcaminhao")]
        public async Task<ActionResult<Caminhao>> AtualizarCaminhao(Caminhao caminhao) => await _serviceCaminhao.AtualizarCaminhao(caminhao);

        [HttpDelete("removercaminhao")]
        public async Task<ActionResult<bool>> RemoverCaminhao(string id) => await _serviceCaminhao.RemoverCaminhao(id);
    }
}
