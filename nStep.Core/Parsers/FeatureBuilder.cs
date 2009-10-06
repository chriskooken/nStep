using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.Core.Features;
using PerCederberg.Grammatica.Runtime;
using nStep.Framework;

namespace nStep.Core.Parsers
{
	internal class FeatureBuilder : Generated.GherkinAnalyzer
	{
		#region Private Data

		private StepKinds CurrentStepKind { get; set; }

		#endregion

		#region Tokens

		public override Node ExitTextChar(Token node)
		{
			return EchoImage(node);
		}

		public override Node ExitHorizontalWhitespace(Token node)
		{
			return EchoImage(node);
		}

		public override Node ExitTFeature(Token node)
		{
			return EchoImage(node);
		}

		public override Node ExitTBackground(Token node)
		{
			return EchoImage(node);
		}

		public override Node ExitTScenario(Token node)
		{
			return EchoImage(node);
		}

		public override Node ExitTScenarioOutline(Token node)
		{
			return EchoImage(node);
		}

		public override Node ExitTExamples(Token node)
		{
			return EchoImage(node);
		}

		public override Node ExitTGiven(Token node)
		{
			return EchoImage(node);
		}

		public override Node ExitTWhen(Token node)
		{
			return EchoImage(node);
		}

		public override Node ExitTThen(Token node)
		{
			return EchoImage(node);
		}

		public override Node ExitTAnd(Token node)
		{
			return EchoImage(node);
		}

		public override Node ExitTBut(Token node)
		{
			return EchoImage(node);
		}

		public override Node ExitTag(Token node)
		{
			var text = node.Image.Substring(1);
			node.AddValue(text);
			return node;
		}

		private static Node EchoImage(Token node)
		{
			node.AddValue(node.Image);
			return node;
		}

		#endregion

		#region Pseudo-Tokens

		public override Node ExitFreeLine(Production node)
		{
			node.AddValues(GetChildAt(node, 0).Values);

			return node;
		}

		public override Node ExitText(Production node)
		{
			var text = string.Concat(GetChildValues(node).Cast<string>().ToArray());

			node.AddValue(text.Trim());

			return node;
		}

		public override Node ExitTags(Production node)
		{
			var tags = GetChildValues(node).Cast<string>().ToArray();
			node.AddValue(tags);
			return node;
		}

		#endregion

		#region Feature

		public override Node ExitFeature(Production node)
		{
			var values = GetChildValues(node);

			var tags = (IEnumerable<string>) null;
			int summaryLinesIndex = 0;
			if (values[0] is IEnumerable<string>)
			{
				tags = values[0] as IEnumerable<string>;
				summaryLinesIndex++;
			}

			var summaryLines = values[summaryLinesIndex] as IList<LineValue>;
			var background = values[summaryLinesIndex + 1] as Background;
			var featureIndex = summaryLinesIndex + (background == null ? 1 : 2);

			// Rest of values are FeatureItems
			var items = values.GetRange(featureIndex, values.Count - featureIndex).Cast<FeatureItem>().ToList();

			var feature = new Feature(summaryLines, background, items, tags)
			{
				// TODO: Should this get a value?
				Description = ""
			};

			node.AddValue(feature);
			return node;
		}

		public override Node ExitSummaryLine(Production node)
		{
			var text = GetChildValues(node).Cast<string>().SingleOrDefault();

			var lineValue = new LineValue
			{
				Text = text,
				LineNumber = node.StartLine
			};

			node.AddValue(lineValue);
			return node;
		}

		public override Node ExitFeatureHeader(Production node)
		{
			// Skip the first value, it's the string for T_FEATURE
			var values = GetChildValues(node);
			var summaryLines = values.GetRange(1, values.Count - 1).Cast<LineValue>().ToList();

			node.AddValue(summaryLines);
			return node;
		}

		public override Node ExitBackgroundHeader(Production node)
		{
			node.AddValue(GetTitle(node));
			return node;
		}

