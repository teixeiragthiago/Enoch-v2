using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoch.CrossCutting.LogWriter
{
    public static class LogWriter
    {
        public static void WriteInfo(string info)
        {
            ConfigLog();

            Log.Information(info);
        }

        public static void WriteError(string error)
        {
            ConfigLog();

            Log.Error(error);
        }

        private static void ConfigLog() =>
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
    }
}
