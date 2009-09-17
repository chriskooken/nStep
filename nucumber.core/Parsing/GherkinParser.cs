using System;
using System.Collections.Generic;
using Spart.Parsers;


namespace Nucumber.Core.Parsing {
	public class GherkinParser : IGherkinParser {
		#region Public Parsers

		public static Parser WhitespaceParser;
		public static Parser CommentParser;
		public static Parser StepParser;
		public static Parser ScenarioParser;

		#endregion

		#region Static Parser Construction

		static GherkinParser()
		{
			CreateParsers();
		}

		private static void CreateParsers()
		{
			WhitespaceParser = Ops.Klenee(Prims.WhiteSpace);
			CommentParser = Ops.Seq(WhitespaceParser, Ops.Seq('#', Prims.Rol));

			var givenParser = Ops.Seq(Ops.Seq(WhitespaceParser, Prims.Str("Given") | Prims.Str("given:")), Prims.Rol);
			var whenParser = Ops.Seq(Ops.Seq(WhitespaceParser, Prims.Str("When") | Prims.Str("when:")), Prims.Rol);
			var thenParser = Ops.Seq(Ops.Seq(WhitespaceParser, Prims.Str("Then") | Prims.Str("then:")), Prims.Rol);
			var andParser = Ops.Seq(Ops.Seq(WhitespaceParser, Prims.Str("And") | Prims.Str("and:") | Prims.Str("But") | Prims.Str("but:")), Prims.Rol);
			StepParser = CommentParser | givenParser | whenParser | thenParser | andParser;

			var backgroundHeaderParser = Ops.Seq(WhitespaceParser, Ops.Seq(Prims.Str("Background:"), Prims.Eol));
			var featureHeaderParser = Ops.Seq(WhitespaceParser, Ops.Seq(Prims.Str("Feature:"), Prims.Rol));
			var scenarioHeaderParser = Ops.Seq(WhitespaceParser, Ops.Seq(Prims.Str("Scenario:"), Prims.Rol));
			var scenarioOutlineHeaderParser = Ops.Seq(WhitespaceParser, Ops.Seq(Prims.Str("Scenario Outline:"), Prims.Rol));

			ScenarioParser = Ops.Seq(scenarioHeaderParser, Ops.Klenee(StepParser));
			var backgroundParser = Ops.Seq(backgroundHeaderParser, Ops.Klenee(StepParser));

			var featureParser = Ops.Seq(featureHeaderParser, Ops.Klenee(Prims.Rol));

		}

		#endregion

		public SimpleTreeNode<LineValue> GetParseTree(string filename) {

			throw new NotImplementedException();
		}
	}
}
