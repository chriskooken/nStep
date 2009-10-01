using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.Framework;

namespace nStep.Core.Features
{
	public interface IExecute
	{
		void Execute(StepMother stepMother, IFormatOutput outputFormatter);
	}
}
