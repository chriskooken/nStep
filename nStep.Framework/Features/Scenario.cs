using System.Collections.Generic;
using System.Linq;
using nStep.Framework.Execution;
using nStep.Framework.Execution.Results;
using nStep.Framework.ScenarioHooks;

namespace nStep.Framework.Features
{
	public class Scenario : FeatureItem
	{
		#region Constructor

		public Scenario(IList<Step> steps)
			: base(steps)
		{ }

		#endregion

		#region Execution

		public override void Execute(IProcessSteps stepProcessor, IProcessScenarioHooks hookProcessor, IFormatOutput outputFormatter)
		{
			hookProcessor.ProcessBeforeScenarioHooks(Tags);
			if(Feature.Background != null)
				Feature.Background.Execute(stepProcessor, hookProcessor, outputFormatter);
			outputFormatter.SkippingSteps = false;
			outputFormatter.WriteScenarioTitle(this);
			foreach (var step in Steps)
			{
				step.Execute(stepProcessor, hookProcessor, outputFormatter);
			}
			var result = new ScenarioResult(null); //TODO: Load an appropriate scenarioResult here...
			hookProcessor.ProcessAfterScenarioHooks(Tags, result);
			outputFormatter.WriteLineBreak();
		}

		#endregion
	}
}