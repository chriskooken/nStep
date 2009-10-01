using System.Collections.Generic;

namespace nStep.Core.Features
{
	public abstract class StepSequence : IExecute
	{
		public string Title { get; set; }
		public IList<FeatureStep> Steps { get; private set; }

		protected StepSequence(IList<FeatureStep> steps)
		{
			Steps = steps;
		}

		public abstract void Execute(StepMother stepMother, IFormatOutput outputFormatter);
	}
}
