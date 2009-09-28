using System;
using System.Collections.Generic;

namespace Nucumber.Core.Features
{
	public class Scenario : FeatureItem
	{
	    public IEnumerable<string> Tags { get; set; }
	}
}