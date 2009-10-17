using System;
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

		public void Execute(IProcessSteps stepProcessor, IProcessScenarioHooks hookProcessor, IFormatOutput outputFormatter)
		{

			if (outputFormatter.SkippingSteps)
			{
				outputFormatter.WriteSkippedFeatureLine(this);
				return;
			}
			outputFormatter.SkippingSteps = true;
			var result = stepProcessor.ProcessStep(this);
			switch (result.ResultCode)
			{
				case StepRunResultCode.Passed:
					outputFormatter.SkippingSteps = false;
					outputFormatter.WritePassedFeatureLine(this, result.MatchedStepDefinition);
					break;
				case StepRunResultCode.Failed:
					outputFormatter.WriteException(this, result.Exception);
					break;
				case StepRunResultCode.Pending:
					outputFormatter.WritePendingFeatureLine(this, result.Exception);
					break;
				case StepRunResultCode.Missing:
					outputFormatter.WriteMissingFeatureLine(this);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void Execute(IProcessSteps stepProcessor, IProcessScenarioHooks hookProcessor, IFormatOutput outputFormatter, IDictionary<string, string> dictionary)
		{
			var newLine = FeatureLine;

			foreach (var key in dictionary.Keys)
				newLine = newLine.Replace("<" + key + ">", dictionary[key]);

			var newFeatureStep = new Step(this) { FeatureLine = newLine };
			newFeatureStep.Execute(stepProcessor, hookProcessor, outputFormatter);
		}

		#endregion
	}
}