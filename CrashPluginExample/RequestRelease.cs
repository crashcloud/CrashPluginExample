﻿using Crash.Changes;
using Crash.Handlers;
using CrashDefinitions.Request;
using Rhino;
using Rhino.Commands;
using Rhino.Input;
using Rhino.Input.Custom;

namespace CrashPluginExample
{
    public class RequestRelease : Command
    {
        public RequestRelease()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static RequestRelease Instance { get; private set; }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName => "PleaseRelease";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var crashDoc = CrashDocRegistry.GetRelatedDocument(doc);
            if (crashDoc is null)
                return Result.Cancel;

            GetString getString = new GetString();
            getString.AcceptEnterWhenDone(true);
            getString.AcceptNothing(true);
            getString.SetWaitDuration(10_000);
            getString.SetCommandPrompt("Name");

            string userName = string.Empty;

            var result = GetResult.NoResult;
            while (result != GetResult.Timeout)
            {
                result = getString.Get();
                if (result == GetResult.Nothing)
                    break;

                else if (result == GetResult.String)
                {
                    userName = getString.StringResult();
                    break;
                }
            }

            if (string.IsNullOrEmpty(userName))
                return Result.Cancel;

            var args = new RequestEventArgs(crashDoc, userName);
            crashDoc.Dispatcher.NotifyServerAsync(ChangeAction.Add, this, args);

            return Result.Success;
        }
    }
}
