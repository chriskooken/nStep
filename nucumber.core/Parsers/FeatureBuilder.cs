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
			var feature = new Feature
			{

			};

			node.AddValue(feature);
			return node;
		}

		public override Node ExitFeatureHeader(Production node)
		{
			var summaryLines = new List<string>();

			foreach (var summaryLine in GetChildValues(node))
				summaryLines.Add(summaryLine as string);

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
			var steps = new List<FeatureStep>();
			foreach (var step in values.GetRange(1, values.Count - 1))
				steps.Add(step as FeatureStep);

			var background = new Scenario
			{
				Title = title,
				Steps = steps
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
			var steps = new List<FeatureStep>();
			foreach (var step in values.GetRange(1, values.Count - 1))
				steps.Add(step as FeatureStep);

			var scenario = new Scenario
			{
				Title = title,
				Steps = steps
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
			var scenarioOutlineLine = values[0] as string;

			var steps = new List<FeatureStep>();
			foreach (var step in values.GetRange(1, values.Count - 2))
				steps.Add(step as FeatureStep);

			var scenario = new Scenario
			{
				Title = scenarioOutlineLine,
				Steps = steps
			};

			node.AddValue(scenario);
			return node;
		}

		public override Node ExitExamples(Production node)
		{
			return node;
		}

		public override Node ExitTable(Production node)
		{
			var rows = GetChildValues(node);
			var header = rows[0];
			var realRows = rows.GetRange(1, rows.Count - 1);

			//int columnCount = header.

			return node;
		}

		public override Node ExitTableRow(Production node)
		{
			var columns = GetChildValues(node).ToArray();
			node.AddValue(columns);
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
			var featureLine = GetChildValues(node).ToArray()[0].ToString().Trim();

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
			return freeLines.Count == 1 ? freeLines[0].ToString().Trim() : null;
		}

		#endregion
	}
}
