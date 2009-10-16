using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nStep.Core.Features
{
	public interface IAmTaggable
	{
		IEnumerable<string> Tags { get; }
	}
}
