/*
 * GherkinTokenizer.cs
 *
 * THIS FILE HAS BEEN GENERATED AUTOMATICALLY. DO NOT EDIT!
 */

using System.IO;

using PerCederberg.Grammatica.Runtime;

namespace nStep.Core.Parsers.Generated {

    /**
     * <remarks>A character stream tokenizer.</remarks>
     */
    internal class GherkinTokenizer : Tokenizer {

        /**
         * <summary>Creates a new tokenizer for the specified input
         * stream.</summary>
         *
         * <param name='input'>the input stream to read</param>
         *
         * <exception cref='ParserCreationException'>if the tokenizer
         * couldn't be initialized correctly</exception>
         */
        public GherkinTokenizer(TextReader input)
            : base(input, false) {

            CreatePatterns();
        }

        /**
         * <summary>Initializes the tokenizer by creating all the token
         * patterns.</summary>
         *
         * <exception cref='ParserCreationException'>if the tokenizer
         * couldn't be initialized correctly</exception>
         */
        private void CreatePatterns() {
            TokenPattern  pattern;

            pattern = new TokenPattern((int) GherkinConstants.T_FEATURE,
                                       "T_FEATURE",
                                       TokenPattern.PatternType.REGEXP,
                                       "Feature:?");
            AddPattern(pattern);

            pattern = new TokenPattern((int) GherkinConstants.T_BACKGROUND,
                                       "T_BACKGROUND",
                                       TokenPattern.PatternType.REGEXP,
                                       "Background:?");
            AddPattern(pattern);

            pattern = new TokenPattern((int) GherkinConstants.T_SCENARIO,
                                       "T_SCENARIO",
                                       TokenPattern.PatternType.REGEXP,
                                       "Scenario:?");
            AddPattern(pattern);

            pattern = new TokenPattern((int) GherkinConstants.T_SCENARIO_OUTLINE,
                                       "T_SCENARIO_OUTLINE",
                                       TokenPattern.PatternType.REGEXP,
                                       "Scenario Outline:?");
            AddPattern(pattern);

            pattern = new TokenPattern((int) GherkinConstants.T_EXAMPLES,
                                       "T_EXAMPLES",
                                       TokenPattern.PatternType.REGEXP,
                                       "Examples:?");
            AddPattern(pattern);

            pattern = new TokenPattern((int) GherkinConstants.T_GIVEN,
                                       "T_GIVEN",
                                       TokenPattern.PatternType.REGEXP,
                                       "Given|given:");
            AddPattern(pattern);

            pattern = new TokenPattern((int) GherkinConstants.T_WHEN,
                                       "T_WHEN",
                                       TokenPattern.PatternType.REGEXP,
                                       "When|when:");
            AddPattern(pattern);

            pattern = new TokenPattern((int) GherkinConstants.T_THEN,
                                       "T_THEN",
                                       TokenPattern.PatternType.REGEXP,
                                       "Then|then:");
            AddPattern(pattern);

            pattern = new TokenPattern((int) GherkinConstants.T_AND,
                                       "T_AND",
                                       TokenPattern.PatternType.REGEXP,
                                       "And|and:");
            AddPattern(pattern);

            pattern = new TokenPattern((int) GherkinConstants.T_BUT,
                                       "T_BUT",
                                       TokenPattern.PatternType.REGEXP,
                                       "But|but:");
            AddPattern(pattern);

            pattern = new TokenPattern((int) GherkinConstants.HORIZONTAL_WHITESPACE,
                                       "HORIZONTAL_WHITESPACE",
                                       TokenPattern.PatternType.REGEXP,
                                       "[\\t ]");
            AddPattern(pattern);

            pattern = new TokenPattern((int) GherkinConstants.EOL,
                                       "EOL",
                                       TokenPattern.PatternType.REGEXP,
                                       "\\r?\\n");
            AddPattern(pattern);

            pattern = new TokenPattern((int) GherkinConstants.COMMENT,
                                       "COMMENT",
                                       TokenPattern.PatternType.REGEXP,
                                       "[\\t ]*#[^\\r^\\n]*\\r?\\n");
            pattern.Ignore = true;
            AddPattern(pattern);

            pattern = new TokenPattern((int) GherkinConstants.TEXT_CHAR,
                                       "TEXT_CHAR",
                                       TokenPattern.PatternType.REGEXP,
                                       "[^\\r^\\n^\\|^\\t^ ]");
            AddPattern(pattern);

            pattern = new TokenPattern((int) GherkinConstants.PIPE,
                                       "PIPE",
                                       TokenPattern.PatternType.STRING,
                                       "|");
            AddPattern(pattern);
        }
    }
}
