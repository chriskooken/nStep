using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucumber.Core.Features
{
	public abstract class FeatureItem : Executable
	{
		public int LineNumber { get; set; }
	}
}
