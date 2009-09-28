using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nucumber.Core.Features;


namespace Nucumber.Core.Parsers {
	public static class GherkinParser  {
		public static Feature GetFeature(string text)
		{
			return GetFeature(new StringReader(text));
		}

		public static Feature GetFeature(FileInfo fileInfo)
		{
			return GetFeature(fileInfo.OpenText());
		}

		public static Feature GetFeature(TextReader reader)
		{
			var parser = new Generated.GherkinParser(reader, new FeatureBuilder());
			var node = parser.Parse();

			var feature = node.Values.ToArray().Single() as Feature;
			return feature;
		}
	}
}
