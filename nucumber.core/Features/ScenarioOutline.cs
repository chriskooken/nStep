using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucumber.Core.Features
{
	public class ScenarioOutline : FeatureItem
	{
		public Table Examples { get; private set; }

		public ScenarioOutline(Table examples)
		{
			Examples = examples;
		}
	}
}
