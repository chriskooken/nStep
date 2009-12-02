using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nStep.Server
{
	public interface IProcessor
	{
		string Process(string request);
	}

	public class Processor : IProcessor
	{
		public string Process(string request)
		{
			throw new NotImplementedException();
		}
	}
}
