using Serilog;
using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public static class MySinkExtensions
    {
        public static LoggerConfiguration MySink(this LoggerSinkConfiguration loggerSinkConfiguration, IFormatProvider formatProvider = null)
        {

            return loggerSinkConfiguration.Sink(new MySink(formatProvider));
        }
    }
}
