using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jayrock.Json;
using Jayrock.Json.Conversion;

namespace nStep.Server.Messages
{
	public abstract class Response
	{
		protected abstract object[] ResponseObjects { get; }
		public string JsonText
		{
			get { return JsonConvert.ExportToString(ResponseObjects); }
		}
	}

	public class SuccessResponse : Response
	{
		protected override object[] ResponseObjects
		{
			get { return new[] { "success", null }; }
		}
	}

	public class YikesResponse : Response
	{
		protected override object[] ResponseObjects
		{
			get { return new[] { "yikes" }; }
		}
	}

	public class StepMatchesResponse : Response
	{
		private readonly Guid stepId;
		private readonly IDictionary<string, string> arguments;

		public StepMatchesResponse(Guid stepId, IDictionary<string, string> arguments)
		{
			this.stepId = stepId;
			this.arguments = arguments;
		}

		protected override object[] ResponseObjects
		{
			get
			{
				var argumentArray = new object[] { arguments };
				var data = new object[] { new Dictionary<string, object> {{ "id", stepId },
																		  { "args", argumentArray }} };
				return new object[] { "step_matches", data };
			}
		}
	}

	public class StepFailedResponse : Response
	{
		private readonly string message;

		public StepFailedResponse(string message)
		{
			this.message = message;
		}

		protected override object[] ResponseObjects
		{
			get { return new object[] { "step_failed", new Dictionary<string, string> {{ "message", message }} }; }
		}
	}
}
