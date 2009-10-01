using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			// TODO: Stuff
			return;
		}
	}
}
