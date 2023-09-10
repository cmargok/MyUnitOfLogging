namespace MyLoggingUnit
{
    public class TargetsActualConfigu
    {
        public TargetConfiguration ConsoleLog { get; set; } = new();
        public TargetConfiguration FileLog { get; set; } = new();
        public TargetConfiguration SeqLog { get; set; } = new() { IsActive = false };

        public bool AllAsync = false;

    }
}
