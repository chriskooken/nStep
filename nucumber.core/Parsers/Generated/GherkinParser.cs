/*
 * GherkinParser.cs
 *
 * THIS FILE HAS BEEN GENERATED AUTOMATICALLY. DO NOT EDIT!
 */

using System.IO;

using PerCederberg.Grammatica.Runtime;

namespace nStep.Core.Parsers.Generated {

    /**
     * <remarks>A token stream parser.</remarks>
     */
    internal class GherkinParser : RecursiveDescentParser {

        /**
         * <summary>An enumeration with the generated production node
         * identity constants.</summary>
         */
        private enum SynteticPatterns {
            SUBPRODUCTION_1 = 3001,
            SUBPRODUCTION_2 = 3002,
            SUBPRODUCTION_3 = 3003,
            SUBPRODUCTION_4 = 3004
        }

        /**
         * <summary>Creates a new parser with a default analyzer.</summary>
         *
         * <param name='input'>the input stream to read from</param>
         *
         * <exception cref='ParserCreationException'>if the parser
         * couldn't be initialized correctly</exception>
         */
        public GherkinParser(TextReader input)
            : base(input) {

            CreatePatterns();
        }

        /**
         * <summary>Creates a new parser.</summary>
         *
         * <param name='input'>the input stream to read from</param>
         *
         * <param name='analyzer'>the analyzer to parse with</param>
         *
         * <exception cref='ParserCreationException'>if the parser
         * couldn't be initialized correctly</exception>
         */
        public GherkinParser(TextReader input, GherkinAnalyzer analyzer)
            : base(input, analyzer) {

            CreatePatterns();
        }

        /**
         * <summary>Creates a new tokenizer for this parser. Can be overridden
         * by a subclass to provide a custom implementation.</summary>
         *
         * <param name='input'>the input stream to read from</param>
         *
         * <returns>the tokenizer created</returns>
         *
         * <exception cref='ParserCreationException'>if the tokenizer
         * couldn't be initialized correctly</exception>
         */
        protected override Tokenizer NewTokenizer(TextReader input) {
            return new GherkinTokenizer(input);
        }

        /**
         * <summary>Initializes the parser by creating all the production
         * patterns.</summary>
         *
         * <exception cref='ParserCreationException'>if the parser
         * couldn't be initialized correctly</exception>
         */
        private void CreatePatterns() {
            ProductionPattern             pattern;
            ProductionPatternAlternative  alt;

            pattern = new ProductionPattern((int) GherkinConstants.FEATURE,
                                            "Feature");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.FEATURE_HEADER, 1, 1);
            alt.AddProduction((int) GherkinConstants.BLANK_LINE, 0, -1);
            alt.AddProduction((int) GherkinConstants.BACKGROUND, 0, 1);
            alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_1, 0, -1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.FEATURE_HEADER,
                                            "FeatureHeader");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_FEATURE, 1, 1);
            alt.AddProduction((int) GherkinConstants.SUMMARY_LINE, 1, -1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.SUMMARY_LINE,
                                            "SummaryLine");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.FREE_LINE, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.BACKGROUND,
                                            "Background");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.BACKGROUND_HEADER, 1, 1);
            alt.AddProduction((int) GherkinConstants.STEP, 1, -1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.BACKGROUND_HEADER,
                                            "BackgroundHeader");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_BACKGROUND, 1, 1);
            alt.AddProduction((int) GherkinConstants.FREE_LINE, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.SCENARIO,
                                            "Scenario");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.SCENARIO_HEADER, 1, 1);
            alt.AddProduction((int) GherkinConstants.STEP, 1, -1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.SCENARIO_HEADER,
                                            "ScenarioHeader");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_SCENARIO, 1, 1);
            alt.AddProduction((int) GherkinConstants.FREE_LINE, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.SCENARIO_OUTLINE,
                                            "ScenarioOutline");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.SCENARIO_OUTLINE_HEADER, 1, 1);
            alt.AddProduction((int) GherkinConstants.STEP, 1, -1);
            alt.AddProduction((int) GherkinConstants.EXAMPLES, 1, 1);
            alt.AddProduction((int) GherkinConstants.BLANK_LINE, 0, -1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.SCENARIO_OUTLINE_HEADER,
                                            "ScenarioOutlineHeader");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_SCENARIO_OUTLINE, 1, 1);
            alt.AddProduction((int) GherkinConstants.FREE_LINE, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.EXAMPLES,
                                            "Examples");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.EXAMPLES_HEADER, 1, 1);
            alt.AddProduction((int) GherkinConstants.TABLE, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.EXAMPLES_HEADER,
                                            "ExamplesHeader");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_EXAMPLES, 1, 1);
            alt.AddToken((int) GherkinConstants.EOL, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.STEP,
                                            "Step");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.WHITESPACE, 0, 1);
            alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_2, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.BLANK_LINE, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.GIVEN,
                                            "Given");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_GIVEN, 1, 1);
            alt.AddProduction((int) GherkinConstants.FREE_LINE, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.WHEN,
                                            "When");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_WHEN, 1, 1);
            alt.AddProduction((int) GherkinConstants.FREE_LINE, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.THEN,
                                            "Then");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_THEN, 1, 1);
            alt.AddProduction((int) GherkinConstants.FREE_LINE, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.AND,
                                            "And");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_AND, 1, 1);
            alt.AddProduction((int) GherkinConstants.FREE_LINE, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.BUT,
                                            "But");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_BUT, 1, 1);
            alt.AddProduction((int) GherkinConstants.FREE_LINE, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.TABLE,
                                            "Table");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.TABLE_ROW, 1, -1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.TABLE_ROW,
                                            "TableRow");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.PIPE, 1, 1);
            alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_3, 1, -1);
            alt.AddToken((int) GherkinConstants.EOL, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.TABLE_COLUMN,
                                            "TableColumn");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.TEXT, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.WHITESPACE,
                                            "Whitespace");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.HORIZONTAL_WHITESPACE, 1, -1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.FREE_LINE,
                                            "FreeLine");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.TEXT, 0, 1);
            alt.AddToken((int) GherkinConstants.EOL, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.BLANK_LINE,
                                            "BlankLine");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.EOL, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) GherkinConstants.TEXT,
                                            "Text");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.HORIZONTAL_WHITESPACE, 0, -1);
            alt.AddToken((int) GherkinConstants.TEXT_CHAR, 1, 1);
            alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_4, 1, -1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_1,
                                            "Subproduction1");
            pattern.Synthetic = true;
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.SCENARIO, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.SCENARIO_OUTLINE, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_2,
                                            "Subproduction2");
            pattern.Synthetic = true;
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.GIVEN, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.WHEN, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.THEN, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.AND, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.BUT, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_3,
                                            "Subproduction3");
            pattern.Synthetic = true;
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) GherkinConstants.TABLE_COLUMN, 1, 1);
            alt.AddToken((int) GherkinConstants.PIPE, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_4,
                                            "Subproduction4");
            pattern.Synthetic = true;
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.HORIZONTAL_WHITESPACE, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.TEXT_CHAR, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_FEATURE, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_BACKGROUND, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_SCENARIO, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_SCENARIO_OUTLINE, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_EXAMPLES, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_GIVEN, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_WHEN, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_THEN, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_AND, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) GherkinConstants.T_BUT, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);
        }
    }
}
