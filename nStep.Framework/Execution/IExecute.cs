using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.Framework;

namespace nStep.Framework.Execution
{
	public interface IExecute
	{
		int LineNumber { get; }
		void Execute(IProcessSteps stepProcessor, IProcessScenarioHooks hookProcessor);
	}
}