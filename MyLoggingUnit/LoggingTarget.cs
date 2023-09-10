using System.ComponentModel;

namespace MyLoggingUnit
{
    public enum LoggingTarget
    {
        [field: Description("None")]
        None = 0,

        [field: Description("Console")]
        Console = 1,

        [field: Description("File")]
        File = 2,

        [field: Description("Seq")]
        Seq = 3,

        [field: Description("ElasticSearch")]
        ElasticSearch = 4,

        [field: Description("Database")]
        Database = 5,
    }
}
