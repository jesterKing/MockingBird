using Rhino;
using Rhino.Commands;
using Rhino.PlugIns;
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
            return Result.Success;
        }
        protected override Result RenderWindow(RhinoDoc doc, RunMode mode, bool fastPreview, RhinoView view, Rectangle rect, bool inWindow)
        {
            return Result.Success;
        }
    }
}