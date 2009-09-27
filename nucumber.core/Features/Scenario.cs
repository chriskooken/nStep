using System.Collections.Generic;

namespace Nucumber.Core.Features
{
	public class Scenario
	{
		public Scenario()
		{
			Steps = new List<FeatureStep>();
		}

		public string Title { get; set; }
		public int LineNumber { get; set; }
		public IList<FeatureStep> Steps { get; set; }
	}
}