using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Nucumber.Framework;

namespace Nucumber.Core
{
    public class StepMother
    {
		private CombinedStepDefinitions combinedStepDefinitions;
		private IConsoleWriter console;

        public StepMother(IConsoleWriter console, CombinedStepDefinitions stepDefinitions)
		{
		    this.combinedStepDefinitions = stepDefinitions;
			this.console = console;
			
		}
	
        public bool ProcessStep(FeatureStep featureStepToProcess)
        {
            var LineText = featureStepToProcess.FeatureLine;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var consoleOutput = LineText;

            IEnumerable<StepDefinition> results;

            switch (featureStepToProcess.Kind)
            {
                case StepKinds.Given:
                    results = combinedStepDefinitions.GivenStepDefinitions.Where(definition => definition.Regex.IsMatch(LineText));
                    break;
                case StepKinds.When:
                    results = combinedStepDefinitions.WhenStepDefinitions.Where(definition => definition.Regex.IsMatch(LineText));
                    break;
                case StepKinds.Then:
                    results = combinedStepDefinitions.ThenStepDefinitions.Where(definition => definition.Regex.IsMatch(LineText));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            

            if (results.Count() == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                //throw new MissingStepException(LineText);
            }

            if (results.Count() > 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                return false;
                //throw new AmbiguousStepException(LineText);
            }

            if (results.Count() == 1)
            {
                try
                {
                    new StepCaller(results.First(), new TypeCaster()).Call(LineText);
                }
                catch (Exception ex)
                {
					console.WriteException(ex);
					return false;
                }
            }

            console.WriteLineLevel3("    " + consoleOutput);
            return true;
        }

    }
}
