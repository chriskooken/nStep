using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.Framework.Execution;
using nStep.Framework.Execution.Results;

namespace nStep.Framework.Features
{
	public class ScenarioOutline : FeatureItem
	{
		public Table Examples { get; private set; }

		public ScenarioOutline(IList<Step> steps, Table examples)
			: base(steps)
		{
			Examples = examples;
		}

		public override void Execute(IProcessSteps stepProcessor, IProcessScenarioHooks hookProcessor, IFormatOutput outputFormatter)
		{
			hookProcessor.ProcessBeforeScenarioHooks(Tags);
			outputFormatter.SkippingSteps = false;
			outputFormatter.WriteScenarioOutlineTitle(this);
			foreach (var dictionary in Examples.GetDictionaries())
			{
				if (Feature.Background != null)
					Feature.Background.Execute(stepProcessor, hookProcessor, outputFormatter);

				foreach (var step in Steps)
					step.Execute(stepProcessor, hookProcessor, outputFormatter, dictionary);
			}
			var result = new ScenarioResult(null); //TODO: Load an appropriate scenarioResult here...
			hookProcessor.ProcessAfterScenarioHooks(Tags, result);
			outputFormatter.WriteLineBreak();
		}
	}
}