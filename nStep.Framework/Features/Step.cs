﻿using System;
using System.Collections.Generic;
using nStep.Framework.Execution;
using nStep.Framework.Execution.Results;
using nStep.Framework.StepDefinitions;

namespace nStep.Framework.Features
{
	public class Step : IExecute
	{
		#region Properties

		public string FeatureLine { get; set; }
		public int LineNumber { get; set; }
		public StepKinds Kind { get; private set; }
		public Table Table { get; private set; }

		#endregion

		#region Constructors

		public Step(StepKinds kind)
			: this(kind, null)
		{ }

		public Step(StepKinds kind, Table table)
		{
			Kind = kind;
			Table = table;
		}

		private Step(Step originalStep)
		{
			FeatureLine = originalStep.FeatureLine;
			LineNumber = originalStep.LineNumber;
			Kind = originalStep.Kind;
			Table = originalStep.Table;
		}

		#endregion

		#region Execution

		public void Execute(StepMother stepMother, IFormatOutput outputFormatter)
		{
			stepMother.CheckForMissingStep(this);

			if (outputFormatter.SkippingSteps)
			{
				outputFormatter.WriteSkippedFeatureLine(this);
				return;
			}
			outputFormatter.SkippingSteps = true;
			switch (stepMother.ProcessStep(this))
			{
				case StepRunResults.Passed:
					outputFormatter.SkippingSteps = false;
					outputFormatter.WritePassedFeatureLine(this, stepMother.LastProcessStepDefinition);
					break;
				case StepRunResults.Failed:
					outputFormatter.WriteException(this, stepMother.LastProcessStepException);
					break;
				case StepRunResults.Pending:
					outputFormatter.WritePendingFeatureLine(this, stepMother.LastProcessStepException);
					break;
				case StepRunResults.Missing:
					outputFormatter.WriteMissingFeatureLine(this);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void Execute(StepMother stepMother, IFormatOutput outputFormatter, IDictionary<string, string> dictionary)
		{
			var newLine = FeatureLine;

			foreach (var key in dictionary.Keys)
				newLine = newLine.Replace("<" + key + ">", dictionary[key]);

			var newFeatureStep = new Step(this) { FeatureLine = newLine };
			newFeatureStep.Execute(stepMother, outputFormatter);
		}

		#endregion
	}
}