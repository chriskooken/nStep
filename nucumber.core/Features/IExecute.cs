using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nucumber.Framework;

namespace Nucumber.Core.Features
{
	public interface IExecute
	{
		void Execute(StepMother stepMother, IFormatOutput outputFormatter);
	}
}
