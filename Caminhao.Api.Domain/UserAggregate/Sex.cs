using System.ComponentModel;

namespace Caminhao.Api.Domain.UserAggregate
{
    public enum Sex
    {
        [Description("Male")]
        Male = 1,

        [Description("Female")]
        Female = 2
    }
}
