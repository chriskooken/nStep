using System;

namespace nStep.Framework.StepDefinitions
{
	public enum StepKinds
	{
		Given,
		When,
		Then
	}

	public static class StepKindExtensions
	{
		public static StepKinds ToStepKind(this string value)
		{
			switch (value.Trim().ToUpper())
			{
				case "GIVEN":
					return StepKinds.Given;
					break;
				case "WHEN":
					return StepKinds.When;
				case "THEN":
					return StepKinds.Then;
				default:
					throw new InvalidOperationException("Value not found in enumeration.");
			}
		}


		public static string ToStringValue(this StepKinds val)
		{
			switch (val)
			{
				case StepKinds.Given:
					return "Given";
				case StepKinds.When:
					return "When";
				case StepKinds.Then:
					return "Then";
				default:
					throw new InvalidOperationException("Value not found in enumeration.");
			}
		}
	}
}