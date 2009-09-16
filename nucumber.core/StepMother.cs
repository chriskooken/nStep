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
		private IEnumerable<StepDefinition> stepDefinitions;
		private IConsoleWriter console;
		
		public StepMother(IConsoleWriter console, IEnumerable<StepDefinition> stepDefinitions)
		{
		    this.stepDefinitions = stepDefinitions;
			this.console = console;
			
		}
	
        public bool ProcessStep(FeatureStep featureStepToProcess)
        {
            var LineText = featureStepToProcess.FeatureLine;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var consoleOutput = LineText;

            var results =
                stepDefinitions.Where(definition => definition.Regex.IsMatch(LineText));

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
