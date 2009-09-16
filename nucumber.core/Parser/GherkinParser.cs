using System;
using System.Collections.Generic;
using Spart.Parsers;


namespace Nucumber.Core.Parser {
	public class GherkinParser : IGherkinParser {
		private Spart.Parsers.Parser AnyToEndParser = Ops.Seq(Ops.Klenee(Prims.AnyChar), Prims.Eol);

		public SimpleTreeNode<LineValue> GetParseTree(string filename) {

			throw new NotImplementedException();
		}

		private void BuildParser()
		{
			var commentParser = Ops.Seq(Prims.WhiteSpace, Ops.Seq(Prims.Ch('#'), AnyToEndParser));

			var givenParser = Ops.Seq(Ops.Seq(Prims.WhiteSpace, Prims.Str("Given") | Prims.Str("given:")), AnyToEndParser);
			var whenParser = Ops.Seq(Ops.Seq(Prims.WhiteSpace, Prims.Str("When") | Prims.Str("when:")), AnyToEndParser);
			var thenParser = Ops.Seq(Ops.Seq(Prims.WhiteSpace, Prims.Str("Then") | Prims.Str("then:")), AnyToEndParser);
			var andParser = Ops.Seq(Ops.Seq(Prims.WhiteSpace, Prims.Str("And") | Prims.Str("and:") | Prims.Str("But") | Prims.Str("but:")), AnyToEndParser);
			var stepParser = commentParser | givenParser | whenParser | thenParser | andParser;

			var featureHeaderParser = Ops.Seq(Prims.WhiteSpace, Ops.Seq(Prims.Str("Feature:"), AnyToEndParser));
			var backgroundHeaderParser = Ops.Seq(Prims.WhiteSpace, Ops.Seq(Prims.Str("Background:"), AnyToEndParser));
			var scenarioHeaderParser = Ops.Seq(Prims.WhiteSpace, Ops.Seq(Prims.Str("Scenario:"), AnyToEndParser));
			var scenarioOutlineHeaderParser = Ops.Seq(Prims.WhiteSpace, Ops.Seq(Prims.Str("Scenario Outline:"), AnyToEndParser));
			
			var featureParser = Ops.Seq(featureHeaderParser, Ops.Klenee(stepParser));
			var backgroundParser = Ops.Seq(backgroundHeaderParser, Ops.Klenee(stepParser));
			var scenarioParser = Ops.Seq(scenarioHeaderParser, Ops.Klenee(stepParser));
			var scenarioOutlineParser = Ops.Seq(scenarioOutlineHeaderParser, Ops.Klenee(stepParser));

			var topLevelElementParser = commentParser | featureParser | backgroundParser | scenarioParser | scenarioOutlineParser;
		}
	}
}
