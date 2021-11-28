using CaminhaoApi.Application.Services;
using CaminhaoApi.Domain.CaminhaoAggregate;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [HttpGet("getall")]
        public async Task<ActionResult<List<Caminhao>>> GetAll() => await _serviceCaminhao.ObterTodosCaminhoes();

        [HttpGet("getbyid")]
        public async Task<ActionResult<Caminhao>> GetById(string id)
        {
            Caminhao caminhao = await _serviceCaminhao.ObterCaminhaoPorId(id);

            if (caminhao == null)
                return NotFound("Caminhão não encontrado");

            return caminhao;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Caminhao>> Create(Caminhao caminhao)
        {
            Validator.ValidateObject(caminhao, new ValidationContext(caminhao, null, null), true);
            return await _serviceCaminhao.CadastrarCaminhao(caminhao);
        }

        [HttpPut("update")]
        public async Task<ActionResult<Caminhao>> Update(Caminhao caminhao)
        {
            Validator.ValidateObject(caminhao, new ValidationContext(caminhao, null, null), true);
            return await _serviceCaminhao.AtualizarCaminhao(caminhao);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            await _serviceCaminhao.RemoverCaminhao(id);
            return true;
        }
    }
}
