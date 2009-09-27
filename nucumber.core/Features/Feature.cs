using System;
using System.Collections.Generic;
using Nucumber.Core.Parsers;
using Nucumber.Core.Parsers.DataStructures;
using Nucumber.Framework;
using System.Linq;

namespace Nucumber.Core.Features
{
	public class Feature
	{

		public IList<LineValue> SummaryLines { get; private set; }
		public Background Background { get; private set; }
		public IList<FeatureItem> Items { get; private set; }

		public Feature(IList<LineValue> summaryLines, Background background, IList<FeatureItem> items)
		{
			SummaryLines = summaryLines;
			Background = background;
			Items = items;
		} 

		public void RecursiveTreeLoad(SimpleTreeNode<LineValue> subtree, Scenario currentScenario)
		{

			if (subtree.Parent != null)
			{
				LoadHeading(subtree);

				LoadBackground(subtree);

				currentScenario = LoadScenario(subtree, currentScenario);
			}


			foreach (SimpleTreeNode<LineValue> node in subtree.Children)
			{
				RecursiveTreeLoad(node, currentScenario);
			}
		}

		void LoadBackground(SimpleTreeNode<LineValue> subtree)
		{
			var val = subtree.Value;
			if (subtree.Value.NodeType == "Background:")
			{
				Background.Title = subtree.Value.Text;
				Background.LineNumber = subtree.Value.LineNumber;
			}

			if (subtree.Parent.Value.NodeType == "Background:")
				Background.Steps.Add(new FeatureStep(val.NodeType.ToStepKind()) { FeatureLine = val.Text, LineNumber = val.LineNumber });
		}

		Scenario LoadScenario(SimpleTreeNode<LineValue> subtree, Scenario currentScenario)
		{
			if (currentScenario == null)
			{
				currentScenario = new Scenario();
				//scenarios.Add(currentScenario);
			}

			var val = subtree.Value;
			if (subtree.Value.NodeType == "Scenario:")
			{
				currentScenario = new Scenario();
				Items.Add(currentScenario);
				currentScenario.Title = subtree.Value.Text;
				currentScenario.LineNumber = subtree.Value.LineNumber;
			}

			if (subtree.Parent.Value.NodeType == "Scenario:")
				currentScenario.Steps.Add(new FeatureStep(val.NodeType.ToStepKind()) { FeatureLine = val.Text, LineNumber = val.LineNumber });

			return currentScenario;
		}

		void LoadHeading(SimpleTreeNode<LineValue> subtree)
		{
			if ((subtree.Parent.Value.NodeType == "Feature:") || (subtree.Value.NodeType == "Feature:"))
				SummaryLines.Add(subtree.Value);
		}

		public string Description { get; set; }

		public FeatureParts WhatIsAtLine(int lineNmber)
		{
			if (Background.LineNumber == lineNmber)
				return FeatureParts.Background;

			if (Items.Where(x => x.LineNumber == lineNmber).Any())
				return FeatureParts.Scenario;

			if (SummaryLines.Where(x => x.LineNumber == lineNmber).Any())
				return FeatureParts.Feature;

			throw new InvalidScenarioLineNumberException("There is nothing to execute on line: " + lineNmber);
		}

		public Scenario GetScenarioAt(int lineNmber)
		{
			var foundScenario = Items.Where(x => x is Scenario && x.LineNumber == lineNmber).Select(x => x as Scenario);
			if (foundScenario.Any())
				return foundScenario.First();

			throw new InvalidScenarioLineNumberException("There are no scenario definitions on line: " + lineNmber);
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