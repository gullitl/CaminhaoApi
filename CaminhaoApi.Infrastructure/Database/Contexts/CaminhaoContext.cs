using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CaminhaoApi.Domain.CaminhaoAggregate;
using Microsoft.Extensions.Logging;

namespace CaminhaoApi.Infrastructure.Database.Contexts
{
    public interface ICaminhaoContext : ICrudContext<Caminhao>
    { }

    public class CaminhaoContext : CrudContext<Caminhao>, ICaminhaoContext
    {
        public CaminhaoContext(ILogger<ICaminhaoContext> logger, DatabaseContext context)
            : base(logger, context)
        { }
    }
}
