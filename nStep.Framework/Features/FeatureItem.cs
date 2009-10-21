using System.Collections.Generic;
using System.Linq;
using nStep.Framework.Execution.Results;
using nStep.Framework.ScenarioHooks;

namespace nStep.Framework.Features
{
	public abstract class FeatureItem : StepSequence, ICanBeTagged
	{
		#region Properties

		public Feature Feature { get; internal set; }
		public IEnumerable<string> Tags { get; set; }

		#endregion

		protected FeatureItem(IList<Step> steps)
			: base(steps)
		{
		}
	}
}