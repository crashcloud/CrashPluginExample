using Crash.Common.Document;
using Crash.Common.Events;

namespace Crash.Handlers.Plugins.Request
{
    public sealed class RequestEventArgs : CrashEventArgs
    {
        public RequestEventArgs(CrashDoc crashDoc, string requestedName) : base(crashDoc)
        {
            RequestedNamee = requestedName;
        }

        public readonly string RequestedNamee;
    }
}
