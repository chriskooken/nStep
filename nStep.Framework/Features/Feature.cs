using System;
using System.Collections.Generic;
using System.Linq;
using nStep.Framework.Exceptions;
using nStep.Framework.Execution;

namespace nStep.Framework.Features
{
	public class Feature : IExecute, ICanBeTagged
	{
		#region Public Properties

		public int LineNumber {
			get { return SummaryLines != null && SummaryLines.Count > 0 ? SummaryLines.First().LineNumber : 1; }
		}

		public IEnumerable<string> Tags { get; set; }
		public IList<LineValue> SummaryLines { get; private set; }
		public Background Background { get; private set; }
		public IList<FeatureItem> Items { get; private set; }
		public string Description { get; set; }
		public string FileName { get; set; }

		#endregion

		#region Constructor

		public Feature(IList<LineValue> summaryLines, Background background, IList<FeatureItem> items, IEnumerable<string> tags)
		{
			SummaryLines = summaryLines;
			Background = background;
			Items = items;
			Tags = tags;

			Background.Feature = this;
			foreach (var item in items)
				item.Feature = this;
		}

		#endregion

		#region Execution

		public void Execute(IProcessSteps stepProcessor, IProcessScenarioHooks hookProcessor, IFormatOutput outputFormatter)
		{
			outputFormatter.WriteFeatureHeading(this);

			foreach (var item in Items)
			{
				item.Execute(stepProcessor, hookProcessor, outputFormatter);
			}
		}

		public void Execute(IProcessSteps stepProcessor, IProcessScenarioHooks hookProcessor, IFormatOutput outputFormatter, int lineNumber)
		{
			GetExecutableAt(lineNumber).Execute(stepProcessor, hookProcessor, outputFormatter);
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

			throw new InvalidExecutableLineNumberException("There is nothing to execute on line: " + lineNumber);
		}

		#endregion
	}

	public class LineValue
	{
		public int LineNumber { get; set; }
		public string Text { get; set; }
	}
}