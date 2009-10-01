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
	}

	public class LineValue
	{
		public int LineNumber { get; set; }
		public string Text { get; set; }
		[Obsolete("Will go away soon")]
		public string NodeType { get; set; }
	}
}