		public override Node ExitBackground(Production node)
		{
			var values = GetChildValues(node);

			// First value is either title from header or first FeatureStep
			var title = values[0] as string;
			var featureIndex = title == null ? 0 : 1;

			// Rest of values are FeatureSteps
			var steps = values.GetRange(featureIndex, values.Count - featureIndex).Cast<FeatureStep>().ToList();

			var background = new Background(steps)
			{
				Title = title,
				LineNumber = node.StartLine
			};

			node.AddValue(background);
			return node;
		}

		#endregion

		#region Scenario

		public override Node ExitScenarioHeader(Production node)
		{
			node.AddValue(GetTitle(node));
			return node;
		}

		public override Node ExitScenario(Production node)
		{
			var values = GetChildValues(node);

			// First value is title from header
			var title = values[0] as string;

			// Rest of values are FeatureSteps
			var steps = values.GetRange(1, values.Count - 1).Cast<FeatureStep>().ToList();

			var scenario = new Scenario(steps)
			{
				Title = title,
				LineNumber = node.StartLine
			};

			node.AddValue(scenario);
			return node;
		}

		#endregion

		#region Scenario Outline

		public override Node ExitScenarioOutlineHeader(Production node)
		{
			node.AddValue(GetTitle(node));
			return node;
		}

		public override Node ExitScenarioOutline(Production node)
		{
			var values = GetChildValues(node);

			// First value is title from header
			var title = values[0] as string;

			// Last value is example table
			var examples = values[values.Count - 1] as Table;

			// Rest of values are FeatureSteps
			var steps = values.GetRange(1, values.Count - 2).Cast<FeatureStep>().ToList();


			var scenarioOutline = new ScenarioOutline(steps, examples)
			{
				Title = title,
				LineNumber = node.StartLine
			};

			node.AddValue(scenarioOutline);
			return node;
		}

		public override Node ExitExamples(Production node)
		{
			var table = GetChildValues(node).Cast<Table>().Single();

			node.AddValue(table);
			return node;
		}

		#endregion

		#region Tables

		public override Node ExitTable(Production node)
		{
			var rows = GetChildValues(node).Cast<Row>().ToList();

			var table = new Table(rows);

			node.AddValue(table);
			return node;
		}

		public override Node ExitTableRow(Production node)
		{
			var columns = GetChildValues(node).Cast<Cell>().ToList();

			var row = new Row(columns);
			node.AddValue(row);
			return node;
		}

		public override Node ExitTableColumn(Production node)
		{
			var cell = GetChildValues(node).Cast<string>().Single().Trim();
			var column = new Cell(cell);
			node.AddValue(column);
			return node;
		}

		#endregion

		#region Steps

		public override Node ExitStep(Production node)
		{
			// May be blank or a real step -- 0 or 1 values
			node.AddValues(GetChildValues(node));
			return node;
		}

		public override Node ExitGiven(Production node)
		{
			return AttachFeatureStep(node, StepKinds.Given);
		}

		public override Node ExitWhen(Production node)
		{
			return AttachFeatureStep(node, StepKinds.When);
		}

		public override Node ExitThen(Production node)
		{
			return AttachFeatureStep(node, StepKinds.Then);
		}

		public override Node ExitAnd(Production node)
		{
			return AttachFeatureStep(node, CurrentStepKind);
		}

		public override Node ExitBut(Production node)
		{
			return AttachFeatureStep(node, CurrentStepKind);
		}

		private Node AttachFeatureStep(Node node, StepKinds kind)
		{
			var values = GetChildValues(node);
			var verbage = values.Cast<string>().First();
			var featureLine = values.GetRange(1, values.Count - 1).Cast<string>().Single();

			var featureStep = new FeatureStep(kind)
			{
				FeatureLine = verbage + " " + featureLine,
				LineNumber = node.StartLine
			};

			node.AddValue(featureStep);
			CurrentStepKind = kind;
			return node;
		}

		#endregion

		#region Helpers

		private string GetTitle(Node node)
		{
			// Join together the value of the token as well as the FreeLine (if present)
			return string.Join(" ", GetChildValues(node).Cast<string>().ToArray());
		}

		#endregion
	}
}
