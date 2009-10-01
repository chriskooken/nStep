using System;
using nStep.Framework;

namespace nStep.Core.Features
{
	public class FeatureStep : IExecute
	{
		public string FeatureLine { get; set; }
		public int LineNumber { get; set; }
		public string FeatureFileName { get; set; }
		public StepKinds Kind { get; private set; }

		public FeatureStep(StepKinds kind)
		{
			Kind = kind;
		}

		public void Execute(StepMother stepMother, IFormatOutput outputFormatter)
		{
            stepMother.ChekForMissingStep(this);

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
	}
}