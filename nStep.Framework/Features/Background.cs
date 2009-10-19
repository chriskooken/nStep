using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.Framework.Execution;

namespace nStep.Framework.Features
{
	public class Background : StepSequence
	{
		public Background(IList<Step> steps)
			: base(steps)
		{ }

		public override void Execute(IProcessSteps stepProcessor, IProcessScenarioHooks hookProcessor, IFormatOutput outputFormatter)
		{
			if (Steps.Count > 0)
				outputFormatter.WriteBackgroundHeading(this);

			outputFormatter.SkippingSteps = false;
			foreach (var step in Steps)
			{
				step.Execute(stepProcessor, hookProcessor, outputFormatter);
			}

			outputFormatter.WriteLineBreak();
		}
	}
}