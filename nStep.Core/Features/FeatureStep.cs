using System;
using System.Collections.Generic;
using nStep.Framework;

namespace nStep.Core.Features
{
	public class FeatureStep : IExecute
	{
		public string FeatureLine { get; set; }
		public int LineNumber { get; set; }
		public StepKinds Kind { get; private set; }

		public FeatureStep(StepKinds kind)
		{
			Kind = kind;
		}

		private FeatureStep(FeatureStep originalStep)
		{
			FeatureLine = originalStep.FeatureLine;
			LineNumber = originalStep.LineNumber;
			Kind = originalStep.Kind;
		}

		public void Execute(StepMother stepMother, IFormatOutput outputFormatter)
		{
            stepMother.CheckForMissingStep(this);

            if (outputFormatter.SkippingSteps)
            {
                outputFormatter.WriteSkippedFeatureLine(this);
                return;
            }
            outputFormatter.SkippingSteps = true;
			switch (stepMother.ProcessStep(this))
			{
				case StepRunResults.Passed:
					outputFormatter.SkippingSteps = false;
					outputFormatter.WritePassedFeatureLine(this, stepMother.LastProcessStepDefinition);
					break;
				case StepRunResults.Failed:
					outputFormatter.WriteException(this, stepMother.LastProcessStepException);
					break;
				case StepRunResults.Pending:
					outputFormatter.WritePendingFeatureLine(this);
					break;
				case StepRunResults.Missing:
					outputFormatter.WritePendingFeatureLine(this);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void Execute(StepMother stepMother, IFormatOutput outputFormatter, IDictionary<string, string> dictionary)
		{
			var newLine = FeatureLine;

			foreach (var key in dictionary.Keys)
				newLine = newLine.Replace("<" + key + ">", dictionary[key]);

			var newFeatureStep = new FeatureStep(this) { FeatureLine = newLine };
			newFeatureStep.Execute(stepMother, outputFormatter);
		}
	}
}