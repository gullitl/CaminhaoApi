using System.ComponentModel;

namespace CaminhaoApi.Domain.CaminhaoAggregate
{
    public enum Modelo
    {
        [Description("FH")]
        FH = 1,

        [Description("FM")]
        FM = 2,

        [Description("SU")]
        SU = 3,

        [Description("TR")]
        TR = 4,

        [Description("RA")]
        RA = 5,

        [Description("WA")]
        WA = 6,

        [Description("BU")]
        BU = 7,

        [Description("PS")]
        PS = 8,

        [Description("CE")]
        CE = 9,

        [Description("ZQ")]
        ZQ = 10

    }
}
