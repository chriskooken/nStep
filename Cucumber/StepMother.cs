using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Cucumber
{
    public class StepMother
    {
        private Dictionary<string, object> loadedsteps;

        public StepMother()
        {
            loadedsteps = new Dictionary<string, object>();
            GetStepClassesFromAssembly();
        }

        private void GetStepClassesFromAssembly()
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.IsSubclassOf(typeof(KeywordBase)))
                {
                    var sm = (KeywordBase)Activator.CreateInstance(t);
                    loadedsteps = (sm.Steps);
                }
            }

        }

        public bool ProcessStep(Step StepToProcess)
        {
            var LineText = StepToProcess.StepText;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var consoleOutput = LineText;
            LineText = Regex.Match(LineText, "(Given|When|Then)(.*)", RegexOptions.Singleline).Groups[2].Value.Trim();

            var results = from result in loadedsteps
                          where Regex.Match(LineText, result.Key, RegexOptions.Singleline).Success
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

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    CConsole.WriteLineLevel3(consoleOutput);
                    CConsole.WriteLineLevel4(ex.StackTrace);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return false;
                }
            }

            CConsole.WriteLineLevel3("    " + consoleOutput);
            Console.ForegroundColor = ConsoleColor.Gray;
            return true;
        }

        static void MatchAndInvokeStep(string LineText, IEnumerable<KeyValuePair<string, object>> results)
        {
            var pattern = results.First().Key;
            var groups = Regex.Match(LineText, pattern, RegexOptions.Singleline).Groups;

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

        public Dictionary<string, object> Loadedsteps
        {
            get
            {
                return loadedsteps;
            }
        }

    }
}
