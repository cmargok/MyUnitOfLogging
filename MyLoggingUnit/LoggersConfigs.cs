﻿using System.ComponentModel.DataAnnotations;

namespace MyLoggingUnit
{
    public class LoggersConfigs
    {
        [Required]
        public bool Active { get; set; } = false;


        [Required]
        [MinLength(1)]
        public string Name { get; set; } = string.Empty;

        public LoggingTarget Target { get; set; } = LoggingTarget.None;

        //public bool Async { get; set; } = false;
    }
}
