using System.Text.Json;
using Crash.Changes;
using Crash.Handlers.Plugins;
using CrashDefinitions;
using CrashDefinitions.Request;

namespace CrashDefinitions.Create
{
    public class RequestCreateAction : IChangeCreateAction
    {
        public ChangeAction Action => ChangeAction.Add;

        public bool TryConvert(object sender, CreateRecieveArgs crashArgs, out IEnumerable<Change> changes)
        {
            if (crashArgs.Args is not RequestEventArgs requestArgs)
            {
                changes = Array.Empty<Change>();
                return false;
            }

            var packet = new PayloadPacket();
            packet.Updates.Add(RequestChange.RequestedNameKey, requestArgs.RequestedNamee);
            var requestChange = new Change
            {
                Id = Guid.NewGuid(),
                Action = ChangeAction.Add,
                Payload = JsonSerializer.Serialize(packet),
                Stamp = DateTime.UtcNow
            };

            changes = new List<Change>
            {
                requestChange
            };

            return true;
        }

        public bool CanConvert(object sender, CreateRecieveArgs crashArgs)
        {
            return crashArgs.Args is RequestEventArgs;
        }
    }
}
