using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.App.CommandLineUtilities;

namespace nStep.Core
{
    public class ConsoleExceptionHandler
    {
        readonly Action action;

        public ConsoleExceptionHandler(Action action)
        {
            this.action = action;
        }

        public void Execute()
        {
            try
            {
                action.Invoke();
            }
            catch (ConsoleOptionsException exception)
            {
                exception.PrintMessageToConsole();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.ReadKey();
            }
            catch (InvalidScenarioLineNumberException ex)
            {
                WriteException(ex.Message);
            }
            catch (ArgumentException ex)
            {
                WriteException(ex.Message);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;

                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                    Console.WriteLine(ex.InnerException);
                    Console.WriteLine();
                }
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        private static void WriteException(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
