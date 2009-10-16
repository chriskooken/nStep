using System.Collections.Generic;
using nStep.Framework.Execution;

namespace nStep.Framework.Features
{
	public abstract class StepSequence : IExecute
	{
		public string Title { get; set; }
		public int LineNumber { get; set; }
		public IList<Step> Steps { get; private set; }

		protected StepSequence(IList<Step> steps)
		{
			Steps = steps;
		}

		public abstract void Execute(StepMother stepMother, IFormatOutput outputFormatter);
	}
}