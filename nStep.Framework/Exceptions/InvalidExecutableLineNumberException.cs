using System;

namespace nStep.Framework.Exceptions
{
	public class InvalidExecutableLineNumberException : ApplicationException
	{
		public InvalidExecutableLineNumberException(string message): base(message)
		{
            
		}
	}
}