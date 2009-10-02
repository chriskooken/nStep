using System;
using System.Collections.Generic;
using System.Linq;

namespace nStep.Core.Features
{
	public class Feature : IExecute
	{

		public IList<LineValue> SummaryLines { get; private set; }
		public Background Background { get; private set; }
		public IList<FeatureItem> Items { get; private set; }
		public string Description { get; set; }

		public Feature(IList<LineValue> summaryLines, Background background, IList<FeatureItem> items)
		{
			SummaryLines = summaryLines;
			Background = background;
			Items = items;

			foreach (var item in items)
				item.Feature = this;
		} 

		public void Execute(StepMother stepMother, IFormatOutput outputFormatter)
		{
			outputFormatter.WriteFeatureHeading(this);

			foreach (var item in Items)
			{
				item.Execute(stepMother, outputFormatter);
			}
		}

		public void Execute(StepMother stepMother, IFormatOutput outputFormatter, int lineNumber)
		{
			GetExecutableAt(lineNumber).Execute(stepMother, outputFormatter);
		}

		private IExecute GetExecutableAt(int lineNumber)
		{
			// Return entire feature if any SummaryLine is selected
			foreach (var summaryLine in SummaryLines)
				if (summaryLine.LineNumber == lineNumber)
					return this;

			// Test for Background header
			if (Background.LineNumber == lineNumber)
				return Background;

			// Test for FeatureItem headers
			foreach (var featureItem in Items)
				if (featureItem.LineNumber == lineNumber)
					return featureItem;

			// TODO: Test for Scenario Outline examples

			throw new InvalidScenarioLineNumberException("There is nothing to execute on line: " + lineNumber);
		}
	}

	public class LineValue
	{
		public int LineNumber { get; set; }
		public string Text { get; set; }
		[Obsolete("Will go away soon")]
		public string NodeType { get; set; }
	}
}