using NLog.Config;

namespace MyLoggingUnit
{

    public class LogBuilder 
    {
        private bool IsLoggingActive;
        private bool DefaultPresets;
        private bool SettingsFromJson = false;

        private LoggersSet? MyLoggers;


        //propiedad final para Nlog
        private LoggingConfiguration NlogCustomConfiguration { get; set; }
        public bool IsDefault
        {
            get => DefaultPresets; 
            set => DefaultPresets = value;
        }

      //  public TargetsActualConfigu targetsActualConfigu;




        public LogBuilder()
        {
            Activate();
            NlogCustomConfiguration = new LoggingConfiguration();
          //  targetsActualConfigu = new TargetsActualConfigu();
        }
               

        public void JsonSettings()
        {
            SettingsFromJson = true;
        }


        public void SetLoggersFromJson(bool Activate, LoggersSet loggersSet) 
        {
            IsLoggingActive = Activate;

            if(loggersSet != null)
            {
                MyLoggers = new LoggersSet();

                if (loggersSet.Console.Active == true) 
                {
                    MyLoggers.Console = loggersSet.Console;
                    MyLoggers.Console.Name = !string.IsNullOrEmpty(loggersSet.Console.Name) ? loggersSet.Console.Name : $"ConsoleLogger_default";
                }
                if (loggersSet.File.Active == true)
                {
                    MyLoggers.File = loggersSet.File;
                    MyLoggers.File.Name = !string.IsNullOrEmpty(loggersSet.File.Name) ? loggersSet.File.Name : $"FileLogger_default";
                }
                if (loggersSet.Seq.Active == true)
                {
                    MyLoggers.Seq = loggersSet.Seq;
                    MyLoggers.Seq.Name = !string.IsNullOrEmpty(loggersSet.Seq.Name) ? loggersSet.Seq.Name : $"SeqLogger_default";
                }
            }
        
        }

        public LoggersSet GetLoggers() => MyLoggers!;












        public bool Json() => SettingsFromJson;
        public void Activate() => IsLoggingActive = true;
        public void Disactivate() => IsLoggingActive = false;

        public bool Status() => IsLoggingActive;


        public  LoggingConfiguration GetLoggingConfiguration()
        {

            return NlogCustomConfiguration;

        } 

        public LoggingConfiguration SetloggingConfiguration(LoggingConfiguration a) => NlogCustomConfiguration = a;
    }

}
