using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.Server.Messages;

namespace nStep.Server
{
	public interface IProcessor
	{
		Response Process(Request request);
	}

	public class Processor : IProcessor
	{
		public Response Process(Request request)
		{
			throw new NotImplementedException();
		}
	}
}
