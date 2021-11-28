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
        private readonly ICaminhaoService _caminhaoService;

        public CaminhaoController(ICaminhaoService userService)
        {
            _caminhaoService = userService;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<List<Caminhao>>> GetAll() => Ok(await _caminhaoService.GetAll());

        [HttpGet("getbyid")]
        public async Task<ActionResult<Caminhao>> GetById(string id)
        {
            Caminhao caminhao = await _caminhaoService.GetById(id);

            if (caminhao == null)
                return NotFound("Caminhão não encontrado");

            return caminhao;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Caminhao>> Create(Caminhao caminhao)
        {
            Validator.ValidateObject(caminhao, new ValidationContext(caminhao, null, null), true);
            return await _caminhaoService.Create(caminhao);
        }

        [HttpPut("update")]
        public async Task<ActionResult<Caminhao>> Update(Caminhao caminhao)
        {
            Validator.ValidateObject(caminhao, new ValidationContext(caminhao, null, null), true);
            return await _caminhaoService.Update(caminhao);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<bool>> Delete(string id) => await _caminhaoService.Delete(id);
    }
}
