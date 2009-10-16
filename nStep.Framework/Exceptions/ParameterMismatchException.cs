using System;

namespace nStep.Framework.Exceptions
{
	public class ParameterMismatchException : ApplicationException
	{

		private readonly string _source;

		public ParameterMismatchException(string s, string source) : base (s)
		{
			_source = source;
		}

		public override string StackTrace
		{
			get
			{
				return _source;
			}
		}
	}
}