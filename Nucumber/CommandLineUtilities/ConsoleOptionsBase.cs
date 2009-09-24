using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Nucumber.App.CommandLineUtilities
{
    public abstract class ConsoleOptionsBase
    {
        public const string DefaultFlag = "#";

        public const char FlagKey = '-';


        private IList<PropertyInfo> classProperties { get; set; }

        private Dictionary<string, PropertyParameter> propertyParameters { get; set; }

        private Type childType { get; set; }

        private IList<PropertyInfo> ClassProperties
        {
            get
            {
                return classProperties ??
                       (classProperties =
                       childType.GetProperties().ToList());
            }
        }

        private Dictionary<string, PropertyParameter> PropertyParameters
        {
            get
            {
                if (propertyParameters != null) return propertyParameters;

                propertyParameters = new Dictionary<string, PropertyParameter>();
                foreach (var propertyInfo in ClassProperties)
                {
                    var propertyParam = new PropertyParameter() {Property = propertyInfo, Switches = new List<string>()};
                    
                    var requiredAttribute =
                        (Required) propertyInfo.GetCustomAttributes(typeof (Required), true).FirstOrDefault();
                    
                    var defaultAttribute =
                        (Default) propertyInfo.GetCustomAttributes(typeof (Default), true).FirstOrDefault();    

                    propertyParam.IsRequired = (requiredAttribute != null);

                    propertyParam.IsEnumerable = IsEnumerable(propertyInfo);

                    propertyParam.IsEnum = propertyInfo.PropertyType.IsEnum;

                    if (defaultAttribute != null)
                    {
                        if (propertyInfo.PropertyType == typeof(bool))
                            throw new ConsoleOptionsException("Default can't be of type bool: " + propertyInfo.Name);

                        propertyParam.IsDefault = true;
                        propertyParam.IsSwitch = false;
                        propertyParam.Switches.Add(DefaultFlag);
                        propertyParameters.Add(DefaultFlag, propertyParam);
                    }
                    else
                    {
                        propertyParam.IsDefault = false;
                        propertyParam.IsSwitch = (propertyInfo.PropertyType == typeof (Boolean) ||
                                                  propertyInfo.GetType() == typeof (bool));
                        var switchAttribute =
                            (Switch) propertyInfo.GetCustomAttributes(typeof (Switch), true).FirstOrDefault();

                        if (switchAttribute != null)
                        {
                            foreach (var switche in switchAttribute.switches)
                            {
                                propertyParam.Switches.Add(switche.ToLower());
                                try
                                {
                                    propertyParameters.Add(switche.ToLower(), propertyParam);
                                }
                                catch (Exception e)
                                {
                                    throw new ConsoleOptionsException("Flag overlap", propertyParam);
                                }
                            }
                            continue;
                        }
                        var namedSwitch = propertyParam.Property.Name[0].ToString().ToLower();

                        try
                        {
                            propertyParam.Switches.Add(namedSwitch);
                            propertyParameters.Add(namedSwitch, propertyParam);
                        }
                        catch (Exception e)
                        {
                            throw new ConsoleOptionsException("Flag overlap",propertyParam);
                        }
                        namedSwitch = propertyParam.Property.Name.ToLower();
                        propertyParam.Switches.Add(namedSwitch);
                        try
                        {
                            propertyParameters.Add(namedSwitch, propertyParam);
                        }
                        catch (Exception e)
                        {
                            throw new ConsoleOptionsException("Flag overlap", propertyParam);
                        }
                    }
                }
                return propertyParameters;
            }
        }




        public TConsoleOptions Parse<TConsoleOptions>(string[] args) where TConsoleOptions : ConsoleOptionsBase
        {
            if(args.Length == 0 || args[0].Length == 0)
                throw new ConsoleOptionsException("No arguments");
            
            var consoleOptions = this as TConsoleOptions;

            childType = typeof (TConsoleOptions);

            var parameters = GetParameters(args);

            foreach (var pair in parameters)
            {
                //Is parameter with flag
                if (pair.Flag != null && pair.Parameter != null)
                {
                    ParameterWithFlag(pair, consoleOptions);
                    continue;
                }


                //Is default
                if(pair.Flag == null && pair.Parameter != null)
                {
                    ParameterDefault(pair, consoleOptions);
                    continue;
                }

                //Is switch
                if (pair.Parameter == null && pair.Flag != null)
                {
                    ParameterSwitch(pair, consoleOptions);
                    continue;
                }
            }
            return consoleOptions;
        }

        private void ParameterSwitch<TConsoleOptions>(ParameterPair pair, TConsoleOptions consoleOptions)
        {
            try
            {
                var propertyParam = PropertyParameters[pair.Flag];
                propertyParam.Property.SetValue(consoleOptions, true, null);
            }
            catch (KeyNotFoundException)
            {
                throw new ConsoleOptionsException("Invalid Switch",PropertyParameters.Values.ToList(),pair);
            }
        }

        private void ParameterDefault<TConsoleOptions>(ParameterPair pair, TConsoleOptions consoleOptions)
        {
            try
            {
                var propertyParam = PropertyParameters[DefaultFlag];
                if (propertyParam.IsEnumerable)
                {
                    propertyParam.Property.SetValue(consoleOptions, pair.Parameter, null);
                    return;
                }
                propertyParam.Property.SetValue(consoleOptions, pair.Parameter.FirstOrDefault(),null);
            }
            catch (KeyNotFoundException)
            {
                throw new ConsoleOptionsException("There is no default option, please use the appropriate flags.",PropertyParameters.Values.ToList(),pair);
            }
        }

        private void ParameterWithFlag<TConsoleOptions>(ParameterPair pair, TConsoleOptions consoleOptions)
        {
            try
            {
                var propertyParam = PropertyParameters[pair.Flag];
                if(propertyParam.IsEnumerable)
                {
                    propertyParam.Property.SetValue(consoleOptions, pair.Parameter, null);

                    return;
                }
                if(propertyParam.IsEnum)
                {
                    propertyParam.Property.SetValue(consoleOptions, Enum.Parse(propertyParam.Property.PropertyType, pair.Parameter.FirstOrDefault()), null);
                }
                else
                {
                    propertyParam.Property.SetValue(consoleOptions, pair.Parameter.FirstOrDefault(), null);
                }
            }
            catch (KeyNotFoundException)
            {
                throw new ConsoleOptionsException("Invalid Switch", PropertyParameters.Values.ToList(), pair);
            }
        }



        public IList<ParameterPair> GetParameters(string[] args)
        {
            var parameters = new List<ParameterPair>();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Length == 0)
                    throw new ConsoleOptionsException("Argument of length zero not allowed");

                if (args[i][0] == FlagKey)
                {
                    if (PropertyParameters.ContainsKey(args[i].TrimStart(FlagKey)))
                    {
                        var propertyParam = PropertyParameters[args[i].TrimStart(FlagKey)];

                        //Parameter is a switch
                        if (propertyParam.IsSwitch)
                        {
                            parameters.Add(new ParameterPair() {Flag = args[i].TrimStart(FlagKey), Parameter = null});
                            continue;
                        }

                        //Parameter is an option
                        if (i >= args.Length - 1)
                            throw new ConsoleOptionsException("A flag that requires input had no input",propertyParam,args[i]);
                        if (propertyParam.IsEnumerable)
                        {
                            int j = i+1;
                            var pair = new ParameterPair() { Flag = args[i].TrimStart(FlagKey), Parameter = new List<string>() };
                            for (; j < args.Length; j++)
                            {
                                if (args[j][0] != FlagKey)
                                {
                                    pair.Parameter.Add(args[j].TrimStart(FlagKey));
                                }else
                                {
                                    break;
                                }
                            }
                            i = j;
                            parameters.Add(pair);
                            continue;
                        }
                        parameters.Add(new ParameterPair() { Flag = args[i].TrimStart(FlagKey), Parameter = new List<string> { args[++i].TrimStart(FlagKey) } });
                        continue;
                    }
                    var x = PropertyParameters;
                    throw new ConsoleOptionsException("Flag not recognized",PropertyParameters.Values.ToList(),args[i]);
                }

                //Parameter is an option with no flag
                if (PropertyParameters.ContainsKey(DefaultFlag))
                {
                    var propertyParam = PropertyParameters[DefaultFlag];
                    if (propertyParam.IsEnumerable)
                    {
                        int j = i;
                        var pair = new ParameterPair() {Flag = null, Parameter = new List<string>()};
                        for (; j < args.Length; j++)
                        {
                            if (args[j][0] != FlagKey)
                            {
                                pair.Parameter.Add(args[j].TrimStart(FlagKey));
                            }
                        }
                        i = j;
                        parameters.Add(pair);
                        continue;
                    }
                    parameters.Add(new ParameterPair()
                                       {Flag = null, Parameter = new List<string> {args[i].TrimStart(FlagKey)}});
                    continue;
                }
                throw new ConsoleOptionsException("There is no Default Option");
            }
            return parameters;
        }
    }

    public class ParameterPair
    {
        public string Flag { get; set; }
        public IList<string> Parameter { get; set; }
    }

    public class PropertyParameter
    {
        public PropertyInfo Property { get; set; }
        public List<string> Switches { get; set; }
        public bool IsDefault { get; set; }
        public bool IsRequired { get; set; }
        public bool IsSwitch { get; set; }
        public bool IsEnumerable { get; set; }
        public bool IsEnum { get; set; }
        public string Help { get; set;}
    }

    
    


    #region attributes

    public class Required : Attribute
    {

    }

    public class Default : Attribute
    {
        
    }

    public class Switch : Attribute
    {
        public IList<string> switches { get; set; }
        public Switch()
        {
            
        }

        public Switch(params string[] switches)
        {
            this.switches = new List<string>(switches);
        }
    }

    public class Help : Attribute
    {
        public string HelpMessage { get; set; }
        public Help(string help)
        {
            HelpMessage = help;
        }
    }

    #endregion

    #region exceptions

    public static class ConsoleOptionsExtensions
    {
        public const string Spacer = "\t\t";

        public static string GetSwitchHelp(this PropertyParameter parameter)
        {
            string help = "";
            parameter.Switches.ForEach(x => help += x + ",");
            return help.TrimEnd(',');
        }

        public static void GenerateHelpMessage(this PropertyParameter parameter)
        {
            var helperAttribute =
                (Help)parameter.Property.GetCustomAttributes(typeof(Default), true).FirstOrDefault();

            var helpString = (helperAttribute != null) ? helperAttribute.HelpMessage : "";
            parameter.Help = parameter.GetSwitchHelp() + "\n" + Spacer + helpString;
        }

        public static string GetHelp(this List<PropertyParameter> parameters)
        {
            return "needs to be implemented";
        }

        public static bool IsEnumerable(this Type PropertyType)
        {
            var genArgs = PropertyType.GetGenericArguments();

            var enumerable = (PropertyType is IEnumerable);


            return PropertyType.IsAssignableFrom(typeof(IList<string>));
        }

    }

    public class ConsoleOptionsException : Exception
    {
        protected string ErrorMessage { get; set; }

        protected string HelperMessage { get; set; }

        public ConsoleOptionsException()
        {
            ErrorMessage = "";
            HelperMessage = "";
        }

        public ConsoleOptionsException(string output) : this()
        {
            ErrorMessage = output;
        }

        public ConsoleOptionsException(string output, PropertyParameter propertyInQuestion): this(output)
        {
            propertyInQuestion.GenerateHelpMessage();

            HelperMessage = propertyInQuestion.Help;
        }
        public ConsoleOptionsException(string output, PropertyParameter property, ParameterPair parameter)
            : this(output)
        {
            property.GenerateHelpMessage();

            HelperMessage = parameter.Flag + " " + parameter.Parameter + " \n\t" + property.Help;
        }
        public ConsoleOptionsException(string output, PropertyParameter property, string input)
            : this(output)
        {
            property.GenerateHelpMessage();
            HelperMessage = input + " \n\t" + property.Help;
        }
        public ConsoleOptionsException(string output, List<PropertyParameter> properties, ParameterPair parameter)
            : this(output)
        {
            HelperMessage = parameter.Flag + " " + parameter.Parameter + " \n\t" + properties.GetHelp();
        }
        public ConsoleOptionsException(string output, List<PropertyParameter> properties, string input)
            : this(output)
        {
            HelperMessage = input + "\n\t" + properties.GetHelp();
        }

        public virtual string OutPut()
        {
            return ErrorMessage + "\n"
                   + "   " + HelperMessage;
        }

    }
    #endregion

}
