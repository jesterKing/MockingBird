using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Input;
using Rhino.Input.Custom;

namespace MockingBirdModal
{
	public class MockingBirdModalCommand : Command
	{
		public MockingBirdModalCommand()
		{
			// Rhino only creates one instance of each command class defined in a
			// plug-in, so it is safe to store a refence in a static property.
			Instance = this;
		}

		///<summary>The only instance of this command.</summary>
		public static MockingBirdModalCommand Instance
		{
			get; private set;
		}

		///<returns>The command name as it appears on the Rhino command line.</returns>
		public override string EnglishName
		{
			get { return "MockingBirdModalCommand"; }
		}

		protected override Result RunCommand(RhinoDoc doc, RunMode mode)
		{
			// Usually commands in rendering plug-ins are used to modify settings and behavior.
			// The rendering work itself is performed by the MockingBirdModalPlugIn class.

			return Result.Success;
		}
	}
}
