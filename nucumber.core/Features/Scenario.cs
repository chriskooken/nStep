using System.Collections.Generic;
using System.Linq;
using Nucumber.Framework;
using Nucumber.Framework.ScenarioHooks;

namespace Nucumber.Core.Features
{
	public class Scenario : FeatureItem
	{
		#region Properties

		public IEnumerable<string> Tags { get; set; }

		#endregion

		#region Constructor

		public Scenario(IList<FeatureStep> steps)
			: base(steps)
		{ }

		#endregion

		#region Execution

		public override void Execute(StepMother stepMother, IFormatOutput outputFormatter)
		{
			ExecuteBeforeScenarioHooks(Tags, stepMother);
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