using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucumber.Core.Features
{
	public abstract class Executable
	{
		protected Executable()
		{
			Steps = new List<FeatureStep>();
		}

		public string Title { get; set; }
		public IList<FeatureStep> Steps { get; set; }
	}
}
