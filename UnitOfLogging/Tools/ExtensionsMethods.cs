using System.ComponentModel;
using UnitOfLogging.Models.ExplicitConfiguration;

namespace UnitOfLogging.Tools
{
    public static class ExtensionsMethods
    {
        public static string GetDescription(this Enum enumType)
        {
            var type = enumType.GetType();

            var memInfo = type.GetMember(enumType.ToString());

            if (memInfo.Length > 0)
            {
                var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return enumType.ToString();
        }

        public static bool IsIn(this Enum enumType, string targetString)
        {
            if (Enum.IsDefined(typeof(Enum), targetString))
            {
                return true;
            }

            return false;
        }

        public static LoggingTarget MapToLoggingTarget(string targetString)
        {
            if (Enum.TryParse(targetString, true, out LoggingTarget target))
            {
                return target;
            }

            return LoggingTarget.None;
        }

        public static string GetKey(Dictionary<LoggingTarget, string> _LoggersNames, LoggingTarget target)
        {
            if (!_LoggersNames.TryGetValue(target, out var key))
            {
                key = $"{target}Logger_default";
                _LoggersNames.Add(target, key);
            }
            return key;
        }
    }



}
