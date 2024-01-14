
using Crash.Changes;
using Crash.Handlers;
using Crash.Handlers.Plugins;
using CrashDefinitions.Create;
using CrashDefinitions.Recieve;
using Rhino.Display;
using Rhino.Geometry;
using System.Drawing;

namespace CrashDefinitions.Request
{
    /// <summary>
    ///     Describes a Request Change, which lets users request currently temporary changes
    /// </summary>
    public sealed class RequestChangeDefinition : IChangeDefinition
    {
        public string ChangeName => RequestChange.ChangeType;
        public IEnumerable<IChangeCreateAction> CreateActions { get; }
        public IEnumerable<IChangeRecieveAction> RecieveActions { get; }

        public RequestChangeDefinition()
        {
            CreateActions = new List<IChangeCreateAction>()
            {
                new RequestCreateAction()
            };
            RecieveActions = new List<IChangeRecieveAction>()
                             {
                                 new RequestRecieveAction()
                             };
        }


        private BoundingBox CachedBox { get; set; } = BoundingBox.Empty;

        public void Draw(DrawEventArgs drawArgs, DisplayMaterial material, IChange change)
        {
            if (change is not RequestChange requestChange)
            {
                return;
            }

            var crashDoc = CrashDocRegistry.GetRelatedDocument(drawArgs.RhinoDoc);

            foreach (var RhinoId in crashDoc.RealisedChangeTable.GetRhinoIds())
            {
                var rhinoObject = drawArgs.RhinoDoc.Objects.FindId(RhinoId);
                if (rhinoObject is null)
                {
                    return;
                }

                CachedBox = rhinoObject.Geometry.GetBoundingBox(Plane.WorldXY);
                CachedBox.Inflate(1.1);

                drawArgs.Display.DrawBox(CachedBox, material.Diffuse, 5);

                var topLeft = CachedBox.PointAt(1, 1, 1);
                drawArgs.Display.DrawDot(topLeft, requestChange.Owner, Color.White, Color.Black);
            }
        }

        public BoundingBox GetBoundingBox(IChange change)
        {
            return CachedBox;
        }
    }
}
