using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PerCederberg.Grammatica.Runtime;
using Nucumber.Framework;

namespace Nucumber.Core.Parsers
{
	internal class FeatureBuilder : Generated.GherkinAnalyzer
	{
		#region Private Data

		private StepKinds CurrentStepKind { get; set; }

		#endregion

		#region Tokens

		public override Node ExitEol(Token node)
		{
			return node;
		}

		public override Node ExitTextChar(Token node)
		{
			node.AddValue(node.Image);
			return node;
		}

		public override Node ExitHorizontalWhitespace(Token node)
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
			var childValues = GetChildValues(node);
			var text = "";

			foreach (var obj in childValues)
				text += obj.ToString();

			node.AddValue(text);

			return node;
		}

		#endregion

		#region Feature

		public override Node ExitFeature(Production node)
		{
			var values = GetChildValues(node);

			var summaryLines = values[0] as IList<LineValue>;
			var background = values[1] as Scenario;

			var feature = new Feature
			{
				Background = background
			};

			node.AddValue(feature);
			return node;
		}

		public override Node ExitFeatureHeader(Production node)
		{
			var summaryLines = GetChildValues(node).Cast<string>().ToList();

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

			// First value is title from header
			var title = values[0] as string;

			// Rest of values are FeatureSteps
			var steps = values.GetRange(1, values.Count - 1).Cast<FeatureStep>().ToList();

			var background = new Scenario
			{
				Title = title,
				Steps = steps,
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

			var scenario = new Scenario
			{
				Title = title,
				Steps = steps,
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


			// TODO: Create Scenarios from outline

			return node;
		}

		public override Node ExitExamples(Production node)
		{
			var table = GetChildValues(node).Cast<Table>().Single();

			node.AddValue(table);
			return node;
		}

		public override Node ExitTable(Production node)
		{
			var rows = GetChildValues(node).Cast<Row>().ToList();

			var table = new Table()
			{
				Rows = rows
			};

			node.AddValue(table);
			return node;
		}

		public override Node ExitTableRow(Production node)
		{
			var columns = GetChildValues(node).Cast<Column>().ToList();

			var row = new Row(columns);
			node.AddValue(row);
			return node;
		}

		public override Node ExitTableColumn(Production node)
		{
			var cell = GetChildValues(node).Cast<string>().Single().Trim();
			var column = new Column(cell);
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
			var featureLine = GetChildValues(node).Cast<string>().Single().Trim();

			var featureStep = new FeatureStep
			{
				FeatureLine = featureLine,
				Kind = kind,
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
			var freeLines = GetChildValues(node);
			//return freeLines.Count == 1 ? freeLines[0].ToString().Trim() : null;
			return freeLines.Cast<string>().SingleOrDefault();
		}

		#endregion
	}
}
