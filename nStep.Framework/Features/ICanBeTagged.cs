using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nStep.Framework.Features
{
	public interface ICanBeTagged
	{
		IEnumerable<string> Tags { get; }
	}
}
