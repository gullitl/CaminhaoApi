using CaminhaoApi.Domain.CaminhaoAggregate;
using CaminhaoApi.Infrastructure.Database.Contexts;
using Microsoft.Extensions.Logging;

namespace CaminhaoApi.Application.Services
{
    public interface ICaminhaoService : ICrudService<Caminhao>
    {
    }

    public class CaminhaoService : CrudService<Caminhao>, ICaminhaoService
    {
        public CaminhaoService(ILogger<CrudService<Caminhao>> logger, DatabaseContext context) : base(logger, context) { }
    }
}
