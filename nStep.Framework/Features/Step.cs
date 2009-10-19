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

		private string _kindWord;
		public string KindWord {
			get { return _kindWord; }
			set
			{
				_kindWord = value;
				Kind = KindWord.ToStepKind();
			}
		}

		public string Body { get; set; }
		public string FeatureLine
		{
			get { return KindWord + " " + Body; }
			set
			{
				var index = value.IndexOf(" ");
				KindWord = value.Substring(0, index);
				Body = value.Substring(index).Trim();
			}
		}
		public int LineNumber { get; set; }
		public StepKinds Kind { get; set; }
		public Table Table { get; private set; }

		#endregion

		#region Constructors

		public Step()
			: this(null as Table)
		{ }

		public Step(Table table)
		{
			Table = table;
		}

		private Step(Step originalStep)
		{
			KindWord = originalStep.KindWord;
			Body = originalStep.Body;
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
			var newBody = FeatureLine;

			foreach (var key in dictionary.Keys)
				newBody = newBody.Replace("<" + key + ">", dictionary[key]);

			var newFeatureStep = new Step(this) { Body = newBody };
			newFeatureStep.Execute(stepProcessor, hookProcessor, outputFormatter);
		}

		#endregion
	}
}