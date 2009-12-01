using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.Framework.Execution.Results;
using nStep.Framework.Steps;

namespace nStep.Framework.Execution
{
	public interface IProcessSteps
	{
		StepRunResult ProcessStep(Step step);
	    void CheckForMissingStep(Step featureStep);
	}
}
