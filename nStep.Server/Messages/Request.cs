using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Jayrock.Json;
using Jayrock.Json.Conversion;

namespace nStep.Server.Messages
{
	public abstract class Request
	{
		public static Request ParseFromJson(string json)
		{
			var array = (JsonArray) JsonConvert.Import(json);
			var requestType = (string) array[0];
			var body = (JsonObject) array[1];

			switch (requestType)
			{
				case "begin_scenario":
					return new BeginScenarioRequest();
				case "end_scenario":
					return new EndScenarioRequest();
				case "step_matches":
					var nameToMatch = (string) body["name_to_match"];
					return new StepMatchesRequest(nameToMatch);
				case "invoke":
					var id = new Guid((string) body["id"]);
					var args = ((JsonArray) body["args"]).Cast<string>().ToArray();
					return new InvokeRequest(id, args);
				default:
					throw new NotImplementedException();
			}
		}

		public override bool Equals(object obj)
		{
			return GetType() == obj.GetType();
		}

		public override int GetHashCode()
		{
			return GetType().GetHashCode();
		}
	}

	public class BeginScenarioRequest : Request
	{ }

	public class EndScenarioRequest : Request
	{ }

	public class StepMatchesRequest : Request
	{
		public string NameToMatch { get; private set; }

		public StepMatchesRequest(string nameToMatch)
		{
			NameToMatch = nameToMatch;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj) && NameToMatch == (obj as StepMatchesRequest).NameToMatch;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ NameToMatch.GetHashCode();
		}
	}

	public class InvokeRequest : Request
	{
		public Guid StepId { get; private set; }
		public string[] Arguments { get; private set; }

		public InvokeRequest(Guid stepId, string[] arguments)
		{
			StepId = stepId;
			Arguments = arguments;
		}

		public override bool Equals(object obj)
		{
			var invokeRequest = obj as InvokeRequest;
			return base.Equals(obj) && StepId == invokeRequest.StepId && Arguments == invokeRequest.Arguments;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ StepId.GetHashCode() ^ Arguments.GetHashCode();
		}
	}
}