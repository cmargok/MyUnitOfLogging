using System.ComponentModel;

namespace MyLoggingUnit.Tools.Enums
{
    public enum LoggingTarget
    {
        [field: Description("None")]
        None = 0,

        [field: Description("Console")]
        Console = 1,

        [field: Description("File")]
        File = 2,

        [field: Description("ErrorFile")]
        ErrorFile = 3,

        [field: Description("Seq")]
        Seq = 4,

        [field: Description("ElasticSearch")]
        ElasticSearch = 5,

        [field: Description("Database")]
        Database = 6,

        [field: Description("RabbitMQ")]
        RabbitMQ = 7,
    }
}
