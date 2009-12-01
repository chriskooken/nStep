using System;
using System.Collections.Generic;
using nStep.Framework.Execution;
using nStep.Framework.Execution.Results;
using nStep.Framework.StepDefinitions;

namespace nStep.Framework.Steps
{
	public class Step : IExecute
	{
		#region Properties

		private string _kindWord;
		public string KindWord {
			get
			{
				return _kindWord ?? Kind.ToStringValue();
			}
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

		public void Execute(IProcessSteps stepProcessor, IProcessScenarioHooks hookProcessor)
		{
			var result = stepProcessor.ProcessStep(this);
		}

		public void Execute(IProcessSteps stepProcessor, IProcessScenarioHooks hookProcessor, IDictionary<string, string> dictionary)
		{
			var newBody = Body;

			foreach (var key in dictionary.Keys)
				newBody = newBody.Replace("<" + key + ">", dictionary[key]);

			var newFeatureStep = new Step(this) { Body = newBody };
			newFeatureStep.Execute(stepProcessor, hookProcessor);
		}

		#endregion
	}
}