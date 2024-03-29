using Crash.Changes;
using Crash.Changes.Extensions;
using Crash.Common.Document;
using Crash.Handlers.Plugins;
using CrashDefinitions;
using CrashDefinitions.Request;

namespace CrashDefinitions.Recieve
{
    public class RequestRecieveAction : IChangeRecieveAction
    {
        public bool CanRecieve(IChange change)
        {
            return change.HasFlag(ChangeAction.Add);
        }

        public Task OnRecieveAsync(CrashDoc crashDoc, Change recievedChange)
        {
            if (!RequestChange.TryCreateFromPayload(recievedChange, out var requestChange))
            {
                return Task.CompletedTask;
            }

            crashDoc.TemporaryChangeTable.UpdateChange(requestChange);

            return Task.CompletedTask;
        }
    }
}
