using System.Drawing;
using System.Threading;
using Rhino;
using Rhino.Commands;
using Rhino.Display;
using Rhino.PlugIns;
using Rhino.Render;

namespace MockingBirdChangeQueue
{
	public class MockingRenderPipeline : RenderPipeline
	{
		private readonly MockingRenderContext m_rc;
		public MockingRenderPipeline(RhinoDoc doc, RunMode mode, RenderPlugIn plugin, MockingRenderContext rc)
			: base(doc, mode, plugin, RenderSize(doc),
					"MockingBird (modal)", Rhino.Render.RenderWindow.StandardChannels.RGBA, false, false)
		{
			m_rc = rc;
		}


		protected override bool OnRenderBegin()
		{
			m_rc.Thread = new Thread(m_rc.Renderer)
			{
				Name = "MockingBird Modal Rendering thread"
			};
			m_rc.Thread.Start();
			return true;
		}

		protected override bool OnRenderBeginQuiet(Size imageSize)
		{
			return OnRenderBegin();
		}

		protected override bool OnRenderWindowBegin(RhinoView view, Rectangle rectangle)
		{
			return false;
		}

		protected override void OnRenderEnd(RenderEndEventArgs e)
		{
			// stop render engine here.
			m_rc.StopRendering();
		}

		protected override bool ContinueModal()
		{
			return !m_rc.Done;
		}
	}
}
