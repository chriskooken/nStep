using System;
using System.Collections.Generic;
using System.Linq;
using nStep.Core.Exceptions;

namespace nStep.Core.Features
{
	public class Feature : IExecute
	{
		#region Public Properties

		public int LineNumber {
			get { return SummaryLines != null && SummaryLines.Count > 0 ? SummaryLines.First().LineNumber : 1; }
		}
		
		public IList<LineValue> SummaryLines { get; private set; }
		public Background Background { get; private set; }
		public IList<FeatureItem> Items { get; private set; }
		public string Description { get; set; }

		#endregion

		#region Constructor

		public Feature(IList<LineValue> summaryLines, Background background, IList<FeatureItem> items)
		{
			SummaryLines = summaryLines;
			Background = background;
			Items = items;

			foreach (var item in items)
				item.Feature = this;
		}

		#endregion

		#region Execution

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
			{
				if (featureItem.LineNumber == lineNumber)
					return featureItem;

				// Test ScenarioOutline examples
				//var scenarioOutline = featureItem as ScenarioOutline;
				//if (scenarioOutline != null)
				//    foreach (var example in scenarioOutline.Examples.Rows)
				//        if (example.LineNumber == lineNumber)
				//            return example;
			}

			throw new InvalidScenarioLineNumberException("There is nothing to execute on line: " + lineNumber);
		}

		#endregion
	}

	public class LineValue
	{
		public int LineNumber { get; set; }
		public string Text { get; set; }
	}
}