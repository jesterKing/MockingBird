using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Render.ChangeQueue;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Render;
using Light = Rhino.Render.ChangeQueue.Light;
using Material = Rhino.Render.ChangeQueue.Material;
using Mesh = Rhino.Render.ChangeQueue.Mesh;

namespace MockingBirdChangeQueue
{
	public class MockingChangeQueue : ChangeQueue
	{
		// for a regular rhino document (i.e. currently
		// active)
		// The constructor can look like you want, as long as the plugin ID,
		// document serial number and view info are given, needed for
		// the base class
		public MockingChangeQueue(Guid pluginId, uint docRuntimeSerialNumber, ViewInfo viewinfo)
			: base(pluginId, docRuntimeSerialNumber, viewinfo)
		{
		}

		protected override void ApplyViewChange(ViewInfo viewInfo)
		{
			var vp = viewInfo.Viewport;
			int near, far;
			var screenport = vp.GetScreenPort(out near, out far);
			RhinoApp.WriteLine($"Camera @ {vp.CameraLocation}, direction {vp.CameraDirection}");
			RhinoApp.WriteLine($"\twith near {near} and far {far}");
			RhinoApp.WriteLine($"\tand {screenport}");
		}

		protected override void ApplyEnvironmentChanges(RenderEnvironment.Usage usage)
		{
			// background/skylight/reflection environments
			var env = EnvironmentForid(EnvironmentIdForUsage(usage));
			RhinoApp.WriteLine(env != null ? $"{usage} {env.Name}" : $"No env for {usage}");
		}

		protected override void ApplyLightChanges(List<Light> lightChanges)
		{
			foreach (var light in lightChanges)
			{
				RhinoApp.WriteLine($"A {light.ChangeType} light. {light.Data.Name}, {light.Data.LightStyle}");
				if (light.Data.LightStyle == LightStyle.CameraDirectional)
				{
					RhinoApp.WriteLine("Use ChangeQueue.ConvertCameraBasedLightToWorld() to convert light transform to world");
					RhinoApp.WriteLine($"\told location {light.Data.Location}, direction {light.Data.Direction}");
					ConvertCameraBasedLightToWorld(this, light, GetQueueView());
					RhinoApp.WriteLine($"\tnew location {light.Data.Location}, direction {light.Data.Direction}");
				}
			}
		}

		/// <summary>
		/// Get all geometry data.
		/// </summary>
		/// <param name="deleted"></param>
		/// <param name="added"></param>
		protected override void ApplyMeshChanges(Guid[] deleted, List<Mesh> added)
		{
			RhinoApp.WriteLine($"Received {added.Count} new meshes, {deleted.Length} for deletion");
			foreach (var m in added)
			{
				var totalVerts = 0;
				var totalFaces = 0;
				var totalQuads = 0;
				var mesh_index = 0;
				RhinoApp.WriteLine($"\t{m.Id()} with {m.GetMeshes().Length} submeshes");
				foreach (var sm in m.GetMeshes())
				{
					RhinoApp.WriteLine($"\t\tmesh index {mesh_index} mesh with {sm.Vertices.Count} verts, {sm.Faces.Count} faces ({sm.Faces.QuadCount} quads).");
					totalVerts += sm.Vertices.Count;
					totalFaces += sm.Faces.Count;
					totalQuads += sm.Faces.QuadCount;
					RhinoApp.WriteLine($"\t\tFor material we remember ({m.Id()},{mesh_index}) as identifier. Connect dots in ApplyMeshInstanceChanged");
					mesh_index++;
				}
				RhinoApp.WriteLine($"\t{totalVerts} verts, {totalFaces} faces (of which {totalQuads} quads)");
			}
		}

		protected override void ApplyMeshInstanceChanges(List<uint> deleted, List<MeshInstance> addedOrChanged)
		{
			RhinoApp.WriteLine($"Received {addedOrChanged.Count} mesh instances to be either added or changed");
			foreach (var mi in addedOrChanged)
			{
				var mat = MaterialFromId(mi.MaterialId);
				RhinoApp.WriteLine($"\tAdd or change object {mi.InstanceId} uses mesh <{mi.MeshId}, {mi.MeshIndex}>, and material {mi.MaterialId}, named {mat.Name})");
			}
			// For single-shot rendering there won't be deletions.
		}
	}
}
