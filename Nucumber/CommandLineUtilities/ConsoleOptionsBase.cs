using System;
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

                    if (defaultAttribute != null)
                    {
                        if (propertyInfo.PropertyType == typeof(bool))
                            throw new ArgumentException("Default can't be of type bool");
                    
                        propertyParam.IsDefault = true;
                        propertyParam.IsSwitch = false;
                        propertyParam.Switches.Add(DefaultFlag);
                        propertyParameters.Add(DefaultFlag, propertyParam);
                        continue;
                    }

                    propertyParam.IsDefault = false;
                    propertyParam.IsSwitch = (propertyInfo.PropertyType == typeof(Boolean) || propertyInfo.GetType()==typeof(bool));
                    var switchAttribute =
                        (Switch) propertyInfo.GetCustomAttributes(typeof (Switch), true).FirstOrDefault();

                    if (switchAttribute != null)
                    {
                        foreach (var switche in switchAttribute.switches)
                        {
                            propertyParam.Switches.Add(switche);
                            try
                            {
                                propertyParameters.Add(switche, propertyParam);
                            }
                            catch (Exception e)
                            {
                                throw new ArgumentException("Flag overlap");
                            }
                        }
                        continue;
                    }
                    var namedSwitch = propertyParam.Property.Name[0].ToString();
                    propertyParam.Switches.Add(namedSwitch);
                    try
                    {
                        propertyParameters.Add(namedSwitch, propertyParam);
                    }
                    catch (Exception e)
                    {
                        throw new ArgumentException("Flag overlap");
                    }
                    namedSwitch = propertyParam.Property.Name;
                    propertyParam.Switches.Add(namedSwitch);
                    try
                    {
                        propertyParameters.Add(namedSwitch, propertyParam);
                    }
                    catch (Exception e)
                    {
                        throw new ArgumentException("Flag overlap");
                    }
                }
                return propertyParameters;
            }
        }



        public TConsoleOptions Parse<TConsoleOptions>(string[] args) where TConsoleOptions : ConsoleOptionsBase
        {
            if(args.Length == 0 || args[0].Length == 0)
                throw new ArgumentException("Wrong input");
            
            var consoleOptions = this as TConsoleOptions;

            childType = typeof (TConsoleOptions);

            var parameters = GetParameters(args);

            foreach (var pair in parameters)
            {
                //Is parameter with flag
                if (pair.Flag != null && pair.Parameter != null)
                {
                    try
                    {
                        var propertyParam = PropertyParameters[pair.Flag];
                        if(propertyParam.IsEnumerable)
                        {
                            propertyParam.Property.SetValue(consoleOptions,pair.Parameter,null);
                            continue;
                        }
                        propertyParam.Property.SetValue(consoleOptions,pair.Parameter.FirstOrDefault(),null);
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ArgumentException(pair.Flag + " isn't a valid option.");
                    }
                    continue;
                }


                //Is default
                if(pair.Flag == null && pair.Parameter != null)
                {
                    try
                    {
                        var propertyParam = PropertyParameters[DefaultFlag];
                        if (propertyParam.IsEnumerable)
                        {
                            propertyParam.Property.SetValue(consoleOptions, pair.Parameter, null);
                            continue;
                        }
                        propertyParam.Property.SetValue(consoleOptions, pair.Parameter.FirstOrDefault(),null);
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ArgumentException("There is no default option, please use the appropriate flags.");
                    }
                    continue;
                }

                //Is switch
                if (pair.Parameter == null && pair.Flag != null)
                {
                    try
                    {
                        var propertyParam = PropertyParameters[pair.Flag];
                        propertyParam.Property.SetValue(consoleOptions, true, null);
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ArgumentException(pair.Flag + " isn't a valid option.");
                    }
                    continue;
                }   
            }
            return consoleOptions;
        }

        public IList<ParameterPair> GetParameters(string[] args)
        {
            var parameters = new List<ParameterPair>();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Length == 0)
                    throw new ArgumentException("Argument of length zero not allowed");

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
                            throw new ArgumentException("A flag that requires input had no input");
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
                    throw new ArgumentException("Flag not recognized");
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
                    else
                    {
                        parameters.Add(new ParameterPair()
                                           {Flag = null, Parameter = new List<string> {args[i].TrimStart(FlagKey)}});
                    }
                }

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
    }


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

}
