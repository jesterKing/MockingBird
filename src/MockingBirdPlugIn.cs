using Rhino;
using Rhino.Commands;
using Rhino.PlugIns;
using Rhino.Render;
using Rhino.Display;
using System.Drawing;
namespace MockingBird
{
    public class MockingBirdPlugIn : RenderPlugIn
    {
        public MockingBirdPlugIn()
        {
            if (Instance == null) Instance = this;
        }
        public static MockingBirdPlugIn Instance
        {
            get; private set;
        }
        protected override Result Render(RhinoDoc doc, RunMode mode, bool fastPreview)
        {
            Rectangle rect = new Rectangle(new Point(0, 0), RenderPipeline.RenderSize(doc, true));
            return RenderWithMockingBird(doc, mode, rect, true);
        }
        protected override Result RenderWindow(RhinoDoc doc, RunMode mode, bool fastPreview, RhinoView view, Rectangle rect, bool inWindow)
        {
            return RenderWithMockingBird(doc, mode, rect, inWindow);
        }
        private Result RenderWithMockingBird(RhinoDoc doc, RunMode mode, Rectangle rect, bool inWindow)
        {
            var rc = RenderPipeline.RenderReturnCode.InternalError;
            using (var rsv = new RenderSourceView(doc))
            {
                << find view >>

                << setup modal render engine >>

                << initialize render pipeline>>

                << engine creates world>>

                << render modal or in view >>

                << clean - up >>

                return Result.Success;
            }
        }
    }
}