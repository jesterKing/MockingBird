using System.Drawing;
using Rhino;
using Rhino.Commands;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.PlugIns;
using Rhino.Render;

namespace MockingBirdModal
{
	public class MockingBirdModalPlugIn : RenderPlugIn

	{
		public MockingBirdModalPlugIn()
		{
			if(Instance == null) Instance = this;
		}

		public static MockingBirdModalPlugIn Instance
		{
			get; private set;
		}

		protected override Result Render(RhinoDoc doc, RunMode mode, bool fastPreview)
		{
			// initialise our render context
			MockingRenderContext rc = new MockingRenderContext();

			// initialise our pipeline implementation
			RenderPipeline pipeline = new MockingRenderPipeline(doc, mode, this, rc);

			// query for render resolution
			var renderSize = RenderPipeline.RenderSize(doc);

			// set up view info
			ViewInfo viewInfo = new ViewInfo(doc.Views.ActiveView.ActiveViewport);

			// set up render window
			rc.RenderWindow = pipeline.GetRenderWindow();
			// add a wireframe channel for curves/wireframes/annotation etc.
			rc.RenderWindow.AddWireframeChannel(doc, viewInfo.Viewport, renderSize, new Rectangle(0, 0, renderSize.Width, renderSize.Height));
			// set correct size
			rc.RenderWindow.SetSize(renderSize);

			// now fire off render thread.
			var renderCode = pipeline.Render();

			// note that the rendering isn't complete yet, rather the pipeline.Render()
			// call starts a rendering thread. Here we essentially check whether
			// starting that thread went ok.
			if (renderCode != RenderPipeline.RenderReturnCode.Ok)
			{
				RhinoApp.WriteLine("Rendering failed:" + rc.ToString());
				return Result.Failure;
			}

			// all ok, so we are apparently rendering.
			return Result.Success;
		}

		protected override Result RenderWindow(RhinoDoc doc, RunMode mode, bool fastPreview, RhinoView view, Rectangle rect, bool inWindow)
		{
			return Result.Success;
		}

	}
}