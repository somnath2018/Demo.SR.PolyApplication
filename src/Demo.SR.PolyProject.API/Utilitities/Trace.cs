using Microsoft.Extensions.Logging;
using System.IO;
using System.Runtime.CompilerServices;

namespace Demo.SR.PolyProject.API.Utilitities
{
    public class Trace
    {
        public static void TraceMessage(ILogger _logger, string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            //_logger.LogInformation($"{message} {memberName} {sourceFilePath}:{sourceLineNumber}");
            _logger.LogInformation($"{Path.GetFileName(sourceFilePath)}:{sourceLineNumber} {message}");


        }
    }
}
