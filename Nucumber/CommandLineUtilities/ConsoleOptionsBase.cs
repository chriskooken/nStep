using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Nucumber.App.CommandLineUtilities
{
    public abstract class ConsoleOptionsBase
    {
        private PropertyInfo _defaultProperty;

        private IList<PropertyInfo> requiredProperties;

        private IList<PropertyInfo> switchProperties;

        public PropertyInfo DefaultProperty
        {
            get
            {
                return _defaultProperty ??
                    (_defaultProperty =
                        GetType().GetProperties()
                            .Where(p => p.GetCustomAttributes(typeof(Switch), true).Length == 0).ToList().FirstOrDefault());
            }
        }

        public IList<PropertyInfo> RequiredProperties
        {
            get
            {
                return requiredProperties ?? 
                    (requiredProperties =
                    GetType().GetProperties()
                        .Where(p => p.GetCustomAttributes(typeof(Required), true).Length > 0).ToList());
            }   
        }

        public IList<PropertyInfo> SwitchProperties
        {
            get
            {
                return switchProperties ??
                    (switchProperties =
                    GetType().GetProperties()
                        .Where(p => p.GetCustomAttributes(typeof(Switch), true).Length > 0).ToList());
            }
        }

        public List<string> GetBooleanSwitches()
        {
            var switches = new List<string>();
            foreach (var list in SwitchProperties)
            {
                if(list.PropertyType == typeof(bool))
                {
                    var optionAttribute = (Switch)list.GetCustomAttributes(typeof(Switch), true).FirstOrDefault();
                    switches.AddRange(optionAttribute.switches);
                }
            }
            return switches;
        }

        public List<string> GetFlags()
        {
            var flags = new List<string>();
            foreach (var list in SwitchProperties)
            {
                if (list.PropertyType != typeof(bool))
                {
                    var optionAttribute = (Switch)list.GetCustomAttributes(typeof(Switch), true).FirstOrDefault();
                    flags.AddRange(optionAttribute.switches);
                }
            }
            return flags;
        }

        public TConsoleOptions Parse<TConsoleOptions>(string[] args) where TConsoleOptions : ConsoleOptionsBase
        {
            if(args.Length == 0 || args[0].Length == 0)
                throw new ArgumentException("Wrong input");
            
            var consoleOptions = this as TConsoleOptions;

            var parameters = GetParameters(args, GetBooleanSwitches(),GetFlags());

            foreach (var pair in parameters)
            {
                if (pair.Option != null && pair.Parameter != null)
                {
                    var property =
                        switchProperties.Where(x =>
                                               ((Switch) (x.GetCustomAttributes(typeof (Switch), true)).FirstOrDefault())
                                                   .switches.Contains(pair.Option))
                            .FirstOrDefault();
                    if (property == null)
                        throw new ArgumentException("Switch not found");

                    property.SetValue(consoleOptions, pair.Parameter, null);

                    continue;
                }
                if(pair.Option == null && pair.Parameter != null)
                {
                    DefaultProperty.SetValue(consoleOptions, pair.Parameter,null);
                    continue;
                }
                if (pair.Parameter == null && pair.Option != null)
                {
                   switchProperties.Where(x =>
                       ((Switch)(x.GetCustomAttributes(typeof(Switch),true)).FirstOrDefault())
                       .switches.Contains(pair.Option))
                       .FirstOrDefault().SetValue(consoleOptions,true,null);
                    continue;
                }   
            }
            return consoleOptions;
        }

        public static IList<ParameterPair> GetParameters(string[] args, List<string> switches, List<string> flags)
        {
            var parameters = new List<ParameterPair>();
            for (int i = 0; i < args.Length; i++)
            {
                if(args[i].Length == 0)
                    throw new ArgumentException("Argument of length zero not allowed");

                if (args[i][0] == '-')
                {
                    if (switches.Contains(args[i].TrimStart('-')))
                    {
                        parameters.Add(new ParameterPair() { Option = args[i].TrimStart('-'), Parameter = null });
                        continue;
                    }
                    if (flags.Contains(args[i].TrimStart('-')))
                    {
                        if (i >= args.Length -1)
                            throw new ArgumentException("A flag that requires input had no input");
                        parameters.Add(new ParameterPair() { Option = args[i].TrimStart('-'), Parameter = args[++i].TrimStart('-') });
                        continue;
                    }
                    throw new ArgumentException("Flag not recognized");
                }
                parameters.Add(new ParameterPair() { Option = null, Parameter = args[i].TrimStart('-') });
            }
            return parameters;
        }
    }

    public class ParameterPair
    {
        public string Option { get; set; }
        public string Parameter { get; set; }
    }



    public class Required : Attribute
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

    public class Files : Attribute
    {
    }

}
