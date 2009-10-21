using System.Collections.Generic;
using nStep.Framework.Execution;

namespace nStep.Framework.Features
{
	public abstract class StepSequence : IExecute
	{
		public Feature Feature { get; internal set; }
		public string Title { get; set; }
		public int LineNumber { get; set; }
		public IList<Step> Steps { get; private set; }

		protected StepSequence(IList<Step> steps)
		{
			Steps = steps;
			foreach (var step in steps)
				step.StepSequence = this;
		}

		public abstract void Execute(IProcessSteps stepProcessor, IProcessScenarioHooks hookProcessor, IFormatOutput outputFormatter);
	}
}