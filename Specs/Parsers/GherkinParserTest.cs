using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using nStep.Core.Parsers;
using NUnit.Framework;

namespace Specs.Parsers
{
	[TestFixture]
	public class GherkinParserTest
	{
		[Test]
		public void It_Parses_Features_Without_Exceptions()
		{
			var filePath = @"..\..\..\example\example4.feature";
			var fileInfo = new FileInfo(filePath);

			try
			{
				var feature = GherkinParser.GetFeature(fileInfo);
			}
			catch (Exception e)
			{
				Assert.Fail();
			}
		}
	}
}
