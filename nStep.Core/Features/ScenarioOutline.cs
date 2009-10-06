using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.Framework;

namespace nStep.Core.Features
{
	public class ScenarioOutline : FeatureItem
	{
		public Table Examples { get; private set; }

		public ScenarioOutline(IList<FeatureStep> steps, Table examples)
			: base(steps)
		{
			Examples = examples;
		}

		public override void Execute(StepMother stepMother, IFormatOutput outputFormatter)
		{
			ExecuteBeforeScenarioHooks(Tags, stepMother);
			Feature.Background.Execute(stepMother, outputFormatter);
			outputFormatter.SkippingSteps = false;
			outputFormatter.WriteScenarioOutlineTitle(this);
			foreach (var dictionary in Examples.GetDictionaries())
				foreach (var step in Steps)
					step.Execute(stepMother, outputFormatter, dictionary);
			ExecuteAfterScenarioHooks(Tags, stepMother, new ScenarioResult(null)); //TODO: Load an appropriate scenarioResult here...
			outputFormatter.WriteLineBreak();
		}
	}
}
