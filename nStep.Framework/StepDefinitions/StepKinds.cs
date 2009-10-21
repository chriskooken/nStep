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
			switch (value)
			{
				case "Given": case "given:":
					return StepKinds.Given;
				case "When": case "when:":
					return StepKinds.When;
				case "Then": case "then:":
					return StepKinds.Then;
				case "And": case "and:": case "But": case "but:":
					return 0;
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