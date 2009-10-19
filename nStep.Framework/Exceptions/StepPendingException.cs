using System;

namespace nStep.Framework.Exceptions
{
	public class StepPendingException : ApplicationException
	{
		public StepPendingException() : base("The StepDefinition is Pending.")
		{}

		public StepPendingException(string message)
			: base(message)
		{ }
	}
}