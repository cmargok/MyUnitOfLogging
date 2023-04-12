namespace UnitOfLogging.Core.TargetConf
{
    public class TargetsOptions
    {
        public bool ConsoleLog { get; set; } = true;
        public bool FileLog { get; set; } = true;
        public bool SeqLog { get; set; } = true;

    }

 


    public class TargetsOptionsAsync : TargetsOptions
    {
        public bool AllAsync = false;
    }




    public class TargetsActualConfigu
    {
        public TargetConfiguration ConsoleLog { get; set; } = new();
        public TargetConfiguration FileLog { get; set; } = new();
        public TargetConfiguration SeqLog { get; set; } = new() { IsActive = false };

        public bool AllAsync = false;

    }

    public class TargetConfiguration
    {
        public bool IsActive { get; set; } = true;
        public bool Async { get; set; } = false;
    }

}
