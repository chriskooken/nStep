using System.Collections.Generic;
using System.Linq;
using nStep.Framework.Execution.Results;
using nStep.Framework.ScenarioHooks;

namespace nStep.Framework.Features
{
	public class Scenario : FeatureItem
	{
		#region Constructor

		public Scenario(IList<FeatureStep> steps)
			: base(steps)
		{ }

		#endregion

		#region Execution

		public override void Execute(StepMother stepMother, IFormatOutput outputFormatter)
		{
			ExecuteBeforeScenarioHooks(Tags, stepMother);
			if(Feature.Background != null)
				Feature.Background.Execute(stepMother, outputFormatter);
			outputFormatter.SkippingSteps = false;
			outputFormatter.WriteScenarioTitle(this);
			foreach (var step in Steps)
			{
				step.Execute(stepMother, outputFormatter);
			}
			ExecuteAfterScenarioHooks(Tags, stepMother, new ScenarioResult(null)); //TODO: Load an appropriate scenarioResult here...
			outputFormatter.WriteLineBreak();
		}

		#endregion
	}
}