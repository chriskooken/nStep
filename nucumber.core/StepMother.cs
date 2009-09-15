using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using nucumber.Framework;

namespace nucumber.core
{
    public class StepMother
    {
		private IDictionary<Step, object> loadedsteps;
		private IConsoleWriter console;
		
		public StepMother(IConsoleWriter console)
		{
			this.console = console;
			loadedsteps = new Dictionary<Step, object>();
		}

        public void LoadStepAssembly(FileInfo assemblyFile)
        {
            foreach (Type t in Assembly.LoadFile(assemblyFile.FullName).GetTypes())
            {
                if (t.IsSubclassOf(typeof(StepBase)) && (t != typeof(StepSetBase<>)))
                {
                    var sm = (IProvideSteps) Activator.CreateInstance(t);
                    loadedsteps = DoSomethingToConnectStepsToFeatureLines(sm.Steps);
                }
            }

        }

		private IDictionary<Step,object> DoSomethingToConnectStepsToFeatureLines(IDictionary<Regex,object> steps)
		{
			return null;
		}
		
        public bool ProcessStep(Step StepToProcess)
        {
            var LineText = StepToProcess.StepText;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var consoleOutput = LineText;
            LineText = Regex.Match(LineText, "(Given|When|Then)(.*)", RegexOptions.Singleline).Groups[2].Value.Trim();

            var results = from result in loadedsteps
                          where Regex.Match(LineText, result.Key.StepText, RegexOptions.Singleline).Success
                          select result;

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
                    MatchAndInvokeStep(LineText, results);
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

        static void MatchAndInvokeStep(string LineText, IEnumerable<KeyValuePair<Step, object>> results)
        {
            var pattern = results.First().Key;
            var groups = Regex.Match(LineText, pattern.StepText, RegexOptions.Singleline).Groups;

            if (groups.Count == 2)
            {
                var methodToInvoke = results.First().Value as Action<string>;
                methodToInvoke.Invoke(groups[1].Value);
            }

            if (groups.Count == 3)
            {
                var methodToInvoke = results.First().Value as Action<string, string>;
                methodToInvoke.Invoke(groups[1].Value, groups[2].Value);
            }

            if (groups.Count == 4)
            {
                var methodToInvoke = results.First().Value as Action<string, string, string>;
                methodToInvoke.Invoke(groups[1].Value, groups[2].Value, groups[3].Value);
            }

            if (groups.Count == 5)
            {
                var methodToInvoke = results.First().Value as Action<string, string, string,string>;
                methodToInvoke.Invoke(groups[1].Value, groups[2].Value, groups[3].Value, groups[4].Value);
            }

        }

        public IDictionary<Step, object> Loadedsteps
        {
            get
            {
                return loadedsteps;
            }
        }

    }
}
