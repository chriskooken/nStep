using System;
using Nucumber.Framework;

namespace Nucumber.Core.Features
{
	public class FeatureStep
	{
		public string FeatureLine { get; set; }
		public int LineNumber { get; set; }
		public string FeatureFileName { get; set; }
		public StepKinds Kind { get; set; }
	}
}